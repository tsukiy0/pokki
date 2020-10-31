import {
  AddPlayerEvent,
  Card,
  CardIdRandomizer,
  CardSet,
  EndRoundEvent,
  Game,
  GameIdRandomizer,
  GameStatus,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
  PlayerRole,
  PlayerRoleSet,
  Role,
  RoundIdRandomizer,
  UserIdRandomizer,
} from "@pokki/core";
import { SystemConfig } from "@tsukiy0/tscore";
import AWSAppSyncClient, { AUTH_TYPE } from "aws-appsync";
import { GraphQlGameService } from "./GraphQlGameService";

describe("GraphQlUserService", () => {
  it("plays through a game", async () => {
    const config = new SystemConfig();

    const service = new GraphQlGameService(
      new AWSAppSyncClient({
        url: config.get("API_URL"),
        region: config.get("API_REGION"),
        auth: {
          type: AUTH_TYPE.API_KEY,
          apiKey: config.get("API_KEY"),
        },
        disableOffline: true,
      }),
    );

    const gameId = GameIdRandomizer.random();
    const adminId = UserIdRandomizer.random();
    const playerId = UserIdRandomizer.random();
    const cards = new CardSet([
      new Card(CardIdRandomizer.random(), "card1"),
      new Card(CardIdRandomizer.random(), "card2"),
    ]);

    await service.newGame(new NewGameEvent(gameId, adminId, cards));
    await service.addPlayer(new AddPlayerEvent(gameId, playerId));
    await service.newRound(
      new NewRoundEvent(gameId, adminId, RoundIdRandomizer.random(), "round1"),
    );
    await service.playCard(
      new PlayCardEvent(gameId, adminId, cards.items[0].id),
    );
    await service.playCard(
      new PlayCardEvent(gameId, playerId, cards.items[1].id),
    );
    const actual = await service.endRound(
      new EndRoundEvent(gameId, playerId, cards.items[1].id),
    );

    expect(
      actual.equals(
        new Game(
          gameId,
          GameStatus.INACTIVE,
          cards,
          new PlayerRoleSet([
            new PlayerRole(adminId, Role.ADMIN),
            new PlayerRole(playerId, Role.PLAYER),
          ]),
        ),
      ),
    ).toBeTruthy();
  });
});
