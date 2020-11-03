import { Game, Role } from "@pokki/core";
import React from "react";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";
import { CardSetView } from "./CardSetView";
import { NewRoundForm } from "./NewRoundForm";
import { PlayerSetView } from "./PlayerSetView";

export const InactiveGameView: React.FC<BaseProps<{
  game: Game;
}>> = ({ className, game }) => {
  const user = useUserContext();

  const isAdmin = game.players.getPlayer(user.id)?.role === Role.ADMIN;

  return (
    <div className={className}>
      <CardSetView value={game.cards} />
      <PlayerSetView value={game.players} />
      {isAdmin && <NewRoundForm gameId={game.id} />}
    </div>
  );
};
