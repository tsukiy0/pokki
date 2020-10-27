import { Comparable } from "@tsukiy0/tscore";
import { UserId } from "../User/User";
import { Role } from "./Role";

export class PlayerRole implements Comparable {
  constructor(public readonly playerId: UserId, public readonly role: Role) {}

  equals(input: this): boolean {
    return this.playerId.equals(input.playerId) && this.role === input.role;
  }
}
