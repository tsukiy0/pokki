import { EnumHelper } from "@tsukiy0/tscore";

export enum Role {
  ADMIN = "ADMIN",
  PLAYER = "PLAYER",
}

export const RoleEnumHelper = new EnumHelper<Role>(Role);
