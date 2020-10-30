import { Serializer } from "@tsukiy0/tscore";

export class Void {
  public readonly void = true;
}

export type VoidJson = {
  void: true;
};

export const VoidSerializer: Serializer<Void, VoidJson> = {
  serialize: (): VoidJson => {
    return {
      void: true,
    };
  },
  deserialize: (): Void => {
    return new Void();
  },
};
