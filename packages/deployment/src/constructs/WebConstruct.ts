import path from "path";
import { Aws, Construct, RemovalPolicy } from "@aws-cdk/core";
import { Bucket } from "@aws-cdk/aws-s3";
import { BucketDeployment, Source } from "@aws-cdk/aws-s3-deployment";
import { WebConfigConstruct } from "./WebConfigConstruct";
import { ApiConstruct } from "./ApiConstruct";

export class WebConstruct extends Construct {
  public readonly url: string;

  public readonly bucket: Bucket;

  constructor(
    scope: Construct,
    id: string,
    props: {
      api: ApiConstruct;
    },
  ) {
    super(scope, id);

    const bucket = new Bucket(this, "Bucket", {
      websiteIndexDocument: "index.html",
      websiteErrorDocument: "index.html",
      publicReadAccess: true,
      removalPolicy: RemovalPolicy.DESTROY,
    });

    new BucketDeployment(this, "BucketDeployment", {
      destinationBucket: bucket,
      sources: [Source.asset(path.resolve(__dirname, "../../../web/out"))],
    });

    new WebConfigConstruct(this, "Config", {
      bucket,
      config: {
        API_KEY: props.api.graphQlApi.apiKey as string,
        API_URL: props.api.graphQlApi.graphqlUrl,
        API_REGION: Aws.REGION,
      },
    });

    this.url = bucket.urlForObject("index.html");
    this.bucket = bucket;
  }
}
