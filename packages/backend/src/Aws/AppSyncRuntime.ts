import { BaseError } from "@tsukiy0/tscore";
import { Handler } from "./Handler";

export enum GraphQlType {
  MUTATION = "Mutation",
  QUERY = "Query",
}

export type HandlerMapItem<TRequest, TResponse> = {
  type: GraphQlType;
  field: string;
  handler: Handler<TRequest, TResponse>;
};

class HandlerNotFoundError extends BaseError {}

export abstract class AppSyncRuntime<TDependencies> {
  abstract getDependencies(): Promise<TDependencies>;

  abstract getHandlerMap(
    dependencies: TDependencies,
  ): HandlerMapItem<unknown, unknown>[];

  async run(
    event: AWSLambda.AppSyncResolverEvent<{
      request: unknown;
    }>,
  ): Promise<unknown> {
    const dependencies = await this.getDependencies();
    const handlerMap = this.getHandlerMap(dependencies);

    const item = handlerMap.find(
      (_) =>
        _.type === event.info.parentTypeName &&
        _.field === event.info.fieldName,
    );

    if (!item) {
      throw new HandlerNotFoundError();
    }

    return item.handler.handle(event.arguments.request);
  }
}
