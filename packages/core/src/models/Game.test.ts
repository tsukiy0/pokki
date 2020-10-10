import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { Card, CardIdRandomizer } from "./Card";
import { CardSet } from "./CardSet";
import { CompletedRoundSet } from "./CompletedRoundSet";
import { Game, GameIdRandomizer, GameSerializer } from "./Game";
import { Person, PersonIdRandomizer } from "./Person";
import { PersonCard } from "./PersonCard";
import { PersonCardSet } from "./PersonCardSet";
import { PersonSet } from "./PersonSet";
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
          new PersonCardSet([new PersonCard(person1.id, card2.id)]),
          card2.id,
        ),
      ]),
    );
  };

  testComparable(() => buildGame());
  testSerializer(GameSerializer, () => buildGame());
});
