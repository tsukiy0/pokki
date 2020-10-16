using System;

namespace Core
{
    public struct GameId
    {
        public readonly Guid Value;

        public GameId(Guid value)
        {
            Value = value;
        }
    }


    public struct Game
    {
        public readonly GameId Id;
        public readonly PersonId AdminId;
        public readonly NonEmptySet<Person> People;
        public readonly NonEmptySet<Card>? Cards;
        public readonly Round? ActiveRound;
        public readonly Set<Round> FinishedRounds;

        public Game(GameId id, PersonId adminId, NonEmptySet<Person> people, NonEmptySet<Card>? cards, Round? activeRound, Set<Round> finishedRounds)
        {
            Id = id;
            AdminId = adminId;
            People = people;
            Cards = cards;
            ActiveRound = activeRound;
            FinishedRounds = finishedRounds;
        }
    }
}
