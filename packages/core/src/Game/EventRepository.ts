import { GameId } from "./Game";
import { Event } from "./Event";

export interface EventRepository {
  appendEvent(event: Event): Promise<void>;
  listEvents(gameId: GameId): Promise<readonly Event[]>;
}
