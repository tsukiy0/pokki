import { Comparable } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { CardId } from "./Card";
import { CardSet } from "./CardSet";
import { GameId } from "./Game";
import { PlayerCard } from "./PlayerCard";

export abstract class Event implements Comparable {
  constructor(public readonly gameId: GameId) {}

  equals(input: this): boolean {
    return this.gameId.equals(input.gameId);
  }
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
      super.equals(input) &&
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
    return super.equals(input) && this.playerId.equals(input.playerId);
  }
}

export class NewRoundEvent extends Event {}

export class PlayCardEvent extends Event {
  constructor(gameId: GameId, public readonly playerCard: PlayerCard) {
    super(gameId);
  }

  equals(input: this): boolean {
    return super.equals(input) && this.playerCard.equals(this.playerCard);
  }
}

export class EndRoundEvent extends Event {
  constructor(gameId: GameId, public readonly resultCardId: CardId) {
    super(gameId);
  }

  equals(input: this): boolean {
    return super.equals(input) && this.resultCardId.equals(input.resultCardId);
  }
}
