import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { PlayerRole } from "./PlayerRole";
import {
  DuplicatePlayerIdException,
  MultipleAdminError,
  NoAdminError,
  PlayerRoleSet,
} from "./PlayerRoleSet";
import { Role } from "./Role";

describe("PlayerRoleSet", () => {
  testComparable(
    () =>
      new PlayerRoleSet([
        new PlayerRole(UserIdRandomizer.random(), Role.ADMIN),
        new PlayerRole(UserIdRandomizer.random(), Role.PLAYER),
      ]),
  );

  it("throws when no admin", () => {
    expect(
      () =>
        new PlayerRoleSet([
          new PlayerRole(UserIdRandomizer.random(), Role.PLAYER),
          new PlayerRole(UserIdRandomizer.random(), Role.PLAYER),
        ]),
    ).toThrow(NoAdminError);
  });

  it("throws when multiple admin", () => {
    expect(
      () =>
        new PlayerRoleSet([
          new PlayerRole(UserIdRandomizer.random(), Role.ADMIN),
          new PlayerRole(UserIdRandomizer.random(), Role.ADMIN),
        ]),
    ).toThrow(MultipleAdminError);
  });

  it("throws when duplicate player id", () => {
    expect(() => {
      const id = UserIdRandomizer.random();
      new PlayerRoleSet([
        new PlayerRole(id, Role.ADMIN),
        new PlayerRole(id, Role.PLAYER),
      ]);
    }).toThrow(DuplicatePlayerIdException);
  });
});
