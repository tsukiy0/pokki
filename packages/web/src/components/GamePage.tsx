import { Card } from "@blueprintjs/core";
import { Game, GameId, GetGameRequest } from "@pokki/core";
import React, { useEffect, useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { BaseProps } from "./BaseProps";
import { LoadingPage } from "./LoadingPage";

export const GamePage: React.FC<BaseProps<{
  id: GameId;
}>> = ({ className, id }) => {
  const { onError } = useAlertContext();
  const { gameService } = useServiceContext();
  const [game, setGame] = useState<Game | undefined>();

  useEffect(() => {
    (async () => {
      try {
        setGame(await gameService.getGame(new GetGameRequest(id)));
      } catch (err) {
        onError(err);
      }
    })();
  }, [id, gameService]);

  if (!game) {
    return <LoadingPage />;
  }

  return <Card className={className}>{game.id.toString()}</Card>;
};
