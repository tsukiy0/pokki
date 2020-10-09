import { Person, PersonId, PersonIdRandomizer } from "./Person";
import { PersonSet, PersonNotFoundError } from "./PersonSet";

describe("PersonSet", () => {
  describe("getPersonById", () => {
    it("get existing", () => {
      const id = PersonIdRandomizer.random();
      const list = new PersonSet([
        new Person(id, "bob"),
        new Person(PersonIdRandomizer.random(), "jim"),
      ]);

      const actual = list.getPersonById(id);

      expect(actual.equals(list.items[0])).toBeTruthy();
    });

    it("throws when get non existant", () => {
      const id = PersonIdRandomizer.random();
      const list = new PersonSet([
        new Person(id, "bob"),
        new Person(PersonIdRandomizer.random(), "jim"),
      ]);

      expect(() => {
        const list = new PersonSet([
          new Person(
            PersonId.fromString("3a303c56-2999-4262-8e51-a3f951ab988b"),
            "bob",
          ),
        ]);

        list.getPersonById(
          PersonId.fromString("9821a0f9-c546-49ef-8202-61d82ede7861"),
        );
      }).toThrowError(PersonNotFoundError);
      const actual = list.getPersonById(id);

      expect(actual.equals(list.items[0])).toBeTruthy();
    });
  });
});
