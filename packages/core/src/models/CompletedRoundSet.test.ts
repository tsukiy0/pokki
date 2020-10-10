import { testSerializer } from "@tsukiy0/tscore/dist/index.testTemplate";
import { CompletedRound, RoundIdRandomizer } from "./Round";
import {
  CompletedRoundSet,
  CompletedRoundSetSerializer,
} from "./CompletedRoundSet";
import { PersonCardSet } from "./PersonCardSet";
import { CardIdRandomizer } from "./Card";

describe("CompletedRoundSet", () => {
  testSerializer(
    CompletedRoundSetSerializer,
    () =>
      new CompletedRoundSet([
        new CompletedRound(
          RoundIdRandomizer.random(),
          new PersonCardSet([]),
          CardIdRandomizer.random(),
        ),
        new CompletedRound(
          RoundIdRandomizer.random(),
          new PersonCardSet([]),
          CardIdRandomizer.random(),
        ),
      ]),
  );
});
