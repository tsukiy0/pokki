import { Construct, Duration, Expiration } from '@aws-cdk/core';
import {
  AuthorizationType, GraphqlApi, MappingTemplate, Schema,
} from '@aws-cdk/aws-appsync';
import { Code, Function, Runtime } from '@aws-cdk/aws-lambda';
import path from 'path';
import { DatabaseConstruct } from './DatabaseConstruct';

export class ApiConstruct extends Construct {
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
        '../../../schema.graphql',
      )),
    });

    const fn = new Function(this, 'ApiFunction', {
      code: Code.fromAsset(
        path.resolve(
          __dirname,
          '../../../backend/Api/bin/Release/netcoreapp3.1/Api.zip',
        ),
      ),
      runtime: Runtime.DOTNET_CORE_3_1,
      handler: 'Api::Api.Function::FunctionHandler',
      memorySize: 512,
      timeout: Duration.seconds(20),
      environment: {
        TABLE_NAME: props.database.table.tableName,
      },
    });

    props.database.table.grantReadWriteData(fn);

    graphQlApi
      .addLambdaDataSource('HelloWorld', fn)
      .createResolver({
        typeName: 'Query',
        fieldName: 'getUser',
        requestMappingTemplate: MappingTemplate.lambdaRequest(),
        responseMappingTemplate: MappingTemplate.lambdaResult(),
      });
  }
}
