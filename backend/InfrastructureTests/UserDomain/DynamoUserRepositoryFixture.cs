using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Core.UserDomain;
using Infrastructure.Config;
using Infrastructure.UserDomain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfrastructureTests
{
    public class DynamoUserRepositoryFixture : IAsyncDisposable
    {
        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public DynamoUserRepositoryFixture(AmazonDynamoDBClient client, string tableName)
        {
            this.client = client;
            this.tableName = tableName;
        }

        public static async Task<DynamoUserRepositoryFixture> Init()
        {
            var config = new SystemConfig();
            var tableName = Guid.NewGuid().ToString();
            var client = new AmazonDynamoDBClient(new BasicAWSCredentials("test", "test"), new AmazonDynamoDBConfig
            {
                ServiceURL = config.Get("DYNAMO_URL")
            });

            await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                KeySchema = new List<KeySchemaElement>{
                    new KeySchemaElement("id" , KeyType.HASH)
                },
                AttributeDefinitions = new List<AttributeDefinition>{
                    new AttributeDefinition("id", ScalarAttributeType.S)
                },
                BillingMode = BillingMode.PAY_PER_REQUEST
            });

            return new DynamoUserRepositoryFixture(client, tableName);
        }

        public IUserRepository GetUserRepository()
        {
            return new DynamoUserRepository(client, tableName);
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
