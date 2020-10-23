import AWSAppSyncClient, { AUTH_TYPE } from 'aws-appsync';
import * as uuid from 'uuid';
import { UserClient } from './UserClient';

describe('UserClient', () => {
  it('creates user', async () => {
    const client = new UserClient(new AWSAppSyncClient({
      url: process.env.API_URL as string,
      region: process.env.API_REGION as string,
      auth: {
        type: AUTH_TYPE.API_KEY,
        apiKey: process.env.API_KEY as string,
      },
      disableOffline: true,
    }));

    const id = uuid.v4();

    await client.createUser({
      Id: id,
      Name: 'bob',
    });

    const actual = await client.getUser({
      Id: id,
    });

    expect(actual.Id).toEqual(id);
    expect(actual.Name).toEqual('bob');
  });
});
