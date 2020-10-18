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
            var newEvent = _value.First() as NewEvent;

            if (newEvent == null)
            {
                throw new NoNewException();
            }

            return _value.Skip(1).Aggregate(Game.New(newEvent), (acc, @event) =>
            {
                switch (@event)
                {
                    case AddPlayerEvent addPlayerEvent:
                        return acc.AddNewPlayer(addPlayerEvent);
                    case NewRoundEvent newRoundEvent:
                        return acc.NewRound(newRoundEvent);
                    case SelectCardEvent selectCardEvent:
                        return acc.SelectCard(selectCardEvent);
                    case EndRoundEvent endRoundEvent:
                        return acc.EndRound(endRoundEvent);
                    default:
                        throw new NotSupportedEventException();
                }
            });
        }
    }
}