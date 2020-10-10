import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { Card, CardIdRandomizer } from "./Card";
import { CardNotFoundError, CardSet } from "./CardSet";
import { CompletedRoundSet } from "./CompletedRoundSet";
import {
  CompletedRoundMissingPersonError,
  Game,
  GameIdRandomizer,
  GameSerializer,
} from "./Game";
import { Person, PersonIdRandomizer } from "./Person";
import { PersonCard } from "./PersonCard";
import { PersonCardSet } from "./PersonCardSet";
import { PersonNotFoundError, PersonSet } from "./PersonSet";
import { ActiveRound, CompletedRound, RoundIdRandomizer } from "./Round";

describe("Game", () => {
  const buildGame = () => {
    const person1 = new Person(PersonIdRandomizer.random(), "bob");
    const person2 = new Person(PersonIdRandomizer.random(), "john");
    const card1 = new Card(CardIdRandomizer.random(), "M");
    const card2 = new Card(CardIdRandomizer.random(), "L");

    return new Game(
      GameIdRandomizer.random(),
      new PersonSet([person1, person2]),
      new CardSet([card1, card2]),
      new ActiveRound(RoundIdRandomizer.random(), new PersonCardSet([])),
      new CompletedRoundSet([
        new CompletedRound(
          RoundIdRandomizer.random(),
          new PersonCardSet([
            new PersonCard(person1.id, card1.id),
            new PersonCard(person2.id, card2.id),
          ]),
          card1.id,
        ),
        new CompletedRound(
          RoundIdRandomizer.random(),
          new PersonCardSet([
            new PersonCard(person1.id, card2.id),
            new PersonCard(person2.id, card2.id),
          ]),
          card2.id,
        ),
      ]),
    );
  };

  testComparable(() => buildGame());
  testSerializer(GameSerializer, () => buildGame());

  it("throws when active round references person that does not exist", () => {
    expect(() => {
      const person = new Person(PersonIdRandomizer.random(), "bob");
      const card = new Card(CardIdRandomizer.random(), "M");

      new Game(
        GameIdRandomizer.random(),
        new PersonSet([person]),
        new CardSet([card]),
        new ActiveRound(
          RoundIdRandomizer.random(),
          new PersonCardSet([
            new PersonCard(PersonIdRandomizer.random(), card.id),
          ]),
        ),
        new CompletedRoundSet([]),
      );
    }).toThrowError(PersonNotFoundError);
  });

  it("throws when active round references card that does not exist", () => {
    expect(() => {
      const person = new Person(PersonIdRandomizer.random(), "bob");
      const card = new Card(CardIdRandomizer.random(), "M");

      return new Game(
        GameIdRandomizer.random(),
        new PersonSet([person]),
        new CardSet([card]),
        new ActiveRound(
          RoundIdRandomizer.random(),
          new PersonCardSet([
            new PersonCard(person.id, CardIdRandomizer.random()),
          ]),
        ),
        new CompletedRoundSet([]),
      );
    }).toThrowError(CardNotFoundError);
  });

  it("throws when completed round missing person", () => {
    expect(() => {
      const person1 = new Person(PersonIdRandomizer.random(), "bob");
      const person2 = new Person(PersonIdRandomizer.random(), "john");
      const card1 = new Card(CardIdRandomizer.random(), "M");
      const card2 = new Card(CardIdRandomizer.random(), "L");

      new Game(
        GameIdRandomizer.random(),
        new PersonSet([person1, person2]),
        new CardSet([card1, card2]),
        new ActiveRound(RoundIdRandomizer.random(), new PersonCardSet([])),
        new CompletedRoundSet([
          new CompletedRound(
            RoundIdRandomizer.random(),
            new PersonCardSet([new PersonCard(person1.id, card2.id)]),
            card2.id,
          ),
        ]),
      );
    }).toThrowError(CompletedRoundMissingPersonError);
  });

  it("throws when completed round result references card that does not exist", () => {
    expect(() => {
      const person1 = new Person(PersonIdRandomizer.random(), "bob");
      const person2 = new Person(PersonIdRandomizer.random(), "john");
      const card1 = new Card(CardIdRandomizer.random(), "M");
      const card2 = new Card(CardIdRandomizer.random(), "L");

      new Game(
        GameIdRandomizer.random(),
        new PersonSet([person1, person2]),
        new CardSet([card1, card2]),
        new ActiveRound(RoundIdRandomizer.random(), new PersonCardSet([])),
        new CompletedRoundSet([
          new CompletedRound(
            RoundIdRandomizer.random(),
            new PersonCardSet([
              new PersonCard(person1.id, card2.id),
              new PersonCard(person2.id, card1.id),
            ]),
            CardIdRandomizer.random(),
          ),
        ]),
      );
    }).toThrowError(CardNotFoundError);
  });
});
