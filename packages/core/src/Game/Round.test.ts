import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { CardIdRandomizer } from "./Card";
import { PlayerCard } from "./PlayerCard";
import { PlayerCardSet } from "./PlayerCardSet";
import {
  BadRoundNameError,
  Round,
  RoundIdRandomizer,
  RoundSerializer,
} from "./Round";

describe("Round", () => {
  testComparable(
    () =>
      new Round(
        RoundIdRandomizer.random(),
        "round1",
        new PlayerCardSet([
          new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
          new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
        ]),
      ),
  );

  testSerializer(
    RoundSerializer,
    () =>
      new Round(
        RoundIdRandomizer.random(),
        "round1",
        new PlayerCardSet([
          new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
          new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
        ]),
      ),
  );

  it("throws when name is empty", () => {
    expect(
      () => new Round(RoundIdRandomizer.random(), "", new PlayerCardSet([])),
    ).toThrow(BadRoundNameError);
  });

  it("throws when name is too long", () => {
    expect(
      () =>
        new Round(
          RoundIdRandomizer.random(),
          "123456789",
          new PlayerCardSet([]),
        ),
    ).toThrow(BadRoundNameError);
  });
});
