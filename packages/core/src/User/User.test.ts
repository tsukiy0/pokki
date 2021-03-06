import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import {
  BadUserNameError,
  User,
  UserIdRandomizer,
  UserSerializer,
} from "./User";

describe("User", () => {
  testComparable(() => new User(UserIdRandomizer.random(), "test"));
  testSerializer(
    UserSerializer,
    () => new User(UserIdRandomizer.random(), "test"),
  );

  it("throws when name is empty", () => {
    expect(() => new User(UserIdRandomizer.random(), "")).toThrow(
      BadUserNameError,
    );
  });

  it("throws when name is too long", () => {
    expect(() => new User(UserIdRandomizer.random(), "123456789")).toThrow(
      BadUserNameError,
    );
  });
});
