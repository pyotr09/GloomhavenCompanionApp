using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Gloom.Model.Bosses;
using Gloom.Model.Monsters;
using Gloom.Model.Scenario;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gloom
{

    public class Function
    {

        private static readonly HttpClient client = new HttpClient();

        private static async Task<string> GetCallingIP()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

            var msg = await client.GetStringAsync("http://checkip.amazonaws.com/").ConfigureAwait(continueOnCapturedContext:false);

            return msg.Replace("\n","");
        }
        
        // API endpoints:
        // set scenario
        // add monster grouping
        // add monster
        // remove monster
        // add boss
        // add character
        // draw
        // end round

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            string body = "";
            if (apigProxyEvent.Path.Equals("/hello"))
            {
                var location = await GetCallingIP();
                body = JsonSerializer.Serialize(new Dictionary<string, string>
                {
                    { "message", "hello world" },
                    { "location", location }
                });
            }

            if (apigProxyEvent.Path.Equals("/setscenario"))
            {
                var requestBody = JsonConvert.DeserializeObject<Dictionary<string, int>>(apigProxyEvent.Body, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                // expecting: {"Level": X, "Number": Y}

                if (requestBody != null)
                {
                    var level = requestBody["Level"];
                    var number = requestBody["Number"];
                    
                    var scenario = new Scenario(level, number, "Gloomhaven");
                    body = JsonConvert.SerializeObject(scenario, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                }
            }

            if (apigProxyEvent.Path.Equals("/addmonster"))
            {
                var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                // expecting {"Level": "1", "Name": "Bandit Guard", "Tier": "elite", "Number": "1"}

                if (requestBody != null)
                {
                    var tier = requestBody["Tier"] == "elite" ? MonsterTier.Elite : MonsterTier.Normal;
                    var number = int.Parse(requestBody["Number"]);
                    var level = int.Parse(requestBody["Level"]);
                    var monster = new Monster(requestBody["Name"], level, number, tier);
                    body = JsonConvert.SerializeObject(monster, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                }
            }
            
            if (apigProxyEvent.Path.Equals("/drawability"))
            {
                var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                // expecting {"PreviousState": "{...}"}

                if (requestBody != null)
                {
                    var scenario = JsonConvert.DeserializeObject<Scenario>(requestBody["PreviousState"], new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    scenario.Draw();
                    body = JsonConvert.SerializeObject(scenario, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                }
            }
            
            if (apigProxyEvent.Path.Equals("/drawforgroup"))
            {
                var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                // expecting {"PreviousState": "{...}", "GroupName": "..."}

                if (requestBody != null)
                {
                    var scenario = JsonConvert.DeserializeObject<Scenario>(requestBody["PreviousState"], new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    var group = scenario.MonsterGroups.First(g => g.Name == requestBody["GroupName"]);
                    if (group is Boss)
                    {
                        (group as Boss).Activate();
                        if (!scenario.IsBetweenRounds)
                            group.Draw();
                    }
                    else 
                        group.Draw();
                    body = JsonConvert.SerializeObject(scenario, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                }
            }
            
            if (apigProxyEvent.Path.Equals("/endround"))
            {
                var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                // expecting {"PreviousState": "{}"}

                if (requestBody != null)
                {
                    var scenario = JsonConvert.DeserializeObject<Scenario>(requestBody["PreviousState"], new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    scenario.EndRound();
                    body = JsonConvert.SerializeObject(scenario, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                }
            }


            var apiGatewayProxyResponse = new APIGatewayProxyResponse
            {
                Body = body,
                StatusCode = 200,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token" },
                    { "Access-Control-Allow-Methods", "OPTIONS,POST,GET" }
                }
            };
            
            return apiGatewayProxyResponse;
        }
    }
}
