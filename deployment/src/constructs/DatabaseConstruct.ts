import { Construct } from '@aws-cdk/core';
import {
  Table,
  BillingMode,
  Attribute,
  AttributeType,
} from '@aws-cdk/aws-dynamodb';

export class DatabaseConstruct extends Construct {
  public readonly gameTable: Table;

  public readonly userTable: Table;

  constructor(scope: Construct, id: string) {
    super(scope, id);

    this.gameTable = this.getGameTable();
    this.userTable = this.getUserTable();
  }

  private getGameTable() {
    const idAttr: Attribute = {
      name: 'id',
      type: AttributeType.STRING,
    };

    const versionAttr: Attribute = {
      name: 'version',
      type: AttributeType.NUMBER,
    };

    return new Table(this, 'GameTable', {
      partitionKey: idAttr,
      sortKey: versionAttr,
      billingMode: BillingMode.PAY_PER_REQUEST,
    });
  }

  private getUserTable() {
    const idAttr: Attribute = {
      name: 'id',
      type: AttributeType.STRING,
    };

    return new Table(this, 'UserTable', {
      partitionKey: idAttr,
      billingMode: BillingMode.PAY_PER_REQUEST,
    });
  }
}
