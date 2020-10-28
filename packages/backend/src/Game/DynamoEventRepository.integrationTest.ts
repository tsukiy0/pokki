import {
  AddPlayerEvent,
  Card,
  CardIdRandomizer,
  CardSet,
  EndRoundEvent,
  GameIdRandomizer,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
  RoundIdRandomizer,
  UserIdRandomizer,
  Event,
} from "@pokki/core";
import { GuidRandomizer, isArrayEqual, SystemConfig } from "@tsukiy0/tscore";
import { CredentialProviderChain, Credentials, DynamoDB } from "aws-sdk";
import {
  DynamoEventRepository,
  EventVersionConflictError,
} from "./DynamoEventRepository";

describe("DynamoEventRepository", () => {
  const withRepository = async (
    callback: (repository: DynamoEventRepository) => Promise<void>,
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
    const repository = new DynamoEventRepository(client, tableName);

    try {
      await client
        .createTable({
          TableName: tableName,
          KeySchema: [
            {
              AttributeName: "id",
              KeyType: "HASH",
            },
            {
              AttributeName: "version",
              KeyType: "SORT",
            },
          ],
          AttributeDefinitions: [
            {
              AttributeName: "id",
              AttributeType: "S",
            },
            {
              AttributeName: "version",
              AttributeType: "N",
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

  describe("appendEvent", () => {
    [
      new NewGameEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        new CardSet([
          new Card(CardIdRandomizer.random(), "card1"),
          new Card(CardIdRandomizer.random(), "card2"),
        ]),
      ),
      new AddPlayerEvent(GameIdRandomizer.random(), UserIdRandomizer.random()),
      new NewRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        RoundIdRandomizer.random(),
        "round1",
      ),
      new PlayCardEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
      new EndRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
    ].forEach((event) => {
      it(`appends ${event.constructor.name}`, async () => {
        await withRepository(async (repository) => {
          await repository.appendEvent(event);

          const actual = await repository.listEvents(event.gameId);

          expect(actual[0].equals(event)).toBeTruthy();
        });
      });
    });

    it("throws when conflicting version", async () => {
      await withRepository(async (repository) => {
        const gameId = GameIdRandomizer.random();
        const event = new AddPlayerEvent(gameId, UserIdRandomizer.random());

        await expect(
          Promise.all([
            repository.appendEvent(event),
            repository.appendEvent(event),
          ]),
        ).rejects.toThrow(EventVersionConflictError);
      });
    });
  });

  describe("listEvents", () => {
    it("lists in order", async () => {
      await withRepository(async (repository) => {
        const gameId = GameIdRandomizer.random();
        const events = [
          new NewGameEvent(
            gameId,
            UserIdRandomizer.random(),
            new CardSet([
              new Card(CardIdRandomizer.random(), "card1"),
              new Card(CardIdRandomizer.random(), "card2"),
            ]),
          ),
          new AddPlayerEvent(gameId, UserIdRandomizer.random()),
          new NewRoundEvent(
            gameId,
            UserIdRandomizer.random(),
            RoundIdRandomizer.random(),
            "round1",
          ),
          new PlayCardEvent(
            gameId,
            UserIdRandomizer.random(),
            CardIdRandomizer.random(),
          ),
          new EndRoundEvent(
            gameId,
            UserIdRandomizer.random(),
            CardIdRandomizer.random(),
          ),
        ];

        // eslint-disable-next-line no-restricted-syntax
        for (const event of events) {
          // eslint-disable-next-line no-await-in-loop
          await repository.appendEvent(event);
        }

        const actual = await repository.listEvents(gameId);

        expect(
          isArrayEqual<Event>(actual, events, (a, b) => a.equals(b)),
        ).toBeTruthy();
      });
    });
  });
});
