using System.Collections.Generic;
using System.Linq;

namespace Core.Game
{
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
    }
}