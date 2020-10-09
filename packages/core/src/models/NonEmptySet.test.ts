import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { Card, CardIdRandomizer, CardSerializer } from "./Card";
import {
  DuplicateSetItemError,
  EmptySetError,
  NonEmptySet,
  NonEmptySetSerializer,
} from "./NonEmptySet";

describe("NonEmptySet", () => {
  testComparable(
    () =>
      new NonEmptySet<Card>(
        [
          new Card(CardIdRandomizer.random(), "S"),
          new Card(CardIdRandomizer.random(), "M"),
        ],
        (a, b) => a.equals(b),
      ),
  );
  testSerializer(
    new NonEmptySetSerializer(
      CardSerializer,
      (input) => new NonEmptySet(input, (a, b) => a.equals(b)),
    ),
    () =>
      new NonEmptySet<Card>(
        [
          new Card(CardIdRandomizer.random(), "S"),
          new Card(CardIdRandomizer.random(), "M"),
        ],
        (a, b) => a.equals(b),
      ),
  );

  it("throws when has duplicate", () => {
    expect(() => {
      const id = CardIdRandomizer.random();
      new NonEmptySet<Card>([new Card(id, "S"), new Card(id, "S")], (a, b) =>
        a.equals(b),
      );
    }).toThrowError(DuplicateSetItemError);
  });

  it("throws when empty", () => {
    expect(() => {
      new NonEmptySet<Card>([], (a, b) => a.equals(b));
    }).toThrowError(EmptySetError);
  });
});
