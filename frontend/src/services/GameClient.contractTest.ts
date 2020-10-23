import AWSAppSyncClient, { AUTH_TYPE } from 'aws-appsync';
import * as uuid from 'uuid';
import { GameClient } from './GameClient';

describe('GameClient', () => {
  it('creates user', async () => {
    const client = new GameClient(new AWSAppSyncClient({
      url: process.env.API_URL as string,
      region: process.env.API_REGION as string,
      auth: {
        type: AUTH_TYPE.API_KEY,
        apiKey: process.env.API_KEY as string,
      },
      disableOffline: true,
    }));

    const gameId = uuid.v4();
    const adminId = uuid.v4();
    const cardId = uuid.v4();
    const playerId = uuid.v4();

    await client.newGame({
      GameId: gameId,
      AdminId: adminId,
      Cards: [
        {
          Id: cardId,
          Name: 'M',
        },
      ],
    });

    await client.addPlayer({
      GameId: gameId,
      PlayerId: playerId,
    });

    await client.newRound({
      GameId: gameId,
      RoundId: uuid.v4(),
      RoundName: 'SM123',
    });

    await client.selectCard({
      GameId: gameId,
      PlayerCard: {
        PlayerId: adminId,
        CardId: cardId,
      },
    });

    await client.selectCard({
      GameId: gameId,
      PlayerCard: {
        PlayerId: playerId,
        CardId: cardId,
      },
    });

    const game = await client.endRound({
      GameId: gameId,
      ResultCardId: cardId,
    });

    console.log(game);

    expect(game).toBeDefined();
  });
});
