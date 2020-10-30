import {
  EndRoundEvent,
  EndRoundEventSerializer,
  Game,
  GameSerializer,
  GameService,
} from "@pokki/core";
import { Handler } from "./Handler";

export class EndRoundHandler extends Handler<EndRoundEvent, Game> {
  constructor(public readonly gameService: GameService) {
    super(EndRoundEventSerializer, GameSerializer);
  }

  handle(request: EndRoundEvent): Promise<Game> {
    return this.gameService.endRound(request);
  }
}
