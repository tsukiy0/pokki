import {
  BaseError,
  Comparable,
  isArrayEqual,
  Serializer,
} from "@tsukiy0/tscore";
import { Person, PersonId, PersonJson, PersonSerializer } from "./Person";

export class DuplicatePersonError extends BaseError {}
export class EmptyPersonSetError extends BaseError {}
export class PersonNotFoundError extends BaseError {
  constructor(public readonly id: PersonId) {
    super();
  }
}

export class PersonSet implements Comparable {
  constructor(public readonly items: readonly Person[]) {
    if (items.length === 0) {
      throw new EmptyPersonSetError();
    }

    const duplicates = items.filter((item, index) => {
      const duplicateIndex = items.findIndex((_) => item.equals(_));
      return duplicateIndex !== index;
    });

    if (duplicates.length !== 0) {
      throw new DuplicatePersonError();
    }
  }

  public readonly getPersonById = (id: PersonId): Person => {
    const found = this.items.find((_) => _.id.equals(id));

    if (!found) {
      throw new PersonNotFoundError(id);
    }

    return found;
  };

  public readonly equals = (input: this): boolean => {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  };
}

export type PersonSetJson = {
  items: readonly PersonJson[];
};

export const PersonSetSerializer: Serializer<PersonSet, PersonSetJson> = {
  serialize: (input) => {
    return {
      items: input.items.map(PersonSerializer.serialize),
    };
  },
  deserialize: (input) => {
    return new PersonSet(input.items.map(PersonSerializer.deserialize));
  },
};
