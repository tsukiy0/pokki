import {
  Game,
  GameSerializer,
  GameService,
  PlayCardEvent,
  PlayCardEventSerializer,
} from "@pokki/core";
import { Handler } from "@pokki/backend";

export class PlayCardHandler extends Handler<PlayCardEvent, Game> {
  constructor(public readonly gameService: GameService) {
    super(PlayCardEventSerializer, GameSerializer);
  }

  handle(request: PlayCardEvent): Promise<Game> {
    return this.gameService.playCard(request);
  }
}
