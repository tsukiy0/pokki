import { Serializer } from "@tsukiy0/tscore";

export abstract class Handler<TRequest, TResponse> {
  constructor(
    private readonly requestSerializer: Serializer<TRequest, unknown>,
    private readonly responseSerializer: Serializer<TResponse, unknown>,
  ) {}

  abstract handle(request: TRequest): Promise<TResponse>;

  async run(request: unknown): Promise<unknown> {
    return this.responseSerializer.serialize(
      await this.handle(this.requestSerializer.deserialize(request)),
    );
  }
}
