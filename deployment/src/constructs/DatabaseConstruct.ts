import { Construct } from '@aws-cdk/core';
import {
  Table,
  BillingMode,
  Attribute,
  AttributeType,
} from '@aws-cdk/aws-dynamodb';

export class DatabaseConstruct extends Construct {
  public readonly table: Table;

  constructor(scope: Construct, id: string) {
    super(scope, id);

    const idAttr: Attribute = {
      name: 'id',
      type: AttributeType.STRING,
    };

    const versionAttr: Attribute = {
      name: 'version',
      type: AttributeType.NUMBER,
    };

    const table = new Table(this, 'Table', {
      partitionKey: idAttr,
      sortKey: versionAttr,
      billingMode: BillingMode.PAY_PER_REQUEST,
    });

    this.table = table;
  }
}
