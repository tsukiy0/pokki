import {
  GetUserRequest,
  GetUserRequestSerializer,
  User,
  UserSerializer,
} from "@pokki/core";
import AWSAppSyncClient from "aws-appsync";
import {
  CreateUser,
  CreateUserMutation,
  CreateUserMutationVariables,
  GetUser,
  GetUserQuery,
  GetUserQueryVariables,
} from "../generated/graphql";

export class GraphQlUserService {
  constructor(private readonly client: AWSAppSyncClient<any>) {}

  async createUser(request: User): Promise<void> {
    await this.client.mutate<CreateUserMutation, CreateUserMutationVariables>({
      mutation: CreateUser,
      variables: {
        request: UserSerializer.serialize(request),
      },
    });
  }

  async getUser(request: GetUserRequest): Promise<User> {
    const result = await this.client.query<GetUserQuery, GetUserQueryVariables>(
      {
        query: GetUser,
        variables: {
          request: GetUserRequestSerializer.serialize(request),
        },
      },
    );

    return UserSerializer.deserialize(result.data.GetUser);
  }
}
