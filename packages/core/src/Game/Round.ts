import {
  BaseError,
  Comparable,
  ExtendedGuidRandomizer,
  Guid,
  Randomizer,
} from "@tsukiy0/tscore";
import { PlayerCardSet } from "./PlayerCardSet";

export class RoundId extends Guid {
  private readonly __tag = "RoundId";
}

export const RoundIdRandomizer: Randomizer<RoundId> = new ExtendedGuidRandomizer(
  (_) => new RoundId(_),
);

export class BadRoundNameError extends BaseError {
  constructor(public readonly name: string) {
    super();
  }
}

export class Round implements Comparable {
  constructor(
    public readonly id: RoundId,
    public readonly name: string,
    public readonly playerCards: PlayerCardSet,
  ) {
    if (name.length === 0) {
      throw new BadRoundNameError(name);
    }

    if (name.length > 8) {
      throw new BadRoundNameError(name);
    }
  }

  equals(input: this): boolean {
    return this.id.equals(input.id) && this.name === input.name;
  }
}
