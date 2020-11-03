import { Comparable, Serializer } from "@tsukiy0/tscore";
import { GameId } from "./Game";

export class GetGameRequest implements Comparable {
  constructor(public readonly id: GameId) {}

  equals(input: this): boolean {
    return this.id.equals(input.id);
  }
}

type GetGameRequestJson = {
  id: string;
};

export const GetGameRequestSerializer: Serializer<
  GetGameRequest,
  GetGameRequestJson
> = {
  serialize: (input: GetGameRequest): GetGameRequestJson => {
    return {
      id: input.id.toString(),
    };
  },
  deserialize: (input: GetGameRequestJson): GetGameRequest => {
    return new GetGameRequest(new GameId(input.id));
  },
};
