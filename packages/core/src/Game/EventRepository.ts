import { GameId } from "./Game";

export interface EventRepository {
  appendEvent(event: Event): Promise<void>;
  listEvents(gameId: GameId): Promise<readonly Event[]>;
}
