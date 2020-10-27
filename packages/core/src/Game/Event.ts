import { Comparable } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { CardSet } from "./CardSet";
import { GameId } from "./Game";

export abstract class Event implements Comparable {
  constructor(public readonly gameId: GameId) {}

  abstract equals(input: this): boolean;
}

export class NewGameEvent extends Event {
  constructor(
    gameId: GameId,
    public readonly adminId: UserId,
    public readonly cards: CardSet,
  ) {
    super(gameId);
  }

  equals(input: this): boolean {
    return (
      this.gameId.equals(input.gameId) &&
      this.adminId.equals(input.adminId) &&
      this.cards.equals(input.cards)
    );
  }
}

export class AddPlayerEvent extends Event {
  constructor(gameId: GameId, public readonly playerId: UserId) {
    super(gameId);
  }

  equals(input: this): boolean {
    return (
      this.gameId.equals(input.gameId) && this.playerId.equals(input.playerId)
    );
  }
}

export class NewRoundEvent extends Event {
  constructor(gameId: GameId, public readonly roundI) {
    super(gameId);
  }

  equals(input: this): boolean {
    return (
      this.gameId.equals(input.gameId) && this.playerId.equals(input.playerId)
    );
  }
}
