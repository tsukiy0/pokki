import { Event, EventRepository, GameId } from "@pokki/core";
import { DynamoDB } from "aws-sdk";
import { EventVersion } from "./EventVersion";

export class DynamoEventRepository implements EventRepository {
  constructor(
    private readonly dynamoClient: DynamoDB,
    private readonly tableName: string,
  ) {}

  async appendEvent(event: Event): Promise<void> {
    throw new Error("Method not implemented.");
  }

  listEvents(gameId: GameId): Promise<readonly Event[]> {
    throw new Error("Method not implemented.");
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
