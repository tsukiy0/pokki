import { User, UserIdRandomizer } from "@pokki/core";
import { SystemConfig, GuidRandomizer } from "@tsukiy0/tscore";
import { DynamoDB, CredentialProviderChain, Credentials } from "aws-sdk";
import { DynamoUserRepository } from "./DynamoUserRepository";

describe("DynamoUserRepository", () => {
  const withRepository = async (
    callback: (repository: DynamoUserRepository) => Promise<void>,
  ): Promise<void> => {
    const config = new SystemConfig();
    const tableName = GuidRandomizer.random().toString();
    const client = new DynamoDB({
      endpoint: config.get("DYNAMO_URL"),
      region: "us-east-1",
      credentialProvider: new CredentialProviderChain([
        () => new Credentials("", ""),
      ]),
    });
    const repository = new DynamoUserRepository(client, tableName);

    try {
      await client
        .createTable({
          TableName: tableName,
          KeySchema: [
            {
              AttributeName: "id",
              KeyType: "HASH",
            },
          ],
          AttributeDefinitions: [
            {
              AttributeName: "id",
              AttributeType: "S",
            },
          ],
          BillingMode: "PAY_PER_REQUEST",
        })
        .promise();

      await callback(repository);
    } finally {
      await client
        .deleteTable({
          TableName: tableName,
        })
        .promise();
    }
  };

  it("creates and gets", async () => {
    await withRepository(async (repository) => {
      const user = new User(UserIdRandomizer.random(), "user1");
      await repository.createUser(user);

      const actual = await repository.getUser(user.id);

      expect(actual.equals(user)).toBeTruthy();
    });
  });
});
