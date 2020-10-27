import {
  BaseError,
  Comparable,
  ExtendedGuidRandomizer,
  Guid,
  Randomizer,
} from "@tsukiy0/tscore";

export class UserId extends Guid {
  private readonly __tag = "GameId";
}

export const UserIdRandomizer: Randomizer<UserId> = new ExtendedGuidRandomizer(
  (_) => new UserId(_),
);

export class BadUserNameError extends BaseError {
  constructor(public readonly name: string) {
    super();
  }
}

export class User implements Comparable {
  constructor(public readonly id: UserId, public readonly name: string) {
    if (name.length === 0) {
      throw new BadUserNameError(name);
    }

    if (name.length > 8) {
      throw new BadUserNameError(name);
    }
  }

  equals(input: this): boolean {
    return this.id.equals(input.id) && this.name === input.name;
  }
}
