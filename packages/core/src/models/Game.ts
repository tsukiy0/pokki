import {
  Comparable,
  Guid,
  GuidRandomizer,
  Randomizer,
  Serializer,
  Set,
} from "@tsukiy0/tscore";
import {
  CompletedRoundSet,
  CompletedRoundSetJson,
  CompletedRoundSetSerializer,
} from "./CompletedRoundSet";
import { CardSet, CardSetJson, CardSetSerializer } from "./CardSet";
import { PersonSet, PersonSetJson, PersonSetSerializer } from "./PersonSet";
import { ActiveRound, ActiveRoundJson, ActiveRoundSerializer } from "./Round";

export class GameId extends Guid {}

export type GameIdJson = {
  value: string;
};

export const GameIdSerializer: Serializer<GameId, GameIdJson> = {
  serialize: (input) => {
    return {
      value: input.toString(),
    };
  },
  deserialize: (input) => {
    return GameId.fromString(input.value);
  },
};

export const GameIdRandomizer: Randomizer<GameId> = {
  random: (): GameId => {
    return GameId.fromString(GuidRandomizer.random().toString());
  },
};

export class Game implements Comparable {
  constructor(
    public readonly id: GameId,
    public readonly people: PersonSet,
    public readonly cards: CardSet,
    public readonly activeRound: ActiveRound,
    public readonly completedRounds: CompletedRoundSet,
  ) {}

  public readonly equals = (input: this): boolean => {
    return (
      this.id.equals(input.id) &&
      this.people.equals(input.people) &&
      this.cards.equals(input.cards) &&
      this.activeRound.equals(input.activeRound) &&
      this.completedRounds.equals(input.completedRounds)
    );
  };
}

export type GameJson = {
  id: string;
  people: PersonSetJson;
  cards: CardSetJson;
  activeRound: ActiveRoundJson;
  completedRounds: CompletedRoundSetJson;
};

export const GameSerializer: Serializer<Game, GameJson> = {
  serialize: (input: Game) => {
    return {
      id: input.id.toString(),
      people: PersonSetSerializer.serialize(input.people),
      cards: CardSetSerializer.serialize(input.cards),
      activeRound: ActiveRoundSerializer.serialize(input.activeRound),
      completedRounds: CompletedRoundSetSerializer.serialize(
        input.completedRounds,
      ),
    };
  },
  deserialize: (input: GameJson) => {
    return new Game(
      GameId.fromString(input.id),
      PersonSetSerializer.deserialize(input.people),
      CardSetSerializer.deserialize(input.cards),
      ActiveRoundSerializer.deserialize(input.activeRound),
      CompletedRoundSetSerializer.deserialize(input.completedRounds),
    );
  },
};
