import {
  Comparable,
  Guid,
  GuidRandomizer,
  Randomizer,
  Serializer,
} from "@tsukiy0/tscore";
import { CardId } from "./Card";
import {
  PersonCardSet,
  PersonCardSetJson,
  PersonCardSetSerializer,
} from "./PersonCardSet";

export class RoundId extends Guid {}

export type RoundIdJson = {
  value: string;
};

export const RoundIdSerializer: Serializer<RoundId, RoundIdJson> = {
  serialize: (input) => {
    return {
      value: input.toString(),
    };
  },
  deserialize: (input) => {
    return RoundId.fromString(input.value);
  },
};

export const RoundIdRandomizer: Randomizer<RoundId> = {
  random: (): RoundId => {
    return RoundId.fromString(GuidRandomizer.random().toString());
  },
};

export class ActiveRound implements Comparable {
  constructor(
    public readonly id: RoundId,
    public readonly personCards: PersonCardSet,
  ) {}

  public readonly equals = (input: this): boolean => {
    return (
      this.id.equals(input.id) && this.personCards.equals(input.personCards)
    );
  };
}

export type ActiveRoundJson = {
  id: string;
  personCards: PersonCardSetJson;
};

export const ActiveRoundSerializer: Serializer<ActiveRound, ActiveRoundJson> = {
  serialize: (input: ActiveRound) => {
    return {
      id: input.id.toString(),
      personCards: PersonCardSetSerializer.serialize(input.personCards),
    };
  },
  deserialize: (input: ActiveRoundJson) => {
    return new ActiveRound(
      RoundId.fromString(input.id),
      PersonCardSetSerializer.deserialize(input.personCards),
    );
  },
};

export class CompletedRound implements Comparable {
  constructor(
    public readonly id: RoundId,
    public readonly personCards: PersonCardSet,
    public readonly resultCardId: CardId,
  ) {}

  public readonly equals = (input: this): boolean => {
    return (
      this.id.equals(input.id) &&
      this.personCards.equals(input.personCards) &&
      this.resultCardId.equals(input.resultCardId)
    );
  };
}

export type CompletedRoundJson = {
  id: string;
  personCards: PersonCardSetJson;
  resultCardId: string;
};

export const CompletedRoundSerializer: Serializer<
  CompletedRound,
  CompletedRoundJson
> = {
  serialize: (input: CompletedRound) => {
    return {
      id: input.id.toString(),
      personCards: PersonCardSetSerializer.serialize(input.personCards),
      resultCardId: input.resultCardId.toString(),
    };
  },
  deserialize: (input: CompletedRoundJson) => {
    return new CompletedRound(
      RoundId.fromString(input.id),
      PersonCardSetSerializer.deserialize(input.personCards),
      CardId.fromString(input.resultCardId),
    );
  },
};
