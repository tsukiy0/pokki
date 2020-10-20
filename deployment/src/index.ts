import * as cdk from '@aws-cdk/core';
import { RootStack } from './RootStack';

const stackName = process.env.CFN_STACK_NAME as string;

const app = new cdk.App();

new RootStack(app, stackName, {
  env: {
    account: process.env.CDK_DEFAULT_ACCOUNT,
    region: process.env.CDK_DEFAULT_REGION,
  },
});
