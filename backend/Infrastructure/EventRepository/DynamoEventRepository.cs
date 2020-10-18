using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Core.Game;
using Core.Shared;
using Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.EventRepository
{
    public class DynamoEventRepository : IEventRepository
    {
        private enum EventType
        {
            New,
            AddPlayer,
            NewRound,
            SelectCard,
            EndRound
        }

        public class BadEventTypeException : Exception { }

        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public DynamoEventRepository(AmazonDynamoDBClient client, string tableName)
        {
            this.client = client;
            this.tableName = tableName;
        }

        public Task AppendAddPlayerEvent(AddPlayerEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public Task AppendEndRoundEvent(EndRoundEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public async Task AppendNewEvent(NewEvent @event)
        {
            await client.PutItemAsync(new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>{
                    {"id", new AttributeValue {S = @event.GameId.Value.ToString()}},
                    {"version", new AttributeValue {N = @event.Version.Value.ToString()}},
                    {"type", new AttributeValue{S = EventType.New.ToString("G")}},
                    {"admin_id", new AttributeValue {S = @event.AdminId.Value.ToString()}},
                    {"cards", new AttributeValue {L = @event.Cards.Value.Select(_ => {
                        return new AttributeValue{M = new Dictionary<string, AttributeValue> {
                            {"id", new AttributeValue{S = _.Id.Value.ToString()}},
                            {"name", new AttributeValue{S = _.Name}}
                        }};
                    }).ToList()
                    }}
                }
            });
        }

        public Task AppendSelectCardEvent(SelectCardEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IList<Event>> ListEvents(GameId gameId)
        {
            var res = await client.QueryAsync(new QueryRequest
            {
                TableName = tableName,
                KeyConditionExpression = "#hn = :hv",
                ExpressionAttributeNames = new Dictionary<string, string>{
                    {"#hn", "id"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>{
                    {":hv", new AttributeValue{S = gameId.Value.ToString()}}
                }
            });

            return res.Items.Select(item =>
            {
                var p = Enum.Parse(typeof(EventType), item["type"].S);
                return (p) switch
                {
                    EventType.New => new NewEvent(
                        new GameId(Guid.Parse(item["id"].S)),
                        new EventVersion(int.Parse(item["version"].N)),
                        new UserId(Guid.Parse(item["admin_id"].S)),
                        new NonEmptySet<Card>(item["cards"].L.Select(_ =>
                        {
                            return new Card(
                                new CardId(Guid.Parse(_.M["id"].S)),
                                _.M["name"].S
                            );
                        }).ToArray())
                    ),
                    _ => throw new BadEventTypeException(),
                };
            }).ToArray();
        }
    }
}
