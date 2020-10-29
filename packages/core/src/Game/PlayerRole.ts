import { Comparable, Serializer } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { Role, RoleEnumHelper } from "./Role";

export class PlayerRole implements Comparable {
  constructor(public readonly playerId: UserId, public readonly role: Role) {}

  equals(input: this): boolean {
    return this.playerId.equals(input.playerId) && this.role === input.role;
  }
}

export type PlayerRoleJson = {
  playerId: string;
  role: string;
};

export const PlayerRoleSerializer: Serializer<PlayerRole, PlayerRoleJson> = {
  serialize: (input: PlayerRole): PlayerRoleJson => {
    return {
      playerId: input.playerId.toString(),
      role: input.role,
    };
  },
  deserialize: (input: PlayerRoleJson): PlayerRole => {
    return new PlayerRole(
      new UserId(input.playerId),
      RoleEnumHelper.fromString(input.role),
    );
  },
};
