import {
  AddPlayerEvent,
  Card,
  CardIdRandomizer,
  CardSet,
  EndRoundEvent,
  GameIdRandomizer,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
  RoundIdRandomizer,
  UserIdRandomizer,
} from "@pokki/core";
import { testSerializer } from "@tsukiy0/tscore/dist/index.testTemplate";
import {
  AddPlayerEventSerializer,
  EndRoundEventSerializer,
  NewGameEventSerializer,
  NewRoundEventSerializer,
  PlayCardEventSerializer,
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

describe("NewRoundEvent", () => {
  testSerializer(
    new NewRoundEventSerializer(new EventVersion(1)),
    () =>
      new NewRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        RoundIdRandomizer.random(),
        "round1",
      ),
  );
});

describe("PlayCardEvent", () => {
  testSerializer(
    new PlayCardEventSerializer(new EventVersion(1)),
    () =>
      new PlayCardEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
});

describe("EndRoundEvent", () => {
  testSerializer(
    new EndRoundEventSerializer(new EventVersion(1)),
    () =>
      new EndRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
});
