import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { CardIdRandomizer } from "./Card";
import {
  CompletedRound,
  CompletedRoundSerializer,
  ActiveRound,
  ActiveRoundSerializer,
  RoundIdRandomizer,
} from "./Round";
import { PersonIdRandomizer } from "./Person";
import { PersonCard } from "./PersonCard";
import { PersonCardSet } from "./PersonCardSet";

describe("CompletedRound", () => {
  testComparable(
    () =>
      new CompletedRound(
        RoundIdRandomizer.random(),
        new PersonCardSet([
          new PersonCard(
            PersonIdRandomizer.random(),
            CardIdRandomizer.random(),
          ),
        ]),
        CardIdRandomizer.random(),
      ),
  );

  testSerializer(
    CompletedRoundSerializer,
    () =>
      new CompletedRound(
        RoundIdRandomizer.random(),
        new PersonCardSet([
          new PersonCard(
            PersonIdRandomizer.random(),
            CardIdRandomizer.random(),
          ),
        ]),
        CardIdRandomizer.random(),
      ),
  );
});

describe("ActiveRound", () => {
  testComparable(
    () =>
      new ActiveRound(
        RoundIdRandomizer.random(),
        new PersonCardSet([
          new PersonCard(
            PersonIdRandomizer.random(),
            CardIdRandomizer.random(),
          ),
        ]),
      ),
  );

  testSerializer(
    ActiveRoundSerializer,
    () =>
      new ActiveRound(
        RoundIdRandomizer.random(),
        new PersonCardSet([
          new PersonCard(
            PersonIdRandomizer.random(),
            CardIdRandomizer.random(),
          ),
        ]),
      ),
  );
});
