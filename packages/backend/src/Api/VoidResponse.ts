import { Serializer } from "@tsukiy0/tscore";

export class VoidResponse {
  public readonly void = true;
}

export type VoidResponseJson = {
  void: true;
};

export const VoidResponseSerializer: Serializer<
  VoidResponse,
  VoidResponseJson
> = {
  serialize: (): VoidResponseJson => {
    return {
      void: true,
    };
  },
  deserialize: (): VoidResponse => {
    return new VoidResponse();
  },
};
