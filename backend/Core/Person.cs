using System;

namespace Core
{
    public class PersonId : Guid
    {
    }

    public class Person
    {
        public readonly PersonId Id;
        public readonly string Name;

        public Person(PersonId id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
