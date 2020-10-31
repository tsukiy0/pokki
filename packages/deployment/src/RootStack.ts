import { Stack, Construct, StackProps, CfnOutput, Aws } from "@aws-cdk/core";
import { ApiConstruct } from "./constructs/ApiConstruct";
import { DatabaseConstruct } from "./constructs/DatabaseConstruct";
import { WebConstruct } from "./constructs/WebConstruct";

export class RootStack extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    const database = new DatabaseConstruct(this, "Database");
    const api = new ApiConstruct(this, "Api", {
      database,
    });
    const web = new WebConstruct(this, "Web", {
      api,
    });

    new CfnOutput(this, "ApiKey", {
      value: api.graphQlApi.apiKey as string,
    });
    new CfnOutput(this, "ApiUrl", {
      value: api.graphQlApi.graphqlUrl,
    });
    new CfnOutput(this, "ApiRegion", {
      value: Aws.REGION,
    });
    new CfnOutput(this, "WebUrl", {
      value: web.url,
    });
  }
}
