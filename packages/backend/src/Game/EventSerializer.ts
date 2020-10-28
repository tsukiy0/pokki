import {
  AddPlayerEvent,
  Card,
  CardId,
  CardSet,
  GameId,
  NewGameEvent,
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
      new GameId(input.id!.S),
      new UserId(input.player_id!.S),
      new CardSet(
        input.cards!.L.map((item) => {
          return new Card(new CardId(item!.M.id!.S), item!.M.name!.S);
        }),
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
      new GameId(input.id!.S),
      new UserId(input.player_id!.S),
    );
  }
}
