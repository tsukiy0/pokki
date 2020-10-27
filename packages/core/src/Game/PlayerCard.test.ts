import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { CardIdRandomizer } from "./Card";
import { PlayerCard } from "./PlayerCard";

describe("PlayerCard", () => {
  testComparable(
    () => new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
  );
});
