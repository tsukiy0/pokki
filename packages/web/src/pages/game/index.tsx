import { GameId } from "@pokki/core";
import { useRouter } from "next/router";
import React, { useEffect, useState } from "react";
import { GamePage } from "../../components/GamePage";

const Game: React.FC = () => {
  const router = useRouter();
  const [gameId, setGameId] = useState<GameId | undefined>();

  useEffect(() => {
    try {
      setGameId(new GameId(router.query.id as string));
    } catch {
      setGameId(undefined);
    }
  }, [router]);

  // @TODO make error case prettier
  if (!gameId) {
    return <>no game id</>;
  }

  return <GamePage id={gameId} />;
};

export default Game;
