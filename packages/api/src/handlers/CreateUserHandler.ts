import { User, UserRepository, UserSerializer } from "@pokki/core";
import { Handler } from "@pokki/backend";
import { VoidResponse, VoidResponseSerializer } from "./VoidResponse";

export class CreateUserHandler extends Handler<User, VoidResponse> {
  constructor(public readonly userRepository: UserRepository) {
    super(UserSerializer, VoidResponseSerializer);
  }

  async handle(user: User): Promise<VoidResponse> {
    await this.userRepository.createUser(user);
    return new VoidResponse();
  }
}
