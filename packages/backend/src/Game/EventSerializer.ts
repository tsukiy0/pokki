import {
  AddPlayerEvent,
  Card,
  CardId,
  CardSet,
  EndRoundEvent,
  GameId,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
  RoundId,
  UserId,
} from "@pokki/core";
import { Serializer } from "@tsukiy0/tscore";
import { AttributeMap } from "aws-sdk/clients/dynamodb";
import { EventVersion } from "./EventVersion";

enum EventType {
  NEW_GAME = "NEW_GAME",
  ADD_PLAYER = "ADD_PLAYER",
  NEW_ROUND = "NEW_ROUND",
  PLAY_CARD = "PLAY_CARD",
  END_ROUND = "END_ROUND",
}

export class NewGameEventSerializer
  implements Serializer<NewGameEvent, AttributeMap> {
  constructor(private readonly eventVersion: EventVersion) {}

  serialize(input: NewGameEvent): AttributeMap {
    return {
      id: { S: input.gameId.toString() },
      version: { N: this.eventVersion.toNumber().toString() },
      type: { S: EventType.NEW_GAME },
      player_id: { S: input.playerId.toString() },
      cards: {
        L: input.cards.items.map((item) => {
          return {
            M: {
              id: { S: item.id.toString() },
              name: { S: item.name },
            },
          };
        }),
      },
    };
  }

  deserialize(input: AttributeMap): NewGameEvent {
    return new NewGameEvent(
      new GameId(input.id.S as string),
      new UserId(input.player_id.S as string),
      new CardSet(
        input.cards?.L?.map((item) => {
          return new Card(
            new CardId(item?.M?.id.S as string),
            item?.M?.name.S as string,
          );
        }) as Card[],
      ),
    );
  }
}

export class AddPlayerEventSerializer
  implements Serializer<AddPlayerEvent, AttributeMap> {
  constructor(public readonly eventVersion: EventVersion) {}

  serialize(input: AddPlayerEvent): AttributeMap {
    return {
      id: { S: input.gameId.toString() },
      version: { N: this.eventVersion.toNumber().toString() },
      type: { S: EventType.NEW_GAME },
      player_id: { S: input.playerId.toString() },
    };
  }

  deserialize(input: AttributeMap): AddPlayerEvent {
    return new AddPlayerEvent(
      new GameId(input.id.S as string),
      new UserId(input.player_id.S as string),
    );
  }
}

export class NewRoundEventSerializer
  implements Serializer<NewRoundEvent, AttributeMap> {
  constructor(public readonly eventVersion: EventVersion) {}

  serialize(input: NewRoundEvent): AttributeMap {
    return {
      id: { S: input.gameId.toString() },
      version: { N: this.eventVersion.toNumber().toString() },
      type: { S: EventType.NEW_GAME },
      player_id: { S: input.playerId.toString() },
      round_id: { S: input.roundId.toString() },
      round_name: { S: input.roundName },
    };
  }

  deserialize(input: AttributeMap): NewRoundEvent {
    return new NewRoundEvent(
      new GameId(input.id.S as string),
      new UserId(input.player_id.S as string),
      new RoundId(input.round_id.S as string),
      input.round_name.S as string,
    );
  }
}

export class PlayCardEventSerializer
  implements Serializer<PlayCardEvent, AttributeMap> {
  constructor(public readonly eventVersion: EventVersion) {}

  serialize(input: PlayCardEvent): AttributeMap {
    return {
      id: { S: input.gameId.toString() },
      version: { N: this.eventVersion.toNumber().toString() },
      type: { S: EventType.NEW_GAME },
      player_id: { S: input.playerId.toString() },
      card_id: { S: input.cardId.toString() },
    };
  }

  deserialize(input: AttributeMap): PlayCardEvent {
    return new PlayCardEvent(
      new GameId(input.id.S as string),
      new UserId(input.player_id.S as string),
      new CardId(input.card_id.S as string),
    );
  }
}

export class EndRoundEventSerializer
  implements Serializer<EndRoundEvent, AttributeMap> {
  constructor(public readonly eventVersion: EventVersion) {}

  serialize(input: EndRoundEvent): AttributeMap {
    return {
      id: { S: input.gameId.toString() },
      version: { N: this.eventVersion.toNumber().toString() },
      type: { S: EventType.NEW_GAME },
      player_id: { S: input.playerId.toString() },
      result_card_id: { S: input.resultCardId.toString() },
    };
  }

  deserialize(input: AttributeMap): EndRoundEvent {
    return new EndRoundEvent(
      new GameId(input.id.S as string),
      new UserId(input.player_id.S as string),
      new CardId(input.result_card_id.S as string),
    );
  }
}
