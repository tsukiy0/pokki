import { Comparable, Serializer } from "@tsukiy0/tscore";
import { UserId } from "./User";

export class GetUserRequest implements Comparable {
  constructor(public readonly id: UserId) {}

  equals(input: this): boolean {
    return this.id.equals(input.id);
  }
}

export type GetUserRequestJson = {
  id: string;
};

export const GetUserRequestSerializer: Serializer<
  GetUserRequest,
  GetUserRequestJson
> = {
  serialize: (input: GetUserRequest): GetUserRequestJson => {
    return {
      id: input.id.toString(),
    };
  },
  deserialize: (input: GetUserRequestJson): GetUserRequest => {
    return new GetUserRequest(new UserId(input.id));
  },
};
