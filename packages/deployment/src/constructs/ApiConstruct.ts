import { Construct, Duration, Expiration } from '@aws-cdk/core';
import {
  AuthorizationType, GraphqlApi, MappingTemplate, Schema,
} from '@aws-cdk/aws-appsync';
import { Code, Function, Runtime } from '@aws-cdk/aws-lambda';
import path from 'path';
import { DatabaseConstruct } from './DatabaseConstruct';

enum TypeName {
  Mutation = 'Mutation',
  Query = 'Query',
}

const addResolver = (
  api: GraphqlApi,
  fn: Function,
  typeName: TypeName,
  fieldName: string,
): void => {
  api
    .addLambdaDataSource(`${typeName}_${fieldName}`, fn)
    .createResolver({
      typeName,
      fieldName,
      requestMappingTemplate: MappingTemplate.lambdaRequest(),
      responseMappingTemplate: MappingTemplate.lambdaResult(),
    });
};

export class ApiConstruct extends Construct {
  public readonly graphQlApi: GraphqlApi;

  constructor(scope: Construct, id: string, props: {
    database: DatabaseConstruct
  }) {
    super(scope, id);

    const graphQlApi = new GraphqlApi(this, 'Api', {
      name: 'Api',
      authorizationConfig: {
        defaultAuthorization: {
          authorizationType: AuthorizationType.API_KEY,
          apiKeyConfig: {
            expires: Expiration.after(Duration.days(365)),
          },
        },
      },
      schema: Schema.fromAsset(path.resolve(
        __dirname,
        '../../../../schema.graphql',
      )),
    });

    const fn = new Function(this, 'ApiFunction', {
      code: Code.fromAsset(
        path.resolve(
          __dirname,
          '../../../../backend/Api/bin/Release/netcoreapp3.1/Api.zip',
        ),
      ),
      runtime: Runtime.DOTNET_CORE_3_1,
      handler: 'Api::Api.Function::FunctionHandler',
      memorySize: 512,
      timeout: Duration.seconds(20),
      environment: {
        GAME_TABLE_NAME: props.database.gameTable.tableName,
        USER_TABLE_NAME: props.database.userTable.tableName,
      },
    });

    props.database.gameTable.grantReadWriteData(fn);
    props.database.userTable.grantReadWriteData(fn);

    addResolver(graphQlApi, fn, TypeName.Query, 'HealthCheck');
    addResolver(graphQlApi, fn, TypeName.Mutation, 'CreateUser');
    addResolver(graphQlApi, fn, TypeName.Query, 'GetUser');

    this.graphQlApi = graphQlApi;
  }
}
