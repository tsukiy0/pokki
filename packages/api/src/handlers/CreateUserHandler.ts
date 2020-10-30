import { User, UserSerializer, UserService } from "@pokki/core";
import { Handler } from "@pokki/backend";
import { Void, VoidSerializer } from "./Void";

export class CreateUserHandler extends Handler<User, Void> {
  constructor(public readonly userService: UserService) {
    super(UserSerializer, VoidSerializer);
  }

  async handle(request: User): Promise<Void> {
    await this.userService.createUser(request);
    return new Void();
  }
}
