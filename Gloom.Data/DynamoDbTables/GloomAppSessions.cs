using Amazon.DynamoDBv2.DataModel;

namespace Gloom.Data.DynamoDbTables;

[DynamoDBTable("GloomAppSessions")]
public class GloomAppSessions
{
    [DynamoDBHashKey]
    public int Id { get; set; }
    public string Scenario { get; set; }
}