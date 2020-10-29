import {
  BaseError,
  Comparable,
  ExtendedGuidRandomizer,
  Guid,
  Randomizer,
  Serializer,
} from "@tsukiy0/tscore";
import {
  PlayerCardSet,
  PlayerCardSetJson,
  PlayerCardSetSerializer,
} from "./PlayerCardSet";

export class RoundId extends Guid {
  private readonly __tag = "RoundId";
}

export const RoundIdRandomizer: Randomizer<RoundId> = new ExtendedGuidRandomizer(
  (_) => new RoundId(_),
);

export class BadRoundNameError extends BaseError {
  constructor(public readonly name: string) {
    super();
  }
}

export class Round implements Comparable {
  constructor(
    public readonly id: RoundId,
    public readonly name: string,
    public readonly playerCards: PlayerCardSet,
  ) {
    if (name.length === 0) {
      throw new BadRoundNameError(name);
    }

    if (name.length > 8) {
      throw new BadRoundNameError(name);
    }
  }

  equals(input: this): boolean {
    return this.id.equals(input.id) && this.name === input.name;
  }
}

export type RoundJson = {
  id: string;
  name: string;
  playerCards: PlayerCardSetJson;
};

export const RoundSerializer: Serializer<Round, RoundJson> = {
  serialize: (input: Round): RoundJson => {
    return {
      id: input.id.toString(),
      name: input.name,
      playerCards: PlayerCardSetSerializer.serialize(input.playerCards),
    };
  },
  deserialize: (input: RoundJson): Round => {
    return new Round(
      new RoundId(input.id),
      input.name,
      PlayerCardSetSerializer.deserialize(input.playerCards),
    );
  },
};
