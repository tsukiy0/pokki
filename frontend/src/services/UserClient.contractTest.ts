import AWSAppSyncClient, { AUTH_TYPE } from 'aws-appsync';
import uuid from 'uuid';
import { UserClient } from './UserClient';

describe('UserClient', () => {
  it('creates user', async () => {
    const client = new UserClient(new AWSAppSyncClient({
      url: '',
      region: '',
      auth: {
        type: AUTH_TYPE.API_KEY,
        apiKey: '',
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
