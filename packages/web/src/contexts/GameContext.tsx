import {
  Game,
  GameId,
  GetGameRequest,
  GetUserRequest,
  User,
} from "@pokki/core";
import React, { useContext, useEffect, useState } from "react";
import { LoadingPage } from "../components/LoadingPage";
import { useAlertContext } from "./AlertContext";
import { useServiceContext } from "./ServiceContext";

type State = {
  game: Game;
  users: readonly User[];
};

const GameContext = React.createContext<State>({} as any);

export const GameContextProvider: React.FC<{
  id: GameId;
}> = ({ id, children }) => {
  const { onError } = useAlertContext();
  const { gameService, userService } = useServiceContext();
  const [state, setState] = useState<State | undefined>();

  useEffect(() => {
    (async () => {
      try {
        const game = await gameService.getGame(new GetGameRequest(id));
        const users = await Promise.all(
          game.players.items.map((item) => {
            return userService.getUser(new GetUserRequest(item.playerId));
          }),
        );

        setState({
          game,
          users,
        });
      } catch (err) {
        onError(err);
      }
    })();
  }, [id, gameService, onError]);

  if (!state) {
    return <LoadingPage />;
  }

  return <GameContext.Provider value={state}>{children}</GameContext.Provider>;
};

export const useGameContext = (): State => {
  return useContext(GameContext);
};
