import { Serializer } from "@tsukiy0/tscore";
import { PersonCard, PersonCardJson, PersonCardSerializer } from "./PersonCard";
import { Set } from "./Set";

export class PersonCardSet extends Set<PersonCard> {
  constructor(public readonly items: readonly PersonCard[]) {
    super(items, (a, b) => a.equals(b));
  }
}

export type PersonCardSetJson = {
  items: readonly PersonCardJson[];
};

export const PersonCardSetSerializer: Serializer<
  PersonCardSet,
  PersonCardSetJson
> = {
  serialize: (input: PersonCardSet) => {
    return {
      items: input.items.map(PersonCardSerializer.serialize),
    };
  },
  deserialize: (input: PersonCardSetJson) => {
    return new PersonCardSet(input.items.map(PersonCardSerializer.deserialize));
  },
};
