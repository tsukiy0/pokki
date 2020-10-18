using System.Threading.Tasks;
using Core.Game;
using Amazon.DynamoDBv2;

namespace Infrastructure.EventRepository
{
    public class DynamoEventRepository : IEventRepository
    {
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

        public Task AppendNewEvent(NewEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public Task AppendSelectCardEvent(SelectCardEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public Task<EventList> GetEvents(GameId gameId)
        {
            throw new System.NotImplementedException();
        }
    }
}
