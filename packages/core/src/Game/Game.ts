import {
  Comparable,
  ExtendedGuidRandomizer,
  Guid,
  Randomizer,
} from "@tsukiy0/tscore";

export class GameId extends Guid {
  private readonly __tag = "GameId";
}

export const GameIdRandomizer: Randomizer<GameId> = new ExtendedGuidRandomizer(
  (_) => new GameId(_),
);

export class Game implements Comparable {
  constructor(public readonly id: GameId) {}

  equals(input: this): boolean {
    return this.id.equals(input.id);
  }
}
