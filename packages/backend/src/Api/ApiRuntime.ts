import { GameService } from "@pokki/core";
import { SystemConfig } from "@tsukiy0/tscore";
import {
  AppSyncRuntime,
  GraphQlType,
  HandlerMapItem,
} from "../Aws/AppSyncRuntime";
import { DynamoEventRepository } from "../Game/DynamoEventRepository";
import { AddPlayerHandler } from "./AddPlayerHandler";
import { EndRoundHandler } from "./EndRoundHandler";
import { NewGameHandler } from "./NewGameHandler";
import { NewRoundHandler } from "./NewRoundHandler";
import { PlayCardHandler } from "./PlayCardHandler";

type Dependencies = {
  gameService: GameService;
};

export class ApiRuntime extends AppSyncRuntime<Dependencies> {
  async getDependencies(): Promise<Dependencies> {
    const config = new SystemConfig();
    const eventRepository = DynamoEventRepository.default(
      config.get("EVENT_TABLE_NAME"),
    );
    const gameService = new GameService(eventRepository);

    return {
      gameService,
    };
  }

  getHandlerMap({
    gameService,
  }: Dependencies): HandlerMapItem<unknown, unknown>[] {
    return [
      {
        type: GraphQlType.MUTATION,
        field: "NewGame",
        handler: new NewGameHandler(gameService),
      },
      {
        type: GraphQlType.MUTATION,
        field: "AddPlayer",
        handler: new AddPlayerHandler(gameService),
      },
      {
        type: GraphQlType.MUTATION,
        field: "NewRound",
        handler: new NewRoundHandler(gameService),
      },
      {
        type: GraphQlType.MUTATION,
        field: "PlayCard",
        handler: new PlayCardHandler(gameService),
      },
      {
        type: GraphQlType.MUTATION,
        field: "EndRound",
        handler: new EndRoundHandler(gameService),
      },
    ];
  }
}
