import { BaseError, Comparable } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { CardId } from "./Card";
import { CardSet } from "./CardSet";
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

export class AddPlayerEvent extends Event {}

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
