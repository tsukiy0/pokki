using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Core.UserDomain;

namespace Infrastructure.UserDomain
{
    public class DynamoUserRepository : IUserRepository
    {
        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public DynamoUserRepository(AmazonDynamoDBClient client, string tableName)
        {
            this.client = client;
            this.tableName = tableName;
        }

        public async Task CreateUser(User user)
        {
            await client.PutItemAsync(new PutItemRequest
            {
                TableName = tableName,
                ConditionExpression = "attribute_not_exists(version)",
                Item = new Dictionary<string, AttributeValue>{
                    {"id", new AttributeValue {S = user.Id.Value.ToString()}},
                    {"name", new AttributeValue{S = user.Name}}
                }
            });
        }

        public async Task<User> GetUser(UserId id)
        {
            var res = await client.GetItemAsync(new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue> {
                    {"id", new AttributeValue { S = id.Value.ToString()}}
                }
            });

            if (res.Item == null)
            {
                throw new UserNotFoundException();
            }

            return new User(
                new UserId(Guid.Parse(res.Item["id"].S)),
                res.Item["name"].S
            );
        }
    }
}
