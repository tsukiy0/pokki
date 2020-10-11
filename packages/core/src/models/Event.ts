import {
  Comparable,
  DateTime,
  EnumHelper,
  Guid,
  GuidRandomizer,
  Randomizer,
  Serializer,
} from "@tsukiy0/tscore";
import { CardId } from "./Card";
import { CardSet, CardSetJson, CardSetSerializer } from "./CardSet";
import { GameId } from "./Game";
import { Person, PersonJson, PersonSerializer } from "./Person";
import { PersonCard, PersonCardJson, PersonCardSerializer } from "./PersonCard";
import { RoundId } from "./Round";

export class EventId extends Guid {}

export type EventIdJson = {
  value: string;
};

export const EventIdSerializer: Serializer<EventId, EventIdJson> = {
  serialize: (input) => {
    return {
      value: input.toString(),
    };
  },
  deserialize: (input) => {
    return EventId.fromString(input.value);
  },
};

export const EventIdRandomizer: Randomizer<EventId> = {
  random: (): EventId => {
    return EventId.fromString(GuidRandomizer.random().toString());
  },
};

export enum EventType {
  NEW_GAME = "NEW_GAME",
  ADD_PERSON = "ADD_PERSON",
  ADD_CARDS = "ADD_CARDS",
  NEW_ROUND = "NEW_ROUND",
  PERSON_SELECT_CARD = "PERSON_SELECT_CARD",
  DECIDE_RESULT = "DECIDE_RESULT",
}

export const EventTypeEnumHelper = new EnumHelper(EventType);

export abstract class Event implements Comparable {
  constructor(
    public readonly id: EventId,
    public readonly type: EventType,
    public readonly created: DateTime,
    public readonly gameId: GameId,
  ) {}

  public equals(input: this): boolean {
    return (
      this.id.equals(input.id) &&
      this.type === input.type &&
      this.created.equals(input.created) &&
      this.gameId.equals(input.gameId)
    );
  }
}

type EventJson = {
  id: string;
  type: string;
  created: string;
  gameId: string;
};

export class NewGameEvent extends Event {
  constructor(
    id: EventId,
    created: DateTime,
    gameId: GameId,
    public readonly person: Person,
  ) {
    super(id, EventType.NEW_GAME, created, gameId);
  }
}

export type NewGameEventJson = EventJson & {
  person: PersonJson;
};

export const NewGameEventSerializer: Serializer<
  NewGameEvent,
  NewGameEventJson
> = {
  serialize: (input: NewGameEvent) => {
    return {
      id: input.id.toString(),
      type: input.type,
      created: input.created.toString(),
      gameId: input.gameId.toString(),
      person: PersonSerializer.serialize(input.person),
    };
  },
  deserialize: (input: NewGameEventJson) => {
    return new NewGameEvent(
      EventId.fromString(input.id),
      DateTime.fromISOString(input.created),
      GameId.fromString(input.gameId),
      PersonSerializer.deserialize(input.person),
    );
  },
};

export class AddPersonEvent extends Event {
  constructor(
    id: EventId,
    created: DateTime,
    gameId: GameId,
    public readonly person: Person,
  ) {
    super(id, EventType.NEW_GAME, created, gameId);
  }

  public readonly equals = (input: this): boolean => {
    return super.equals(input) && this.person.equals(input.person);
  };
}

export type AddPersonEventJson = EventJson & {
  person: PersonJson;
};

export const AddPersonEventSerializer: Serializer<
  AddPersonEvent,
  AddPersonEventJson
> = {
  serialize: (input: AddPersonEvent) => {
    return {
      id: input.id.toString(),
      type: input.type,
      created: input.created.toString(),
      gameId: input.gameId.toString(),
      person: PersonSerializer.serialize(input.person),
    };
  },
  deserialize: (input: AddPersonEventJson) => {
    return new AddPersonEvent(
      EventId.fromString(input.id),
      DateTime.fromISOString(input.created),
      GameId.fromString(input.gameId),
      PersonSerializer.deserialize(input.person),
    );
  },
};

export class AddCardsEvent extends Event {
  constructor(
    id: EventId,
    created: DateTime,
    gameId: GameId,
    public readonly cards: CardSet,
  ) {
    super(id, EventType.ADD_CARDS, created, gameId);
  }

  public readonly equals = (input: this): boolean => {
    return super.equals(input) && this.cards.equals(input.cards);
  };
}

export type AddCardsEventJson = EventJson & {
  cards: CardSetJson;
};

export const AddCardsEventSerializer: Serializer<
  AddCardsEvent,
  AddCardsEventJson
> = {
  serialize: (input: AddCardsEvent) => {
    return {
      id: input.id.toString(),
      type: input.type,
      created: input.created.toString(),
      gameId: input.gameId.toString(),
      cards: CardSetSerializer.serialize(input.cards),
    };
  },
  deserialize: (input: AddCardsEventJson) => {
    return new AddCardsEvent(
      EventId.fromString(input.id),
      DateTime.fromISOString(input.created),
      GameId.fromString(input.gameId),
      CardSetSerializer.deserialize(input.cards),
    );
  },
};

export class NewRoundEvent extends Event {
  constructor(
    id: EventId,
    created: DateTime,
    gameId: GameId,
    public readonly roundId: RoundId,
  ) {
    super(id, EventType.NEW_ROUND, created, gameId);
  }

  public readonly equals = (input: this): boolean => {
    return super.equals(input) && this.roundId.equals(input.roundId);
  };
}

export type NewRoundEventJson = EventJson & {
  roundId: string;
};

export const NewRoundEventSerializer: Serializer<
  NewRoundEvent,
  NewRoundEventJson
> = {
  serialize: (input: NewRoundEvent) => {
    return {
      id: input.id.toString(),
      type: input.type,
      created: input.created.toString(),
      gameId: input.gameId.toString(),
      roundId: input.roundId.toString(),
    };
  },
  deserialize: (input: NewRoundEventJson) => {
    return new NewRoundEvent(
      EventId.fromString(input.id),
      DateTime.fromISOString(input.created),
      GameId.fromString(input.gameId),
      RoundId.fromString(input.roundId),
    );
  },
};

export class PersonSelectCardEvent extends Event {
  constructor(
    id: EventId,
    created: DateTime,
    gameId: GameId,
    public readonly roundId: RoundId,
    public readonly personCard: PersonCard,
  ) {
    super(id, EventType.PERSON_SELECT_CARD, created, gameId);
  }

  public readonly equals = (input: this): boolean => {
    return (
      super.equals(input) &&
      this.roundId.equals(input.roundId) &&
      this.personCard.equals(input.personCard)
    );
  };
}

export type PersonSelectCardEventJson = EventJson & {
  roundId: string;
  personCard: PersonCardJson;
};

export const PersonSelectCardEventSerializer: Serializer<
  PersonSelectCardEvent,
  PersonSelectCardEventJson
> = {
  serialize: (input: PersonSelectCardEvent) => {
    return {
      id: input.id.toString(),
      type: input.type,
      created: input.created.toString(),
      gameId: input.gameId.toString(),
      roundId: input.roundId.toString(),
      personCard: PersonCardSerializer.serialize(input.personCard),
    };
  },
  deserialize: (input: PersonSelectCardEventJson) => {
    return new PersonSelectCardEvent(
      EventId.fromString(input.id),
      DateTime.fromISOString(input.created),
      GameId.fromString(input.gameId),
      RoundId.fromString(input.roundId),
      PersonCardSerializer.deserialize(input.personCard),
    );
  },
};

export class DecideResultEvent extends Event {
  constructor(
    id: EventId,
    created: DateTime,
    gameId: GameId,
    public readonly roundId: RoundId,
    public readonly cardId: CardId,
  ) {
    super(id, EventType.DECIDE_RESULT, created, gameId);
  }

  public readonly equals = (input: this): boolean => {
    return (
      super.equals(input) &&
      this.roundId.equals(input.roundId) &&
      this.cardId.equals(input.cardId)
    );
  };
}

export type DecideResultEventJson = EventJson & {
  roundId: string;
  cardId: string;
};

export const DecideResultEventSerializer: Serializer<
  DecideResultEvent,
  DecideResultEventJson
> = {
  serialize: (input: DecideResultEvent) => {
    return {
      id: input.id.toString(),
      type: input.type,
      created: input.created.toString(),
      gameId: input.gameId.toString(),
      roundId: input.roundId.toString(),
      cardId: input.cardId.toString(),
    };
  },
  deserialize: (input: DecideResultEventJson) => {
    return new DecideResultEvent(
      EventId.fromString(input.id),
      DateTime.fromISOString(input.created),
      GameId.fromString(input.gameId),
      RoundId.fromString(input.roundId),
      CardId.fromString(input.cardId),
    );
  },
};
