import { Serializer, Set } from "@tsukiy0/tscore";
import {
  CompletedRound,
  CompletedRoundJson,
  CompletedRoundSerializer,
} from "./Round";

export class CompletedRoundSet extends Set<CompletedRound> {
  constructor(items: readonly CompletedRound[]) {
    super(items, (a, b) => a.equals(b));
  }
}

export type CompletedRoundSetJson = {
  items: readonly CompletedRoundJson[];
};

export const CompletedRoundSetSerializer: Serializer<
  CompletedRoundSet,
  CompletedRoundSetJson
> = {
  serialize: (input: CompletedRoundSet) => {
    return {
      items: input.items.map(CompletedRoundSerializer.serialize),
    };
  },
  deserialize: (input: CompletedRoundSetJson) => {
    return new CompletedRoundSet(
      input.items.map(CompletedRoundSerializer.deserialize),
    );
  },
};
