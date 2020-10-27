import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { Game, GameIdRandomizer } from "./Game";

describe("Game", () => {
  testComparable(() => new Game(GameIdRandomizer.random()));
});
