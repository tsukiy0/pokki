import {
  Game,
  GameId,
  GetGameRequest,
  GetUserRequest,
  PlayerRoleSet,
  User,
} from "@pokki/core";
import { OnGameRequest } from "@pokki/frontend";
import { isArrayEqual } from "@tsukiy0/tscore";
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

  const getUsers = async (players: PlayerRoleSet): Promise<readonly User[]> => {
    return Promise.all(
      players.items.map((item) => {
        return userService.getUser(new GetUserRequest(item.playerId));
      }),
    );
  };

  useEffect(() => {
    (async () => {
      try {
        const game = await gameService.getGame(new GetGameRequest(id));
        const users = await getUsers(game.players);

        setState({
          game,
          users,
        });
      } catch (err) {
        onError(err);
      }
    })();
  }, [id, gameService, onError]);

  useEffect(() => {
    return gameService.onGame(new OnGameRequest(id), async (game) => {
      if (state) {
        const isSameUsers = isArrayEqual(
          game.players.items.map((_) => _.playerId),
          state.users.map((_) => _.id),
          (a, b) => a.equals(b),
        );
        const users = isSameUsers ? state.users : await getUsers(game.players);

        setState({
          game,
          users,
        });
      }
    });
  }, [id]);

  if (!state) {
    return <LoadingPage />;
  }

  return <GameContext.Provider value={state}>{children}</GameContext.Provider>;
};

export const useGameContext = (): State => {
  return useContext(GameContext);
};
