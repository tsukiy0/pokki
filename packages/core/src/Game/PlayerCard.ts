import { Comparable, Serializer } from "@tsukiy0/tscore";
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

export type PlayerCardJson = {
  playerId: string;
  cardId: string;
};

export const PlayerCardSerializer: Serializer<PlayerCard, PlayerCardJson> = {
  serialize: (input: PlayerCard): PlayerCardJson => {
    return {
      playerId: input.playerId.toString(),
      cardId: input.cardId.toString(),
    };
  },
  deserialize: (input: PlayerCardJson): PlayerCard => {
    return new PlayerCard(new UserId(input.playerId), new CardId(input.cardId));
  },
};
