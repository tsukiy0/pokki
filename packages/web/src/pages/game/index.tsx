import { GameId } from "@pokki/core";
import { useRouter } from "next/router";
import React, { useEffect, useState } from "react";

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

  if (!gameId) {
    return <>no game id</>;
  }

  return <>{gameId.toString()}</>;
};

export default Game;
