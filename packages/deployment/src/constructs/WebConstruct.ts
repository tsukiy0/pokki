import path from "path";
import { Construct, RemovalPolicy } from "@aws-cdk/core";
import { Bucket } from "@aws-cdk/aws-s3";
import { BucketDeployment, Source } from "@aws-cdk/aws-s3-deployment";

export class WebConstruct extends Construct {
  public readonly url: string;

  public readonly bucket: Bucket;

  constructor(scope: Construct, id: string) {
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

    this.url = bucket.urlForObject("index.html");
    this.bucket = bucket;
  }
}
