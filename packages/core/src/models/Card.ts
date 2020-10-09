import {
  BaseError,
  Comparable,
  Guid,
  GuidRandomizer,
  Randomizer,
  Serializer,
} from "@tsukiy0/tscore";

export class CardId extends Guid {}

export type CardIdJson = {
  value: string;
};

export const CardIdSerializer: Serializer<CardId, CardIdJson> = {
  serialize: (input) => {
    return {
      value: input.toString(),
    };
  },
  deserialize: (input) => {
    return CardId.fromString(input.value);
  },
};

export const CardIdRandomizer: Randomizer<CardId> = {
  random: (): CardId => {
    return CardId.fromString(GuidRandomizer.random().toString());
  },
};

export class CardNameTooLongError extends BaseError {
  constructor(name: string) {
    super({
      name,
    });
  }
}

export class Card implements Comparable {
  constructor(public readonly id: CardId, public readonly name: string) {
    if (name.length > 8) {
      throw new CardNameTooLongError(name);
    }
  }

  public readonly equals = (input: this): boolean => {
    return this.id.equals(input.id) && this.name === input.name;
  };
}

export type CardJson = {
  id: CardIdJson;
  name: string;
};

export const CardSerializer: Serializer<Card, CardJson> = {
  serialize: (input) => {
    return {
      id: CardIdSerializer.serialize(input.id),
      name: input.name,
    };
  },
  deserialize: (input) => {
    return new Card(CardIdSerializer.deserialize(input.id), input.name);
  },
};
