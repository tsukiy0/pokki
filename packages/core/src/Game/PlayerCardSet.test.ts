import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { CardIdRandomizer } from "./Card";
import { PlayerCard } from "./PlayerCard";
import {
  DuplicatePlayerCardError,
  PlayerCardSet,
  PlayerCardSetSerializer,
} from "./PlayerCardSet";

describe("PlayerCardSet", () => {
  testComparable(
    () =>
      new PlayerCardSet([
        new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
        new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
      ]),
  );

  testSerializer(
    PlayerCardSetSerializer,
    () =>
      new PlayerCardSet([
        new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
        new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
      ]),
  );

  it("throw when duplicate player id", () => {
    expect(() => {
      const id = UserIdRandomizer.random();
      new PlayerCardSet([
        new PlayerCard(id, CardIdRandomizer.random()),
        new PlayerCard(id, CardIdRandomizer.random()),
      ]);
    }).toThrow(DuplicatePlayerCardError);
  });
});
