import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { Card, CardId, CardIdRandomizer } from "./Card";
import {
  DuplicateCardError,
  EmptyCardSetError,
  CardSet,
  CardSetSerializer,
  CardNotFoundError,
} from "./CardSet";

describe("CardSet", () => {
  testComparable(
    () =>
      new CardSet([
        new Card(CardIdRandomizer.random(), "S"),
        new Card(CardIdRandomizer.random(), "M"),
      ]),
  );
  testSerializer(
    CardSetSerializer,
    () =>
      new CardSet([
        new Card(CardIdRandomizer.random(), "S"),
        new Card(CardIdRandomizer.random(), "M"),
      ]),
  );

  it("throws when has duplicate", () => {
    expect(() => {
      const id = CardIdRandomizer.random();
      new CardSet([new Card(id, "S"), new Card(id, "M")]);
    }).toThrowError(DuplicateCardError);
  });

  it("throws when empty", () => {
    expect(() => {
      new CardSet([]);
    }).toThrowError(EmptyCardSetError);
  });

  describe("getCardById", () => {
    it("get existing", () => {
      const id = CardIdRandomizer.random();
      const list = new CardSet([
        new Card(id, "S"),
        new Card(CardIdRandomizer.random(), "M"),
      ]);

      const actual = list.getCardById(id);

      expect(actual.equals(list.items[0])).toBeTruthy();
    });

    it("throws when get non existant", () => {
      const id = CardIdRandomizer.random();
      const list = new CardSet([
        new Card(id, "S"),
        new Card(CardIdRandomizer.random(), "M"),
      ]);

      expect(() => {
        const list = new CardSet([
          new Card(
            CardId.fromString("3a303c56-2999-4262-8e51-a3f951ab988b"),
            "L",
          ),
        ]);

        list.getCardById(
          CardId.fromString("9821a0f9-c546-49ef-8202-61d82ede7861"),
        );
      }).toThrowError(CardNotFoundError);
      const actual = list.getCardById(id);

      expect(actual.equals(list.items[0])).toBeTruthy();
    });
  });
});
