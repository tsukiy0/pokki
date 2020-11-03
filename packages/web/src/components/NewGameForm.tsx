import { Button, Card } from "@blueprintjs/core";
import { CardSet, GameId, GameIdRandomizer, NewGameEvent } from "@pokki/core";
import React, { useState } from "react";
import { useServiceContext } from "../contexts/ServiceContext";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";
import { CardSetInput } from "./CardSetInput";

export const NewGameForm: React.FC<BaseProps> = ({ className }) => {
  const { gameService } = useServiceContext();
  const user = useUserContext();
  const [cardSet, setCardSet] = useState(new CardSet([]));
  const [gameId, setGameId] = useState<GameId | undefined>();

  const onNewGame = async () => {
    const newGameId = GameIdRandomizer.random();
    await gameService.newGame(new NewGameEvent(newGameId, user.id, cardSet));
    setGameId(newGameId);
  };

  if (gameId) {
    return <div>{gameId.toString()}</div>;
  }

  return (
    <Card className={className}>
      <CardSetInput value={cardSet} onChange={setCardSet} />
      <Button onClick={onNewGame}>Submit</Button>
    </Card>
  );
};
