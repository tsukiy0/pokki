import { BaseError, Comparable, Serializer } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { CardId } from "./Card";
import { CardSet, CardSetJson, CardSetSerializer } from "./CardSet";
import { GameId } from "./Game";
import { RoundId } from "./Round";

export abstract class Event implements Comparable {
  constructor(
    public readonly gameId: GameId,
    public readonly playerId: UserId,
  ) {}

  equals(input: this): boolean {
    return (
      this.gameId.equals(input.gameId) && this.playerId.equals(input.playerId)
    );
  }
}

export type EventJson = {
  gameId: string;
  playerId: string;
};

export class NewGameEvent extends Event {
  constructor(
    gameId: GameId,
    playerId: UserId,
    public readonly cards: CardSet,
  ) {
    super(gameId, playerId);
  }

  equals(input: this): boolean {
    return super.equals(input) && this.cards.equals(input.cards);
  }
}

type NewGameEventJson = EventJson & {
  cards: CardSetJson;
};

export const NewGameEventSerializer: Serializer<
  NewGameEvent,
  NewGameEventJson
> = {
  serialize: (input: NewGameEvent): NewGameEventJson => {
    return {
      gameId: input.gameId.toString(),
      playerId: input.playerId.toString(),
      cards: CardSetSerializer.serialize(input.cards),
    };
  },
  deserialize: (input: NewGameEventJson): NewGameEvent => {
    return new NewGameEvent(
      new GameId(input.gameId),
      new UserId(input.playerId),
      CardSetSerializer.deserialize(input.cards),
    );
  },
};

export class AddPlayerEvent extends Event {}

type AddPlayerEventJson = EventJson;

export const AddPlayerEventSerializer: Serializer<
  AddPlayerEvent,
  AddPlayerEventJson
> = {
  serialize: (input: AddPlayerEvent): AddPlayerEventJson => {
    return {
      gameId: input.gameId.toString(),
      playerId: input.playerId.toString(),
    };
  },
  deserialize: (input: AddPlayerEventJson): AddPlayerEvent => {
    return new AddPlayerEvent(
      new GameId(input.gameId),
      new UserId(input.playerId),
    );
  },
};

export class NewRoundEvent extends Event {
  constructor(
    gameId: GameId,
    playerId: UserId,
    public readonly roundId: RoundId,
    public readonly roundName: string,
  ) {
    super(gameId, playerId);
  }

  equals(input: this): boolean {
    return (
      super.equals(input) &&
      this.roundId.equals(input.roundId) &&
      this.roundName === input.roundName
    );
  }
}

type NewRoundEventJson = EventJson & {
  roundId: string;
  roundName: string;
};

export const NewRoundEventSerializer: Serializer<
  NewRoundEvent,
  NewRoundEventJson
> = {
  serialize: (input: NewRoundEvent): NewRoundEventJson => {
    return {
      gameId: input.gameId.toString(),
      playerId: input.playerId.toString(),
      roundId: input.roundId.toString(),
      roundName: input.roundName,
    };
  },
  deserialize: (input: NewRoundEventJson): NewRoundEvent => {
    return new NewRoundEvent(
      new GameId(input.gameId),
      new UserId(input.playerId),
      new RoundId(input.roundId),
      input.roundName,
    );
  },
};

export class PlayCardEvent extends Event {
  constructor(
    gameId: GameId,
    playerId: UserId,
    public readonly cardId: CardId,
  ) {
    super(gameId, playerId);
  }

  equals(input: this): boolean {
    return super.equals(input) && this.cardId.equals(this.cardId);
  }
}

type PlayCardEventJson = EventJson & {
  cardId: string;
};

export const PlayCardEventSerializer: Serializer<
  PlayCardEvent,
  PlayCardEventJson
> = {
  serialize: (input: PlayCardEvent): PlayCardEventJson => {
    return {
      gameId: input.gameId.toString(),
      playerId: input.playerId.toString(),
      cardId: input.cardId.toString(),
    };
  },
  deserialize: (input: PlayCardEventJson): PlayCardEvent => {
    return new PlayCardEvent(
      new GameId(input.gameId),
      new UserId(input.playerId),
      new CardId(input.cardId),
    );
  },
};

export class EndRoundEvent extends Event {
  constructor(
    gameId: GameId,
    playerId: UserId,
    public readonly resultCardId: CardId,
  ) {
    super(gameId, playerId);
  }

  equals(input: this): boolean {
    return super.equals(input) && this.resultCardId.equals(input.resultCardId);
  }
}

type EndRoundEventJson = EventJson & {
  resultCardId: string;
};

export const EndRoundEventSerializer: Serializer<
  EndRoundEvent,
  EndRoundEventJson
> = {
  serialize: (input: EndRoundEvent): EndRoundEventJson => {
    return {
      gameId: input.gameId.toString(),
      playerId: input.playerId.toString(),
      resultCardId: input.resultCardId.toString(),
    };
  },
  deserialize: (input: EndRoundEventJson): EndRoundEvent => {
    return new EndRoundEvent(
      new GameId(input.gameId),
      new UserId(input.playerId),
      new CardId(input.resultCardId),
    );
  },
};

export class NoEventMatchedError extends BaseError {}

export const matchEvent = <T>(
  input: Event,
  actions: {
    newGame: (event: NewGameEvent) => T;
    addPlayer: (event: AddPlayerEvent) => T;
    newRound: (event: NewRoundEvent) => T;
    playCard: (event: PlayCardEvent) => T;
    endRound: (event: EndRoundEvent) => T;
  },
): T => {
  if (input instanceof NewGameEvent) {
    return actions.newGame(input);
  }

  if (input instanceof AddPlayerEvent) {
    return actions.addPlayer(input);
  }

  if (input instanceof NewRoundEvent) {
    return actions.newRound(input);
  }

  if (input instanceof PlayCardEvent) {
    return actions.playCard(input);
  }

  if (input instanceof EndRoundEvent) {
    return actions.endRound(input);
  }

  throw new NoEventMatchedError();
};
