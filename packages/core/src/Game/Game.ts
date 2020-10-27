import {
  BaseError,
  Comparable,
  EnumHelper,
  ExtendedGuidRandomizer,
  Guid,
  isOptionalEqual,
  Randomizer,
} from "@tsukiy0/tscore";
import { CardSet } from "./CardSet";
import { PlayerRoleSet } from "./PlayerRoleSet";
import { Round } from "./Round";

export class GameId extends Guid {
  private readonly __tag = "GameId";
}

export const GameIdRandomizer: Randomizer<GameId> = new ExtendedGuidRandomizer(
  (_) => new GameId(_),
);

export enum GameStatus {
  ACTIVE = "ACTIVE",
  INACTIVE = "INACTIVE",
}

export const GameStatusEnumHelper = new EnumHelper<GameStatus>(GameStatus);

export class Game implements Comparable {
  constructor(
    public readonly id: GameId,
    public readonly status: GameStatus,
    public readonly cards: CardSet,
    public readonly players: PlayerRoleSet,
    public readonly round?: Round,
  ) {}

  equals(input: this): boolean {
    return (
      this.id.equals(input.id) &&
      this.status === input.status &&
      this.cards.equals(input.cards) &&
      this.players.equals(input.players) &&
      isOptionalEqual(this.round, input.round, (a, b) => a.equals(b))
    );
  }
}
