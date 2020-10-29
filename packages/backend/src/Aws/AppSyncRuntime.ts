import { BaseError, Serializer } from "@tsukiy0/tscore";

export enum GraphQlType {
  MUTATION = "Mutation",
  QUERY = "Query",
}

type RequestMap<TRequest, TResponse> = {
  type: GraphQlType;
  field: string;
  requestSerializer: Serializer<TRequest, unknown>;
  responseSerializer: Serializer<TResponse, unknown>;
  handler: (request: TRequest) => Promise<TResponse>;
};

class HandlerNotFoundError extends BaseError {}

export class AppSyncRuntime {
  constructor(private readonly map: RequestMap<unknown, unknown>[]) {}

  async run(
    event: AWSLambda.AppSyncResolverEvent<{
      request: unknown;
    }>,
  ): Promise<unknown> {
    const item = this.map.find(
      (_) =>
        _.type === event.info.parentTypeName &&
        _.field === event.info.fieldName,
    );

    if (!item) {
      throw new HandlerNotFoundError();
    }

    const request = item.requestSerializer.deserialize(event.arguments.request);
    const response = await item.handler(request);
    return item.responseSerializer.serialize(response);
  }
}
