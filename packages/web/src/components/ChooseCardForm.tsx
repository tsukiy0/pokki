import { Card, Spinner } from "@blueprintjs/core";
import { CardId, PlayCardEvent } from "@pokki/core";
import { css } from "emotion";
import React, { useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useGameContext } from "../contexts/GameContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";

export const ChooseCardForm: React.FC<BaseProps> = ({ className }) => {
  const { onError } = useAlertContext();
  const { gameService } = useServiceContext();
  const { game } = useGameContext();
  const user = useUserContext();
  const [isLoading, setIsLoading] = useState(false);

  const onPlayCard = async (cardId: CardId) => {
    try {
      setIsLoading(true);
      await gameService.playCard(new PlayCardEvent(game.id, user.id, cardId));
    } catch (err) {
      onError(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card className={className}>
      {isLoading ? (
        <Spinner />
      ) : (
        <>
          <h1>Play card</h1>
          <div
            className={css({
              display: "grid",
              gridTemplateColumns: "1fr",
              rowGap: "1rem",
              columnGap: "1rem",
            })}
          >
            {game.cards.items.map((item) => {
              return (
                <Card onClick={() => onPlayCard(item.id)}>{item.name}</Card>
              );
            })}
          </div>
        </>
      )}
    </Card>
  );
};
