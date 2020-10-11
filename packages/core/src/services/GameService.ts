import {
  AddCardsEvent,
  AddPersonEvent,
  DecideResultEvent,
  NewGameEvent,
  NewRoundEvent,
  PersonSelectCardEvent,
} from "../models/Event";

export interface GameService {
  newGame(event: NewGameEvent): Promise<void>;
  addPerson(event: AddPersonEvent): Promise<void>;
  addCards(event: AddCardsEvent): Promise<void>;
  newRound(event: NewRoundEvent): Promise<void>;
  personSelectCard(event: PersonSelectCardEvent): Promise<void>;
  decideResult(event: DecideResultEvent): Promise<void>;
}
