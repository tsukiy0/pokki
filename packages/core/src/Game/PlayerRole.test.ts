import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { PlayerRole, PlayerRoleSerializer } from "./PlayerRole";
import { Role } from "./Role";

describe("PlayerRole", () => {
  testComparable(() => new PlayerRole(UserIdRandomizer.random(), Role.ADMIN));
  testSerializer(
    PlayerRoleSerializer,
    () => new PlayerRole(UserIdRandomizer.random(), Role.ADMIN),
  );
});
