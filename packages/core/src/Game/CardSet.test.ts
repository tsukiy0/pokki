import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { Card, CardIdRandomizer } from "./Card";
import {
  CardSet,
  DuplicateCardIdException,
  DuplicateCardNameException,
} from "./CardSet";

describe("CardSet", () => {
  testComparable(
    () =>
      new CardSet([
        new Card(CardIdRandomizer.random(), "1234"),
        new Card(CardIdRandomizer.random(), "5678"),
      ]),
  );

  it("throws when duplicate id", () => {
    expect(() => {
      const id = CardIdRandomizer.random();
      new CardSet([new Card(id, "1234"), new Card(id, "5678")]);
    }).toThrow(DuplicateCardIdException);
  });

  it("throws when duplicate name", () => {
    expect(
      () =>
        new CardSet([
          new Card(CardIdRandomizer.random(), "1234"),
          new Card(CardIdRandomizer.random(), "1234"),
        ]),
    ).toThrow(DuplicateCardNameException);
  });

  describe("hasCard", () => {
    it("true when has card", () => {
      const id = CardIdRandomizer.random();
      const cards = new CardSet([
        new Card(CardIdRandomizer.random(), "1234"),
        new Card(id, "5678"),
      ]);

      const actual = cards.hasCard(id);

      expect(actual).toBeTruthy();
    });

    it("false when not has card", () => {
      const cards = new CardSet([
        new Card(CardIdRandomizer.random(), "1234"),
        new Card(CardIdRandomizer.random(), "5678"),
      ]);

      const actual = cards.hasCard(CardIdRandomizer.random());

      expect(actual).toBeFalsy();
    });
  });

  describe("addCard", () => {
    it("adds card", () => {
      const card = new Card(CardIdRandomizer.random(), "card1");

      const actual = new CardSet([]).addCard(card);

      expect(actual.equals(new CardSet([card]))).toBeTruthy();
    });
  });

  describe("removeCard", () => {
    it("adds card", () => {
      const card = new Card(CardIdRandomizer.random(), "card1");

      const actual = new CardSet([card]).removeCard(card.id);

      expect(actual.equals(new CardSet([]))).toBeTruthy();
    });
  });
});
