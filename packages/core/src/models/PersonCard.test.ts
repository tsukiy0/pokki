import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { CardIdRandomizer } from "./Card";
import { PersonIdRandomizer } from "./Person";
import { PersonCardSerializer, PersonCard } from "./PersonCard";

describe("PersonCard", () => {
  testComparable(
    () =>
      new PersonCard(PersonIdRandomizer.random(), CardIdRandomizer.random()),
  );

  testSerializer(
    PersonCardSerializer,
    () =>
      new PersonCard(PersonIdRandomizer.random(), CardIdRandomizer.random()),
  );
});
