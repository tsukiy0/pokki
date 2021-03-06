import { GameId } from "@pokki/core";
import { useRouter } from "next/router";
import React, { useEffect, useState } from "react";
import { GamePage } from "../../components/GamePage";
import { NotFoundPage } from "../../components/NotFoundPage";
import { GameContextProvider } from "../../contexts/GameContext";

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
    return <NotFoundPage />;
  }

  return (
    <GameContextProvider id={gameId}>
      <GamePage />
    </GameContextProvider>
  );
};

export default Game;
