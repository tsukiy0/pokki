import { Comparable } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { CardId } from "./Card";

export class PlayerCard implements Comparable {
  constructor(
    public readonly playerId: UserId,
    public readonly cardId: CardId,
  ) {}

  equals(input: this): boolean {
    return (
      this.playerId.equals(input.playerId) && this.cardId.equals(input.cardId)
    );
  }
}
