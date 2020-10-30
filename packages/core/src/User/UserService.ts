import { GetUserRequest } from "./GetUserRequest";
import { User } from "./User";
import { UserRepository } from "./UserRepository";

export class UserService {
  constructor(public readonly userRepository: UserRepository) {}

  async createUser(request: User): Promise<void> {
    await this.userRepository.createUser(request);
  }

  getUser(request: GetUserRequest): Promise<User> {
    return this.userRepository.getUser(request.id);
  }
}
