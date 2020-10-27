import {
  BaseError,
  Comparable,
  ExtendedGuidRandomizer,
  Guid,
  Randomizer,
} from "@tsukiy0/tscore";

export class CardId extends Guid {
  private readonly __tag = "CardId";
}

export const CardIdRandomizer: Randomizer<CardId> = new ExtendedGuidRandomizer(
  (_) => new CardId(_),
);

export class BadCardNameError extends BaseError {
  constructor(public readonly name: string) {
    super();
  }
}

export class Card implements Comparable {
  constructor(public readonly id: CardId, public readonly name: string) {
    if (name.length === 0) {
      throw new BadCardNameError(name);
    }

    if (name.length > 8) {
      throw new BadCardNameError(name);
    }
  }

  equals(input: this): boolean {
    return this.id.equals(input.id) && this.name === input.name;
  }
}
