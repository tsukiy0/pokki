import { Comparable } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { CardId } from "./Card";
import { CardSet } from "./CardSet";
import { GameId } from "./Game";

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
    public readonly roundName: string,
  ) {
    super(gameId, playerId);
  }

  equals(input: this): boolean {
    return super.equals(input) && this.roundName === input.roundName;
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
