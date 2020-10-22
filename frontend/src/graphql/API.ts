/* tslint:disable */
/* eslint-disable */
//  This file was automatically generated and should not be edited.

export type CreateUserRequest = {
  Id: string,
  Name: string,
};

export type VoidRequest = {
  void?: boolean | null,
};

export type GetUserRequest = {
  Id: string,
};

export type CreateUserMutationVariables = {
  request: CreateUserRequest,
};

export type CreateUserMutation = {
  CreateUser:  {
    __typename: "VoidResponse",
    void: boolean | null,
  },
};

export type HealthCheckQueryVariables = {
  request: VoidRequest,
};

export type HealthCheckQuery = {
  HealthCheck:  {
    __typename: "HealthCheckResponse",
    IsHealthy: boolean,
  },
};

export type GetUserQueryVariables = {
  request: GetUserRequest,
};

export type GetUserQuery = {
  GetUser:  {
    __typename: "GetUserResponse",
    Id: string,
    Name: string,
  },
};
