import { Comparable, isArrayEqual, Serializer } from "@tsukiy0/tscore";
import { PlayerCard, PlayerCardJson, PlayerCardSerializer } from "./PlayerCard";

export class DuplicatePlayerCardError extends Error {}

export class PlayerCardSet implements Comparable {
  constructor(public readonly items: readonly PlayerCard[]) {
    const hasDuplicatePlayerId =
      new Set(items.map((_) => _.playerId.toString())).size !== items.length;

    if (hasDuplicatePlayerId) {
      throw new DuplicatePlayerCardError();
    }
  }

  equals(input: this): boolean {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  }
}

export type PlayerCardSetJson = {
  items: PlayerCardJson[];
};

export const PlayerCardSetSerializer: Serializer<
  PlayerCardSet,
  PlayerCardSetJson
> = {
  serialize: (input: PlayerCardSet): PlayerCardSetJson => {
    return {
      items: input.items.map(PlayerCardSerializer.serialize),
    };
  },
  deserialize: (input: PlayerCardSetJson): PlayerCardSet => {
    return new PlayerCardSet(input.items.map(PlayerCardSerializer.deserialize));
  },
};
