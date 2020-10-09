import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import {
  Card,
  CardIdRandomizer,
  CardIdSerializer,
  CardNameTooLongError,
  CardSerializer,
} from "./Card";

describe("CardId", () => {
  testComparable(() => CardIdRandomizer.random());
  testSerializer(CardIdSerializer, () => CardIdRandomizer.random());
});

describe("Card", () => {
  testComparable(() => new Card(CardIdRandomizer.random(), "L"));
  testSerializer(
    CardSerializer,
    () => new Card(CardIdRandomizer.random(), "L"),
  );

  it("throws when name too long", () => {
    expect(() => {
      new Card(CardIdRandomizer.random(), "111111111");
    }).toThrowError(CardNameTooLongError);
  });
});
