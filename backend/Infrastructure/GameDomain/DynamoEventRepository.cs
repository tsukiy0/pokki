using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Core.GameDomain;
using Core.UserDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.GameDomain
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

        public class ConflictingVersionException : Exception { }

        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public DynamoEventRepository(AmazonDynamoDBClient client, string tableName)
        {
            this.client = client;
            this.tableName = tableName;
        }

        public static IEventRepository Default(string tableName)
        {
            var client = new AmazonDynamoDBClient();
            return new DynamoEventRepository(client, tableName);
        }

        public async Task AppendEvent(Event @event)
        {
            try
            {
                switch (@event)
                {
                    case NewEvent newEvent:
                        await client.PutItemAsync(new PutItemRequest
                        {
                            TableName = tableName,
                            ConditionExpression = "attribute_not_exists(version)",
                            Item = new Dictionary<string, AttributeValue>{
                                {"id", new AttributeValue {S = @event.GameId.Value.ToString()}},
                                {"version", new AttributeValue {N = @event.Version.Value.ToString()}},
                                {"type", new AttributeValue{S = EventType.New.ToString("G")}},
                                {"admin_id", new AttributeValue {S = newEvent.AdminId.Value.ToString()}},
                                {"cards", new AttributeValue {L = newEvent.Cards.Value.Select(_ => {
                                    return new AttributeValue{M = new Dictionary<string, AttributeValue> {
                                        {"id", new AttributeValue{S = _.Id.Value.ToString()}},
                                        {"name", new AttributeValue{S = _.Name}}
                                    }};
                                }).ToList()
                                }}
                            }
                        });
                        break;
                    case AddPlayerEvent addPlayerEvent:
                        await client.PutItemAsync(new PutItemRequest
                        {
                            TableName = tableName,
                            ConditionExpression = "attribute_not_exists(version)",
                            Item = new Dictionary<string, AttributeValue>{
                                {"id", new AttributeValue {S = @event.GameId.Value.ToString()}},
                                {"version", new AttributeValue {N = @event.Version.Value.ToString()}},
                                {"type", new AttributeValue{S = EventType.AddPlayer.ToString("G")}},
                                {"player_id", new AttributeValue {S = addPlayerEvent.PlayerId.Value.ToString()}},
                            }
                        });
                        break;
                    case NewRoundEvent newRoundEvent:
                        await client.PutItemAsync(new PutItemRequest
                        {
                            TableName = tableName,
                            ConditionExpression = "attribute_not_exists(version)",
                            Item = new Dictionary<string, AttributeValue>{
                                {"id", new AttributeValue {S = @event.GameId.Value.ToString()}},
                                {"version", new AttributeValue {N = @event.Version.Value.ToString()}},
                                {"type", new AttributeValue{S = EventType.NewRound.ToString("G")}},
                                {"round_id", new AttributeValue {S = newRoundEvent.RoundId.Value.ToString()}},
                                {"round_name", new AttributeValue {S = newRoundEvent.RoundName}},
                            }
                        });
                        break;
                    case SelectCardEvent selectCardEvent:
                        await client.PutItemAsync(new PutItemRequest
                        {
                            TableName = tableName,
                            ConditionExpression = "attribute_not_exists(version)",
                            Item = new Dictionary<string, AttributeValue>{
                                {"id", new AttributeValue {S = @event.GameId.Value.ToString()}},
                                {"version", new AttributeValue {N = @event.Version.Value.ToString()}},
                                {"type", new AttributeValue{S = EventType.SelectCard.ToString("G")}},
                                {"player_card", new AttributeValue {M = new Dictionary<string, AttributeValue> {
                                    {"player_id", new AttributeValue{S = selectCardEvent.PlayerCard.PlayerId.Value.ToString()}},
                                    {"card_id", new AttributeValue{S = selectCardEvent.PlayerCard.CardId.Value.ToString()}}
                                }}},
                            }
                        });
                        break;
                    case EndRoundEvent endRoundEvent:
                        await client.PutItemAsync(new PutItemRequest
                        {
                            TableName = tableName,
                            ConditionExpression = "attribute_not_exists(version)",
                            Item = new Dictionary<string, AttributeValue>{
                                {"id", new AttributeValue {S = @event.GameId.Value.ToString()}},
                                {"version", new AttributeValue {N = @event.Version.Value.ToString()}},
                                {"type", new AttributeValue{S = EventType.EndRound.ToString("G")}},
                                {"result_card_id", new AttributeValue {S = endRoundEvent.ResultCardId.Value.ToString()}},
                            }
                        });
                        break;
                }
            }
            catch (ConditionalCheckFailedException)
            {
                throw new ConflictingVersionException();
            }
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
                },
                ScanIndexForward = true
            });

            return res.Items.Select<Dictionary<string, AttributeValue>, Event>(item =>
            {
                return (Enum.Parse(typeof(EventType), item["type"].S)) switch
                {
                    EventType.New => new NewEvent(
                        new GameId(Guid.Parse(item["id"].S)),
                        new EventVersion(int.Parse(item["version"].N)),
                        new UserId(Guid.Parse(item["admin_id"].S)),
                        new CardSet(item["cards"].L.Select(_ =>
                        {
                            return new Card(
                                new CardId(Guid.Parse(_.M["id"].S)),
                                _.M["name"].S
                            );
                        }).ToArray())
                    ),
                    EventType.AddPlayer => new AddPlayerEvent(
                        new GameId(Guid.Parse(item["id"].S)),
                        new EventVersion(int.Parse(item["version"].N)),
                        new UserId(Guid.Parse(item["player_id"].S))
                    ),
                    EventType.NewRound => new NewRoundEvent(
                        new GameId(Guid.Parse(item["id"].S)),
                        new EventVersion(int.Parse(item["version"].N)),
                        new RoundId(Guid.Parse(item["round_id"].S)),
                        item["round_name"].S
                    ),
                    EventType.SelectCard => new SelectCardEvent(
                        new GameId(Guid.Parse(item["id"].S)),
                        new EventVersion(int.Parse(item["version"].N)),
                        new PlayerCard(
                            new UserId(Guid.Parse(item["player_card"].M["player_id"].S)),
                            new CardId(Guid.Parse(item["player_card"].M["card_id"].S))
                        )
                    ),
                    EventType.EndRound => new EndRoundEvent(
                        new GameId(Guid.Parse(item["id"].S)),
                        new EventVersion(int.Parse(item["version"].N)),
                        new CardId(Guid.Parse(item["result_card_id"].S))
                    ),
                    _ => throw new NotImplementedException(),
                };
            }).ToArray();
        }
    }
}
