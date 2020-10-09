import {
  BaseError,
  Comparable,
  Guid,
  GuidRandomizer,
  Randomizer,
  Serializer,
} from "@tsukiy0/tscore";

export class PersonId extends Guid {}

export type PersonIdJson = {
  value: string;
};

export const PersonIdSerializer: Serializer<PersonId, PersonIdJson> = {
  serialize: (input) => {
    return {
      value: input.toString(),
    };
  },
  deserialize: (input) => {
    return PersonId.fromString(input.value);
  },
};

export const PersonIdRandomizer: Randomizer<PersonId> = {
  random: (): PersonId => {
    return PersonId.fromString(GuidRandomizer.random().toString());
  },
};

export class PersonNameTooLongError extends BaseError {
  constructor(name: string) {
    super({
      name,
    });
  }
}

export class Person implements Comparable {
  constructor(public readonly id: PersonId, public readonly name: string) {
    if (name.length > 20) {
      throw new PersonNameTooLongError(name);
    }
  }

  public readonly equals = (input: this): boolean => {
    return this.id.equals(input.id) && this.name === input.name;
  };
}

export type PersonJson = {
  id: PersonIdJson;
  name: string;
};

export const PersonSerializer: Serializer<Person, PersonJson> = {
  serialize: (input) => {
    return {
      id: PersonIdSerializer.serialize(input.id),
      name: input.name,
    };
  },
  deserialize: (input) => {
    return new Person(PersonIdSerializer.deserialize(input.id), input.name);
  },
};
