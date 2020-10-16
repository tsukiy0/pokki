using System;

namespace Core.Game
{
    public struct CardId
    {
        public readonly Guid Value;

        public CardId(Guid value)
        {
            Value = value;
        }
    }

    public struct Card
    {
        public readonly CardId Id;
        public readonly string Name;

        public Card(CardId id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
