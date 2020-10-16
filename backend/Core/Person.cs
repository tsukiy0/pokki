﻿using System;

namespace Core
{
    public struct PersonId
    {
        public readonly Guid Value;

        public PersonId(Guid value)
        {
            Value = value;
        }
    }

    public struct Person
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