import {
  GetUserRequest,
  GetUserRequestSerializer,
  User,
  UserSerializer,
  UserService,
} from "@pokki/core";
import { Handler } from "@pokki/backend";

export class GetUserHandler extends Handler<GetUserRequest, User> {
  constructor(public readonly userService: UserService) {
    super(GetUserRequestSerializer, UserSerializer);
  }

  handle(request: GetUserRequest): Promise<User> {
    return this.userService.getUser(request);
  }
}
