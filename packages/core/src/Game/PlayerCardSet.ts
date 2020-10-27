import { Comparable, isArrayEqual } from "@tsukiy0/tscore";
import { PlayerCard } from "./PlayerCard";

export class DuplicatePlayerIdError extends Error {}

export class PlayerCardSet implements Comparable {
  constructor(public readonly items: readonly PlayerCard[]) {
    const hasDuplicatePlayerId =
      new Set(items.map((_) => _.playerId.toString())).size !== items.length;

    if (hasDuplicatePlayerId) {
      throw new DuplicatePlayerIdError();
    }
  }

  equals(input: this): boolean {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  }
}
