import {
  BaseError,
  Comparable,
  isArrayEqual,
  Serializer,
} from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { PlayerRole, PlayerRoleJson, PlayerRoleSerializer } from "./PlayerRole";
import { Role } from "./Role";

export class NoAdminError extends BaseError {}
export class MultipleAdminError extends BaseError {}
export class DuplicatePlayerRoleError extends BaseError {}

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
      throw new DuplicatePlayerRoleError();
    }
  }

  getPlayer(id: UserId): PlayerRole | undefined {
    return this.items.find((_) => _.playerId.equals(id));
  }

  equals(input: this): boolean {
    return isArrayEqual(this.items, input.items, (a, b) => a.equals(b));
  }
}

export type PlayerRoleSetJson = PlayerRoleJson[];

export const PlayerRoleSetSerializer: Serializer<
  PlayerRoleSet,
  PlayerRoleSetJson
> = {
  serialize: (input: PlayerRoleSet): PlayerRoleSetJson => {
    return input.items.map(PlayerRoleSerializer.serialize);
  },
  deserialize: (input: PlayerRoleSetJson): PlayerRoleSet => {
    return new PlayerRoleSet(input.map(PlayerRoleSerializer.deserialize));
  },
};
