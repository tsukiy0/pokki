using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Game
{
    public class NoNewException : Exception { }
    public class MultipleNewException : Exception { }
    public class NotSupportedEventException : Exception { }

    public struct EventList
    {
        private readonly IList<Event> _value;

        public EventList(IList<Event> value)
        {
            if (!(value.First() is NewEvent))
            {
                throw new NoNewException();
            }

            if (value.Skip(1).Where(_ => _ is NewEvent).Any())
            {
                throw new MultipleNewException();
            }

            _value = value;
        }

        public Game Reduce()
        {
            if (!(_value.First() is NewEvent newEvent))
            {
                throw new NoNewException();
            }

            return _value.Skip(1).Aggregate(Game.New(newEvent), (acc, @event) =>
            {
                return @event switch
                {
                    AddPlayerEvent addPlayerEvent => acc.AddPlayer(addPlayerEvent),
                    NewRoundEvent newRoundEvent => acc.NewRound(newRoundEvent),
                    SelectCardEvent selectCardEvent => acc.SelectCard(selectCardEvent),
                    EndRoundEvent endRoundEvent => acc.EndRound(endRoundEvent),
                    _ => throw new NotSupportedEventException(),
                };
            });
        }
    }
}
