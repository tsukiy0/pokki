import { BaseError, Comparable, isArrayEqual } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { PlayerRole } from "./PlayerRole";
import { Role } from "./Role";

export class NoAdminError extends BaseError {}
export class MultipleAdminError extends BaseError {}
export class DuplicatePlayerIdException extends BaseError {}

export class PlayerRoleSet implements Comparable {
  constructor(public readonly items: readonly PlayerRole[]) {
    const numberOfAdmins = items.filter((_) => _.role === Role.ADMIN).length;

    if (numberOfAdmins === 0) {
      throw new NoAdminError();
    }

    if (numberOfAdmins > 1) {
      throw new MultipleAdminError();
    }

    const hasDuplicatePlayerId =
      new Set(items.map((_) => _.playerId.toString())).size !== items.length;

    if (hasDuplicatePlayerId) {
      throw new DuplicatePlayerIdException();
    }
  }

  hasPlayer(id: UserId): boolean {
    return this.items.find((_) => _.playerId.equals(id)) !== undefined;
  }

  equals(input: this): boolean {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  }
}
