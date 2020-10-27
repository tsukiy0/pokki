import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { PlayerRole } from "./PlayerRole";
import { Role } from "./Role";

describe("PlayerRole", () => {
  testComparable(() => new PlayerRole(UserIdRandomizer.random(), Role.ADMIN));
});
