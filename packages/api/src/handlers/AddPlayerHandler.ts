import {
  AddPlayerEvent,
  AddPlayerEventSerializer,
  Game,
  GameSerializer,
  GameService,
} from "@pokki/core";
import { Handler } from "@pokki/backend";

export class AddPlayerHandler extends Handler<AddPlayerEvent, Game> {
  constructor(public readonly gameService: GameService) {
    super(AddPlayerEventSerializer, GameSerializer);
  }

  handle(request: AddPlayerEvent): Promise<Game> {
    return this.gameService.addPlayer(request);
  }
}
