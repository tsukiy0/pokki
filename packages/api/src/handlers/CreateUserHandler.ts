import { User, UserSerializer, UserService } from "@pokki/core";
import { Handler } from "@pokki/backend";
import { VoidResponse, VoidResponseSerializer } from "./VoidResponse";

export class CreateUserHandler extends Handler<User, VoidResponse> {
  constructor(public readonly userService: UserService) {
    super(UserSerializer, VoidResponseSerializer);
  }

  async handle(request: User): Promise<VoidResponse> {
    await this.userService.createUser(request);
    return new VoidResponse();
  }
}
