import {
  AddPlayerEvent,
  Card,
  CardIdRandomizer,
  CardSet,
  GameIdRandomizer,
  NewGameEvent,
  UserIdRandomizer,
} from "@pokki/core";
import { testSerializer } from "@tsukiy0/tscore/dist/index.testTemplate";
import {
  AddPlayerEventSerializer,
  NewGameEventSerializer,
} from "./EventSerializer";
import { EventVersion } from "./EventVersion";

describe("NewGameEventSerializer", () => {
  testSerializer(
    new NewGameEventSerializer(new EventVersion(1)),
    () =>
      new NewGameEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        new CardSet([
          new Card(CardIdRandomizer.random(), "card1"),
          new Card(CardIdRandomizer.random(), "card2"),
        ]),
      ),
  );
});

describe("AddPlayerEventSerializer", () => {
  testSerializer(
    new AddPlayerEventSerializer(new EventVersion(1)),
    () =>
      new AddPlayerEvent(GameIdRandomizer.random(), UserIdRandomizer.random()),
  );
});
