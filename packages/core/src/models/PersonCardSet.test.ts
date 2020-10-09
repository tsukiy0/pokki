import { testSerializer } from "@tsukiy0/tscore/dist/index.testTemplate";
import { CardIdRandomizer } from "./Card";
import { PersonIdRandomizer } from "./Person";
import { PersonCard } from "./PersonCard";
import { PersonCardSet, PersonCardSetSerializer } from "./PersonCardSet";

describe("PersonCardSet", () => {
  testSerializer(
    PersonCardSetSerializer,
    () =>
      new PersonCardSet([
        new PersonCard(PersonIdRandomizer.random(), CardIdRandomizer.random()),
      ]),
  );
});
