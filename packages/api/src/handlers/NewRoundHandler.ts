import {
  Game,
  GameSerializer,
  GameService,
  NewRoundEvent,
  NewRoundEventSerializer,
} from "@pokki/core";
import { Handler } from "@pokki/backend";

export class NewRoundHandler extends Handler<NewRoundEvent, Game> {
  constructor(public readonly gameService: GameService) {
    super(NewRoundEventSerializer, GameSerializer);
  }

  handle(request: NewRoundEvent): Promise<Game> {
    return this.gameService.newRound(request);
  }
}
