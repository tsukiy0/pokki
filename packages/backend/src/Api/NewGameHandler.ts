import {
  Game,
  GameSerializer,
  GameService,
  NewGameEvent,
  NewGameEventSerializer,
} from "@pokki/core";
import { Handler } from "../Aws/Handler";

export class NewGameHandler extends Handler<NewGameEvent, Game> {
  constructor(public readonly gameService: GameService) {
    super(NewGameEventSerializer, GameSerializer);
  }

  handle(request: NewGameEvent): Promise<Game> {
    return this.gameService.newGame(request);
  }
}
