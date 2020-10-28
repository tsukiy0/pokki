import { User, UserId, UserRepository } from "@pokki/core";
import { DynamoDB } from "aws-sdk";
import { AttributeMap } from "aws-sdk/clients/dynamodb";
import { UserSerializer } from "./UserSerializer";

export class DynamoUserRepository implements UserRepository {
  constructor(
    private readonly dynamoClient: DynamoDB,
    private readonly tableName: string,
  ) {}

  async createUser(user: User): Promise<void> {
    await this.dynamoClient
      .putItem({
        TableName: this.tableName,
        Item: UserSerializer.serialize(user),
      })
      .promise();
  }

  async getUser(id: UserId): Promise<User> {
    const res = await this.dynamoClient
      .getItem({
        TableName: this.tableName,
        Key: {
          id: { S: id.toString() },
        },
      })
      .promise();

    return UserSerializer.deserialize(res.Item as AttributeMap);
  }
}
