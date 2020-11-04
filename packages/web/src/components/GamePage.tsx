import { Card } from "@blueprintjs/core";
import { GameStatus, Role } from "@pokki/core";
import React from "react";
import { useGameContext } from "../contexts/GameContext";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";
import { ChooseCardForm } from "./ChooseCardForm";
import { EndRoundForm } from "./EndRoundForm";
import { NewRoundForm } from "./NewRoundForm";

export const GamePage: React.FC<BaseProps> = ({ className }) => {
  const { game } = useGameContext();
  const user = useUserContext();

  const isAdmin = game.players.getPlayer(user.id)?.role === Role.ADMIN;

  if (game.status === GameStatus.INACTIVE) {
    if (isAdmin) {
      return <NewRoundForm />;
    }
    return <Card>waiting for new round</Card>;
  }

  if (GameStatus.ACTIVE && game.round) {
    const hasChosenCard =
      game.round.playerCards.items.find((_) => _.playerId.equals(user.id)) !==
      undefined;

    const hasAllPlayersPlayed =
      game.players.items.length === game.round.playerCards.items.length;

    if (hasAllPlayersPlayed) {
      return <EndRoundForm />;
    }

    if (hasChosenCard) {
      return <Card>waiting for other players</Card>;
    }

    return <ChooseCardForm />;
  }

  return <Card className={className}>{game.id.toString()}</Card>;
};
