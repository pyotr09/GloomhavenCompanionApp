using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace Gloom;

public class DbHandler
{
    public void TryThings()
    {
        // FOR LOCAL DYNAMODB INSTANCE
        AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
        config.ServiceURL = "http://localhost:8000";
        
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(config);
    }
}