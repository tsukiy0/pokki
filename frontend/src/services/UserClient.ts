import AWSAppSyncClient from 'aws-appsync';
import gql from 'graphql-tag';
import { CreateUserMutation, CreateUserMutationVariables, CreateUserRequest, GetUserQuery, GetUserQueryVariables, GetUserRequest } from '../graphql/API';
import { createUser } from '../graphql/mutations';
import { getUser } from '../graphql/queries';

export class UserClient {
    constructor(private readonly client: AWSAppSyncClient<any>) {}

    public async createUser(request: CreateUserRequest): Promise<void> {
        await this.client.mutate<CreateUserMutation, CreateUserMutationVariables>({
            mutation: gql(createUser),
            variables: {
                request
            }
        })
    }

    public async getUser(request: GetUserRequest): Promise<GetUserQuery["GetUser"]> {
        const result = await this.client.query<GetUserQuery, GetUserQueryVariables>({
            query: gql(getUser),
            variables: {
                request
            }
        });

        return result.data.GetUser;
    }
}