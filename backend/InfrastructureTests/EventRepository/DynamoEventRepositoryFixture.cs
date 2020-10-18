using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Infrastructure.EventRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfrastructureTests
{
    public class DynamoEventRepositoryFixture : IAsyncDisposable
    {
        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public DynamoEventRepositoryFixture(AmazonDynamoDBClient client, string tableName)
        {
            this.client = client;
            this.tableName = tableName;
        }

        public static async Task<DynamoEventRepositoryFixture> Init(string serviceUrl)
        {
            var tableName = Guid.NewGuid().ToString();
            var client = new AmazonDynamoDBClient(new BasicAWSCredentials("test", "test"), new AmazonDynamoDBConfig
            {
                ServiceURL = serviceUrl,
                // Timeout = TimeSpan.FromSeconds(2)
            });

            await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                KeySchema = new List<KeySchemaElement>{
                    new KeySchemaElement("id" , KeyType.HASH),
                    new KeySchemaElement("version" , KeyType.RANGE)
                },
                AttributeDefinitions = new List<AttributeDefinition>{
                    new AttributeDefinition("id", ScalarAttributeType.S),
                    new AttributeDefinition("version", ScalarAttributeType.N),
                },
                BillingMode = BillingMode.PAY_PER_REQUEST
            });

            return new DynamoEventRepositoryFixture(client, tableName);
        }

        public IEventRepository GetEventRepository()
        {
            return new DynamoEventRepository(client, tableName);
        }

        public async ValueTask DisposeAsync()
        {
            await client.DeleteTableAsync(new DeleteTableRequest
            {
                TableName = tableName
            });
        }
    }
}
