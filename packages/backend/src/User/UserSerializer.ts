import { User, UserId } from "@pokki/core";
import { Serializer } from "@tsukiy0/tscore";
import { AttributeMap } from "aws-sdk/clients/dynamodb";

export const UserSerializer: Serializer<User, AttributeMap> = {
  serialize: (input) => ({
    id: { S: input.id.toString() },
    name: { S: input.name },
  }),
  deserialize: (input) =>
    new User(new UserId(input.id.S as string), input.name.S as string),
};
