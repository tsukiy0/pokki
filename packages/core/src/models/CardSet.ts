import { BaseError, Serializer, NonEmptySet } from "@tsukiy0/tscore";
import { Card, CardId, CardJson, CardSerializer } from "./Card";

export class DuplicateCardError extends BaseError {}
export class EmptyCardSetError extends BaseError {}
export class CardNotFoundError extends BaseError {
  constructor(public readonly id: CardId) {
    super();
  }
}

export class CardSet extends NonEmptySet<Card> {
  constructor(public readonly items: readonly Card[]) {
    super(items, (a, b) => a.equals(b));
  }

  public readonly getCardById = (id: CardId): Card => {
    const found = this.items.find((_) => _.id.equals(id));

    if (!found) {
      throw new CardNotFoundError(id);
    }

    return found;
  };
}

export type CardSetJson = {
  items: CardJson[];
};

export const CardSetSerializer: Serializer<CardSet, CardSetJson> = {
  serialize: (input: CardSet) => {
    return {
      items: input.items.map(CardSerializer.serialize),
    };
  },
  deserialize: (input: CardSetJson) => {
    return new CardSet(input.items.map(CardSerializer.deserialize));
  },
};
