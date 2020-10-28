import { User, UserId } from "./User";

export interface UserRepository {
  createUser(user: User): Promise<void>;
  getUser(id: UserId): Promise<User>;
}
