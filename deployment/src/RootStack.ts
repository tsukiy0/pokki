import { Stack, Construct, StackProps } from '@aws-cdk/core';
import { GameDatabaseConstruct } from './constructs/GameDatabaseConstruct';

export class RootStack extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    new GameDatabaseConstruct(this, 'GameDatabase');
  }
}
