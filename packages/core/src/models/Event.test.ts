import { DateTimeRandomizer } from "@tsukiy0/tscore";
import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { Card, CardIdRandomizer } from "./Card";
import { CardSet } from "./CardSet";
import {
  AddCardsEvent,
  AddCardsEventSerializer,
  AddPersonEvent,
  AddPersonEventSerializer,
  DecideResultEvent,
  DecideResultEventSerializer,
  EventIdRandomizer,
  NewGameEvent,
  NewGameEventSerializer,
  NewRoundEvent,
  NewRoundEventSerializer,
  PersonSelectCardEvent,
  PersonSelectCardEventSerializer,
} from "./Event";
import { GameIdRandomizer } from "./Game";
import { Person, PersonIdRandomizer } from "./Person";
import { PersonCard } from "./PersonCard";
import { RoundIdRandomizer } from "./Round";

describe("NewGameEvent", () => {
  testComparable(
    () =>
      new NewGameEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        new Person(PersonIdRandomizer.random(), "bill"),
      ),
  );
  testSerializer(
    NewGameEventSerializer,
    () =>
      new NewGameEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        new Person(PersonIdRandomizer.random(), "bill"),
      ),
  );
});

describe("AddPersonEvent", () => {
  testComparable(
    () =>
      new AddPersonEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        new Person(PersonIdRandomizer.random(), "bill"),
      ),
  );
  testSerializer(
    AddPersonEventSerializer,
    () =>
      new AddPersonEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        new Person(PersonIdRandomizer.random(), "bill"),
      ),
  );
});

describe("AddCardsEvent", () => {
  testComparable(
    () =>
      new AddCardsEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        new CardSet([
          new Card(CardIdRandomizer.random(), "XL"),
          new Card(CardIdRandomizer.random(), "M"),
        ]),
      ),
  );
  testSerializer(
    AddCardsEventSerializer,
    () =>
      new AddCardsEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        new CardSet([
          new Card(CardIdRandomizer.random(), "XL"),
          new Card(CardIdRandomizer.random(), "M"),
        ]),
      ),
  );
});

describe("NewRoundEvent", () => {
  testComparable(
    () =>
      new NewRoundEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        RoundIdRandomizer.random(),
      ),
  );
  testSerializer(
    NewRoundEventSerializer,
    () =>
      new NewRoundEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        RoundIdRandomizer.random(),
      ),
  );
});

describe("PersonSelectCardEvent", () => {
  testComparable(
    () =>
      new PersonSelectCardEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        RoundIdRandomizer.random(),
        new PersonCard(PersonIdRandomizer.random(), CardIdRandomizer.random()),
      ),
  );
  testSerializer(
    PersonSelectCardEventSerializer,
    () =>
      new PersonSelectCardEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        RoundIdRandomizer.random(),
        new PersonCard(PersonIdRandomizer.random(), CardIdRandomizer.random()),
      ),
  );
});

describe("DecideResultEvent", () => {
  testComparable(
    () =>
      new DecideResultEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        RoundIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
  testSerializer(
    DecideResultEventSerializer,
    () =>
      new DecideResultEvent(
        EventIdRandomizer.random(),
        DateTimeRandomizer.random(),
        GameIdRandomizer.random(),
        RoundIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
});
