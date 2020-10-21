import { Stack, Construct, StackProps } from '@aws-cdk/core';
import { ApiConstruct } from './constructs/ApiConstruct';
import { DatabaseConstruct } from './constructs/DatabaseConstruct';

export class RootStack extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    const database = new DatabaseConstruct(this, 'Database');
    new ApiConstruct(this, 'Api', {
      database,
    });
  }
}
