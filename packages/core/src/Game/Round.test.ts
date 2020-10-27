import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { BadRoundNameError, Round, RoundIdRandomizer } from "./Round";

describe("Round", () => {
  testComparable(() => new Round(RoundIdRandomizer.random(), "test"));

  it("throws when name is empty", () => {
    expect(() => new Round(RoundIdRandomizer.random(), "")).toThrow(
      BadRoundNameError,
    );
  });

  it("throws when name is too long", () => {
    expect(() => new Round(RoundIdRandomizer.random(), "123456789")).toThrow(
      BadRoundNameError,
    );
  });
});
