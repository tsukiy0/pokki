import { GameId } from "@pokki/core";
import { Comparable, Serializer } from "@tsukiy0/tscore";

export class OnGameRequest implements Comparable {
  constructor(public readonly gameId: GameId) {}

  equals(input: this): boolean {
    return this.gameId.equals(input.gameId);
  }
}

export type OngameRequestJson = {
  gameId: string;
};

export const OnGameRequestSerializer: Serializer<
  OnGameRequest,
  OngameRequestJson
> = {
  serialize: (input: OnGameRequest): OngameRequestJson => {
    return {
      gameId: input.gameId.toString(),
    };
  },
  deserialize: (input: OngameRequestJson): OnGameRequest => {
    return new OnGameRequest(new GameId(input.gameId));
  },
};
