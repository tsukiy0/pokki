import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { Card, CardIdRandomizer } from "./Card";
import { Set, DuplicateSetItemError } from "./Set";

describe("Set", () => {
  testComparable(
    () =>
      new Set<Card>(
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
      new Set<Card>([new Card(id, "S"), new Card(id, "S")], (a, b) =>
        a.equals(b),
      );
    }).toThrowError(DuplicateSetItemError);
  });
});
