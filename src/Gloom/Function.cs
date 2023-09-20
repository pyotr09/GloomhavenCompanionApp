using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Gloom.Model;
using Gloom.Model.Bosses;
using Gloom.Model.Monsters;
using Gloom.Model.Player_Characters;
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
        // new session
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
            // AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
            // config.ServiceURL = "http://host.docker.internal:8000"; // LOCAL DYNAMODB INSTANCE
            AmazonDynamoDBClient dynamoDbClient = new AmazonDynamoDBClient();
            string tableName = "GloomAppSessions";

            string body = "";
            int sessionId = -1;
            
            if (apigProxyEvent.Path.Equals("/hello"))
            {
                var location = await GetCallingIP();
                body = JsonSerializer.Serialize(new Dictionary<string, string>
                {
                    { "message", "hello world" },
                    { "location", location }
                });
            }


            if (apigProxyEvent.Path.Equals("/newsession"))
            {
                var newId = GenerateId();
                body = JsonSerializer.Serialize(new Dictionary<string, int>
                {
                    { "SessionId", newId }
                });
            }
            
            if (apigProxyEvent.Path.Equals("/setelement"))
            {
                (sessionId, body) = await SetElement(apigProxyEvent, tableName, dynamoDbClient);
            }
            
            if (apigProxyEvent.Path.Equals("/getscenario"))
            {
                (sessionId, body) = await GetScenario(apigProxyEvent, tableName, dynamoDbClient);
            }

            if (apigProxyEvent.Path.Equals("/setscenario"))
            {
                body = SetScenario(apigProxyEvent, body, ref sessionId);
            }

            if (apigProxyEvent.Path.Equals("/addmonster"))
            {
                (sessionId, body) = await AddMonster(apigProxyEvent, tableName, dynamoDbClient);
            }
            
            if (apigProxyEvent.Path.Equals("/addcharacter"))
            {
                (sessionId, body) = await AddCharacter(apigProxyEvent, tableName, dynamoDbClient);
            }
            
            if (apigProxyEvent.Path.Equals("/setinitiative"))
            {
                (sessionId, body) = await SetCharacterInitiative(apigProxyEvent, tableName, dynamoDbClient);
            }
            
            if (apigProxyEvent.Path.Equals("/updatemonsterstate"))
            {
                (sessionId, body) = await UpdateMonsterState(apigProxyEvent, tableName, dynamoDbClient);
            }

            if (apigProxyEvent.Path.Equals("/removemonster"))
            {
                (sessionId, body) = await RemoveMonster(apigProxyEvent, tableName, dynamoDbClient);
            }

            if (apigProxyEvent.Path.Equals("/drawability"))
            {
                (sessionId, body) = await DrawAbility(apigProxyEvent, tableName, dynamoDbClient);
            }
            
            if (apigProxyEvent.Path.Equals("/drawforgroup"))
            {
                (sessionId, body) = await DrawForGroup(apigProxyEvent, tableName, dynamoDbClient);
            }
            
            if (apigProxyEvent.Path.Equals("/endround"))
            {
                (sessionId, body) = await EndRound(apigProxyEvent, tableName, dynamoDbClient);
            }
            
            if (sessionId != -1)
            {
                await UpdateDbScenario(tableName, sessionId, body, dynamoDbClient);
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

        private static async Task<Tuple<int, string>> EndRound(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, int>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": X}

            if (requestBody != null)
            {
                sessionId = requestBody["SessionId"];
                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                scenario.EndRound();
                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            
            return new Tuple<int, string>(sessionId, body);
        }

        private static async Task<Tuple<int, string>> DrawForGroup(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": "X", "GroupName": "..."}

            if (requestBody != null)
            {
                sessionId = int.Parse(requestBody["SessionId"]);
                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                var group = scenario.MonsterGroups.First(g => g.Name == requestBody["GroupName"]);
                if (@group is Boss)
                {
                    (@group as Boss).Activate();
                    if (!scenario.IsBetweenRounds)
                        @group.Draw();
                }
                else
                    @group.Draw();

                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }

        private static async Task<Tuple<int, string>> DrawAbility(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, int>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": X}

            if (requestBody != null)
            {
                sessionId = requestBody["SessionId"];
                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                scenario.Draw();
                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }

        private static async Task<Tuple<int, string>> RemoveMonster(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": "X", "GroupName": "...", "Number": "."}

            if (requestBody != null)
            {
                sessionId = int.Parse(requestBody["SessionId"]);
                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                scenario.RemoveMonster(requestBody["GroupName"], int.Parse(requestBody["Number"]));
                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }

        private static async Task<Tuple<int, string>> UpdateMonsterState(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": "X", "GroupName": "Bandit Guard", "MonsterNumber": "1", "NewHp": "5", 
            // "Statuses": "{"Disarm": true, "Stun": false, etc. for rest of statuses}"
            // }

            if (requestBody != null)
            {
                sessionId = int.Parse(requestBody["SessionId"]);

                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                var groupName = requestBody["GroupName"];
                var num = int.Parse(requestBody["MonsterNumber"]);
                var monster = (scenario.MonsterGroups.First(g => g.Name == groupName)
                        as MonsterGrouping)?
                    .Monsters.First(m => m.MonsterNumber == num);
                if (monster != null)
                {
                    monster.CurrentHitPoints = int.Parse(requestBody["NewHp"]);
                    var statusBody =
                        JsonConvert.DeserializeObject<Dictionary<string, bool>>(requestBody["Statuses"]);
                    foreach (var statusKvp in statusBody)
                    {
                        monster.Statuses.GetStatusByName(statusKvp.Key).IsActive = statusKvp.Value;
                    }
                }

                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }

        private static async Task<Tuple<int, string>> AddMonster(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": "X", "Name": "Bandit Guard", "Tier": "elite", "Number": "1"}

            if (requestBody != null)
            {
                sessionId = int.Parse(requestBody["SessionId"]);
                var tier = requestBody["Tier"] == "elite" ? MonsterTier.Elite : MonsterTier.Normal;
                var number = int.Parse(requestBody["Number"]);

                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                scenario.AddMonster(requestBody["Name"], tier, number);

                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }
        
        private static async Task<Tuple<int, string>> AddCharacter(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": "X", "Name": "Brute", "Level": "1"}

            if (requestBody != null)
            {
                sessionId = int.Parse(requestBody["SessionId"]);
                var level = int.Parse(requestBody["Level"]);

                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                scenario.AddCharacter(requestBody["Name"], level);

                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }
        
        private static async Task<Tuple<int, string>> SetCharacterInitiative(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": "X", "Name": "Brute", "Initiative": "52"}

            if (requestBody != null)
            {
                sessionId = int.Parse(requestBody["SessionId"]);
                var init = int.Parse(requestBody["Initiative"]);

                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                ((Character) scenario.MonsterGroups.First(mg => mg.Name == requestBody["Name"]))
                    .Initiative = init;
                
                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }

        private static string SetScenario(APIGatewayProxyRequest apigProxyEvent, string body, ref int sessionId)
        {
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, int>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting: {"Level": X, "Number": Y, "SessionId": Z}

            if (requestBody != null)
            {
                var level = requestBody["Level"];
                var number = requestBody["Number"];

                var scenario = new Scenario(level, number, "Gloomhaven");
                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                sessionId = requestBody["SessionId"];
            }

            return body;
        }

        private static async Task<Tuple<int, string>> GetScenario(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, int>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": X}
            if (requestBody != null)
            {
                sessionId = requestBody["SessionId"];
                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);
                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }

        private async Task<Tuple<int, string>> SetElement(APIGatewayProxyRequest apigProxyEvent, string tableName,
            AmazonDynamoDBClient dynamoDbClient)
        {
            int sessionId = -1;
            string body = "";
            var requestBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(apigProxyEvent.Body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            // expecting {"SessionId": "X", "Element": "Fire", "SetWaning": "false"}
            if (requestBody != null)
            {
                sessionId = int.Parse(requestBody["SessionId"]);
                var scenario = await GetDbScenario(tableName, dynamoDbClient, sessionId);

                Element e = GetElement(requestBody["Element"]);
                if (requestBody["SetWaning"] != null && bool.Parse(requestBody["SetWaning"]))
                {
                    scenario.SetElementWaning(e);
                }
                else
                {
                    if (scenario.Elements[e] > 0) scenario.ConsumeElement(e);
                    else scenario.InfuseElement(e);
                }

                body = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return new Tuple<int, string>(sessionId, body);
        }

        private Element GetElement(string elString)
        {
            switch (elString)
            {
                case "Fire": return Element.Fire;
                case "Ice" : return Element.Ice;
                case "Earth" : return Element.Earth;
                case "Air" : return Element.Air;
                case "Light" : return Element.Light;
                case "Dark" : return Element.Dark;
            }

            return Element.Any;
        }
        

        private static async Task UpdateDbScenario(string tableName, int sessionId, string scenarioString,
            AmazonDynamoDBClient dynamoDbClient)
        {
            // update -- puts if doesn't already exist
            var updateRequest = new UpdateItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue {N = sessionId.ToString()}}
                },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {"#S", "Scenario"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":scenario", new AttributeValue {S = scenarioString}}
                },
                UpdateExpression = "SET #S = :scenario"
            };
            await dynamoDbClient.UpdateItemAsync(updateRequest);
        }

        private static async Task<Scenario> GetDbScenario(string tableName, AmazonDynamoDBClient dbClient, int sessionId)
        {
            var getRequest = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue> {{"Id", new AttributeValue {N = sessionId.ToString()}}},
                ProjectionExpression = "Id, Scenario"
            };
            var response = await dbClient.GetItemAsync(getRequest);
            var scenarioString = response.Item["Scenario"].S;
            return JsonConvert.DeserializeObject<Scenario>(scenarioString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
        
        private int GenerateId()
        {
            var ticks = DateTime.Now.Ticks % 65535;
            ushort ts = Convert.ToUInt16(ticks);
            var randid = new Random().Next(512);

            var result = ts * 512 + randid;
            return result;
        }
    }
}
