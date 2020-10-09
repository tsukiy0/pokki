import { Card } from "./Card";
import { EmptySetError, NonEmptySet } from "./NonEmptySet";

describe("NonEmptySet", () => {
  it("throws when empty", () => {
    expect(() => {
      new NonEmptySet<Card>([], (a, b) => a.equals(b));
    }).toThrowError(EmptySetError);
  });
});
