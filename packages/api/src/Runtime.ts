import { BaseError, SystemConfig } from "@tsukiy0/tscore";
import { GameService } from "@pokki/core";
import { DynamoEventRepository, DynamoUserRepository } from "@pokki/backend";
import { CreateUserHandler } from "./handlers/CreateUserHandler";
import { GetUserHandler } from "./handlers/GetUserHandler";
import { NewGameHandler } from "./handlers/NewGameHandler";
import { AddPlayerHandler } from "./handlers/AddPlayerHandler";
import { NewRoundHandler } from "./handlers/NewRoundHandler";
import { PlayCardHandler } from "./handlers/PlayCardHandler";
import { EndRoundHandler } from "./handlers/EndRoundHandler";

export enum GraphQlType {
  MUTATION = "Mutation",
  QUERY = "Query",
}

class HandlerNotFoundError extends BaseError {}

export class AppSyncRuntime {
  async run(
    event: AWSLambda.AppSyncResolverEvent<{
      request: unknown;
    }>,
  ): Promise<unknown> {
    const config = new SystemConfig();
    const eventRepository = DynamoEventRepository.default(
      config.get("EVENT_TABLE_NAME"),
    );
    const userRepository = DynamoUserRepository.default(
      config.get("USER_TABLE_NAME"),
    );
    const gameService = new GameService(eventRepository);

    const item = [
      {
        type: GraphQlType.MUTATION,
        field: "CreateUser",
        handler: new CreateUserHandler(userRepository),
      },
      {
        type: GraphQlType.QUERY,
        field: "GetUser",
        handler: new GetUserHandler(userRepository),
      },
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
    ].find(
      (_) =>
        _.type === event.info.parentTypeName &&
        _.field === event.info.fieldName,
    );

    if (!item) {
      throw new HandlerNotFoundError();
    }

    return item.handler.run(event.arguments.request);
  }
}
