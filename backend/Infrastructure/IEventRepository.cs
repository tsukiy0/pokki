namespace Infrastructure
{
    public interface IEventRepository
    {
        Task AppendNewEvent(NewEvent @event);
        Task AppendAddPlayerEvent(AddPlayerEvent @event);
        Task AppendSelectCardEvent(SelectCardEvent @event);
        Task AppendEndRoundEvent(EndRoundEvent @event);
        Task<EventList> GetEvents(GameId gameId);
    }
}
