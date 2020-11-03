import {
  Game,
  GameSerializer,
  GameService,
  GetGameRequest,
  GetGameRequestSerializer,
} from "@pokki/core";
import { Handler } from "@pokki/backend";

export class GetGameHandler extends Handler<GetGameRequest, Game> {
  constructor(public readonly gameService: GameService) {
    super(GetGameRequestSerializer, GameSerializer);
  }

  handle(request: GetGameRequest): Promise<Game> {
    return this.gameService.getGame(request);
  }
}
