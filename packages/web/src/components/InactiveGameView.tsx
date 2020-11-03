import { Game } from "@pokki/core";
import React from "react";
import { BaseProps } from "./BaseProps";
import { CardSetView } from "./CardSetView";

export const InactiveGameView: React.FC<BaseProps<{
  game: Game;
}>> = ({ className, game }) => {
  return (
    <div className={className}>
      <CardSetView value={game.cards} />
    </div>
  );
};
