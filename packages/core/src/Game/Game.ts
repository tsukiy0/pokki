import {
  Comparable,
  EnumHelper,
  ExtendedGuidRandomizer,
  Guid,
  isOptionalEqual,
  Randomizer,
  Serializer,
} from "@tsukiy0/tscore";
import { CardSet, CardSetJson, CardSetSerializer } from "./CardSet";
import {
  PlayerRoleSet,
  PlayerRoleSetJson,
  PlayerRoleSetSerializer,
} from "./PlayerRoleSet";
import { Round, RoundJson, RoundSerializer } from "./Round";

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

export type GameJson = {
  id: string;
  status: string;
  cards: CardSetJson;
  players: PlayerRoleSetJson;
  round?: RoundJson;
};

export const GameSerializer: Serializer<Game, GameJson> = {
  serialize: (input: Game): GameJson => {
    return {
      id: input.id.toString(),
      status: input.status,
      cards: CardSetSerializer.serialize(input.cards),
      players: PlayerRoleSetSerializer.serialize(input.players),
      round: input.round ? RoundSerializer.serialize(input.round) : undefined,
    };
  },
  deserialize: (input: GameJson): Game => {
    return new Game(
      new GameId(input.id),
      GameStatusEnumHelper.fromString(input.status),
      CardSetSerializer.deserialize(input.cards),
      PlayerRoleSetSerializer.deserialize(input.players),
      input.round ? RoundSerializer.deserialize(input.round) : undefined,
    );
  },
};
