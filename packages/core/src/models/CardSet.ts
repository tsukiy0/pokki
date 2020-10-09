import {
  BaseError,
  Comparable,
  isArrayEqual,
  Serializer,
} from "@tsukiy0/tscore";
import { Card, CardId, CardJson, CardSerializer } from "./Card";

export class DuplicateCardError extends BaseError {}
export class EmptyCardSetError extends BaseError {}
export class CardNotFoundError extends BaseError {
  constructor(public readonly id: CardId) {
    super();
  }
}

export class CardSet implements Comparable {
  constructor(public readonly items: readonly Card[]) {
    if (items.length === 0) {
      throw new EmptyCardSetError();
    }

    const duplicates = items.filter((item, index) => {
      const duplicateIndex = items.findIndex((_) => item.equals(_));
      return duplicateIndex !== index;
    });

    if (duplicates.length !== 0) {
      throw new DuplicateCardError();
    }
  }

  public readonly getCardById = (id: CardId): Card => {
    const found = this.items.find((_) => _.id.equals(id));

    if (!found) {
      throw new CardNotFoundError(id);
    }

    return found;
  };

  public readonly equals = (input: this): boolean => {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  };
}

export type CardSetJson = {
  items: readonly CardJson[];
};

export const CardSetSerializer: Serializer<CardSet, CardSetJson> = {
  serialize: (input) => {
    return {
      items: input.items.map(CardSerializer.serialize),
    };
  },
  deserialize: (input) => {
    return new CardSet(input.items.map(CardSerializer.deserialize));
  },
};
