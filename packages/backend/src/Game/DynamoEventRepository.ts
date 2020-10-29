import {
  AddPlayerEvent,
  EndRoundEvent,
  Event,
  EventRepository,
  GameId,
  matchEvent,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
} from "@pokki/core";
import { BaseError } from "@tsukiy0/tscore";
import { DynamoDB } from "aws-sdk";
import { AttributeMap } from "aws-sdk/clients/dynamodb";
import {
  AddPlayerEventSerializer,
  EndRoundEventSerializer,
  EventType,
  NewGameEventSerializer,
  NewRoundEventSerializer,
  PlayCardEventSerializer,
} from "./EventSerializer";
import { EventVersion } from "./EventVersion";

export class NoEventTypeMatchedError extends BaseError {}
export class EventVersionConflictError extends BaseError {}

export class DynamoEventRepository implements EventRepository {
  constructor(
    private readonly dynamoClient: DynamoDB,
    private readonly tableName: string,
  ) {}

  static default = (tableName: string): EventRepository => {
    return new DynamoEventRepository(new DynamoDB(), tableName);
  };

  async appendEvent(event: Event): Promise<void> {
    const nextVersion = await this.getNextVersion(event.gameId);

    try {
      await this.dynamoClient
        .putItem({
          TableName: this.tableName,
          ConditionExpression: "attribute_not_exists(version)",
          Item: this.getItem(event, nextVersion),
        })
        .promise();
    } catch (err) {
      if (err.name === "ConditionalCheckFailedException") {
        throw new EventVersionConflictError();
      }

      throw err;
    }
  }

  async listEvents(gameId: GameId): Promise<readonly Event[]> {
    const res = await this.dynamoClient
      .query({
        TableName: this.tableName,
        KeyConditionExpression: "#hn = :hv",
        ExpressionAttributeNames: {
          "#hn": "id",
        },
        ExpressionAttributeValues: {
          ":hv": {
            S: gameId.toString(),
          },
        },
        ScanIndexForward: true,
      })
      .promise();

    return (res.Items as AttributeMap[]).map((item) => {
      const type = item.type.S as string;

      switch (type) {
        case EventType.NEW_GAME:
          return new NewGameEventSerializer(new EventVersion(1)).deserialize(
            item,
          );
        case EventType.ADD_PLAYER:
          return new AddPlayerEventSerializer(new EventVersion(1)).deserialize(
            item,
          );
        case EventType.NEW_ROUND:
          return new NewRoundEventSerializer(new EventVersion(1)).deserialize(
            item,
          );
        case EventType.PLAY_CARD:
          return new PlayCardEventSerializer(new EventVersion(1)).deserialize(
            item,
          );
        case EventType.END_ROUND:
          return new EndRoundEventSerializer(new EventVersion(1)).deserialize(
            item,
          );
        default:
          throw new NoEventTypeMatchedError();
      }
    });
  }

  private getItem(input: Event, version: EventVersion): AttributeMap {
    return matchEvent(input, {
      newGame: (event: NewGameEvent) =>
        new NewGameEventSerializer(version).serialize(event),
      addPlayer: (event: AddPlayerEvent) =>
        new AddPlayerEventSerializer(version).serialize(event),
      newRound: (event: NewRoundEvent) =>
        new NewRoundEventSerializer(version).serialize(event),
      playCard: (event: PlayCardEvent) =>
        new PlayCardEventSerializer(version).serialize(event),
      endRound: (event: EndRoundEvent) =>
        new EndRoundEventSerializer(version).serialize(event),
    });
  }

  private async getNextVersion(gameId: GameId): Promise<EventVersion> {
    const res = await this.dynamoClient
      .query({
        TableName: this.tableName,
        KeyConditionExpression: "#hn = :hv",
        ExpressionAttributeNames: {
          "#hn": "id",
        },
        ExpressionAttributeValues: {
          ":hv": {
            S: gameId.toString(),
          },
        },
        ScanIndexForward: false,
        Limit: 1,
      })
      .promise();

    const item = (res.Items || [])[0]?.version.N;

    if (!item) {
      return new EventVersion(1);
    }

    return new EventVersion(Number.parseInt(item, 10) + 1);
  }
}
