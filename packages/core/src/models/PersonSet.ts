import { BaseError, Serializer } from "@tsukiy0/tscore";
import { Person, PersonId, PersonJson, PersonSerializer } from "./Person";
import {
  NonEmptySet,
  NonEmptySetJson,
  NonEmptySetSerializer,
} from "./NonEmptySet";

export class DuplicatePersonError extends BaseError {}
export class EmptyPersonSetError extends BaseError {}
export class PersonNotFoundError extends BaseError {
  constructor(public readonly id: PersonId) {
    super();
  }
}

export class PersonSet extends NonEmptySet<Person> {
  constructor(public readonly items: readonly Person[]) {
    super(items, (a, b) => a.equals(b));
  }

  public readonly getPersonById = (id: PersonId): Person => {
    const found = this.items.find((_) => _.id.equals(id));

    if (!found) {
      throw new PersonNotFoundError(id);
    }

    return found;
  };
}

export type PersonSetJson = NonEmptySetJson<PersonJson>;

export const PersonSetSerializer: Serializer<
  PersonSet,
  PersonSetJson
> = new NonEmptySetSerializer(
  PersonSerializer,
  (input) => new PersonSet(input),
);
