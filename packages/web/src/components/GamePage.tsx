import { Card } from "@blueprintjs/core";
import { GameStatus } from "@pokki/core";
import React from "react";
import { useGameContext } from "../contexts/GameContext";
import { BaseProps } from "./BaseProps";
import { InactiveGameView } from "./InactiveGameView";

export const GamePage: React.FC<BaseProps> = ({ className }) => {
  const { game } = useGameContext();

  if (game.status === GameStatus.INACTIVE) {
    return <InactiveGameView game={game} />;
  }

  return <Card className={className}>{game.id.toString()}</Card>;
};
