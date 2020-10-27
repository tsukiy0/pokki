import { BaseError, Comparable, isArrayEqual } from "@tsukiy0/tscore";
import { Card } from "./Card";

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
      throw new DuplicateCardIdException();
    }
  }

  equals(input: this): boolean {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  }
}
