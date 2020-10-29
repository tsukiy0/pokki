import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import {
  BadCardNameError,
  Card,
  CardIdRandomizer,
  CardSerializer,
} from "./Card";

describe("Card", () => {
  testComparable(() => new Card(CardIdRandomizer.random(), "test"));
  testSerializer(
    CardSerializer,
    () => new Card(CardIdRandomizer.random(), "test"),
  );

  it("throws when name is empty", () => {
    expect(() => new Card(CardIdRandomizer.random(), "")).toThrow(
      BadCardNameError,
    );
  });

  it("throws when name is too long", () => {
    expect(() => new Card(CardIdRandomizer.random(), "123456789")).toThrow(
      BadCardNameError,
    );
  });
});
