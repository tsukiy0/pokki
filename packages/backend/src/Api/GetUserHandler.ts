import { User, UserId, UserRepository, UserSerializer } from "@pokki/core";
import { Serializer } from "@tsukiy0/tscore";
import { Handler } from "./Handler";

export class GetUserRequest {
  constructor(public readonly id: UserId) {}
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

export class GetUserHandler extends Handler<GetUserRequest, User> {
  constructor(public readonly userRepository: UserRepository) {
    super(GetUserRequestSerializer, UserSerializer);
  }

  handle(request: GetUserRequest): Promise<User> {
    return this.userRepository.getUser(request.id);
  }
}
