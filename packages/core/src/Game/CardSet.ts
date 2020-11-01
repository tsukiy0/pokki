import {
  BaseError,
  Comparable,
  isArrayEqual,
  Serializer,
} from "@tsukiy0/tscore";
import { Card, CardId, CardJson, CardSerializer } from "./Card";

export class DuplicateCardIdException extends BaseError {}

export class DuplicateCardNameException extends BaseError {}

export class CardSet implements Comparable {
  constructor(public readonly items: readonly Card[]) {
    const hasDuplicateCardId =
      new Set(items.map((_) => _.id.toString())).size !== items.length;
    const hasDuplicateCardName =
      new Set(items.map((_) => _.name)).size !== items.length;

    if (hasDuplicateCardId) {
      throw new DuplicateCardIdException();
    }

    if (hasDuplicateCardName) {
      throw new DuplicateCardNameException();
    }
  }

  hasCard(id: CardId): boolean {
    return this.items.find((_) => _.id.equals(id)) !== undefined;
  }

  addCard(card: Card): CardSet {
    return new CardSet([...this.items, card]);
  }

  removeCard(id: CardId): CardSet {
    return new CardSet(this.items.filter((_) => !_.id.equals(id)));
  }

  equals(input: this): boolean {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  }
}

export type CardSetJson = CardJson[];

export const CardSetSerializer: Serializer<CardSet, CardSetJson> = {
  serialize: (input: CardSet): CardSetJson => {
    return input.items.map(CardSerializer.serialize);
  },
  deserialize: (input: CardSetJson): CardSet => {
    return new CardSet(input.map(CardSerializer.deserialize));
  },
};
