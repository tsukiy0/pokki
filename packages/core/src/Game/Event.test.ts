import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { Card, CardIdRandomizer } from "./Card";
import { CardSet } from "./CardSet";
import {
  AddPlayerEvent,
  EndRoundEvent,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
} from "./Event";
import { GameIdRandomizer } from "./Game";

describe("NewGameEvent", () => {
  testComparable(
    () =>
      new NewGameEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        new CardSet([
          new Card(CardIdRandomizer.random(), "aaaa"),
          new Card(CardIdRandomizer.random(), "bbbb"),
        ]),
      ),
  );
});

describe("AddPlayerEvent", () => {
  testComparable(
    () =>
      new AddPlayerEvent(GameIdRandomizer.random(), UserIdRandomizer.random()),
  );
});

describe("NewRoundEvent", () => {
  testComparable(
    () =>
      new NewRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        "aaaa",
      ),
  );
});

describe("PlayCardEvent", () => {
  testComparable(
    () =>
      new PlayCardEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
});

describe("EndRoundEvent", () => {
  testComparable(
    () =>
      new EndRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
});
