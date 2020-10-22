/* tslint:disable */
/* eslint-disable */
// this is an auto generated file. This will be overwritten

export const healthCheck = /* GraphQL */ `
  query HealthCheck($request: VoidRequest!) {
    HealthCheck(request: $request) {
      IsHealthy
    }
  }
`;
export const getUser = /* GraphQL */ `
  query GetUser($request: GetUserRequest!) {
    GetUser(request: $request) {
      Id
      Name
    }
  }
`;
