import { Comparable, Serializer } from "@tsukiy0/tscore";
import { CardId } from "./Card";
import { PersonId } from "./Person";

export class PersonCard implements Comparable {
  constructor(
    public readonly personId: PersonId,
    public readonly cardId: CardId,
  ) {}

  public readonly equals = (input: this): boolean => {
    return (
      this.personId.equals(input.personId) && this.cardId.equals(input.cardId)
    );
  };
}

export type PersonCardJson = {
  personId: string;
  cardId: string;
};

export const PersonCardSerializer: Serializer<PersonCard, PersonCardJson> = {
  serialize: (input: PersonCard) => {
    return {
      personId: input.personId.toString(),
      cardId: input.cardId.toString(),
    };
  },
  deserialize: (input: PersonCardJson) => {
    return new PersonCard(
      PersonId.fromString(input.personId),
      CardId.fromString(input.cardId),
    );
  },
};
