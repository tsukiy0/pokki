import { Card, Spinner, Tag } from "@blueprintjs/core";
import { CardId, EndRoundEvent } from "@pokki/core";
import { css } from "emotion";
import React, { useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useGameContext } from "../contexts/GameContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";

export const EndRoundForm: React.FC<BaseProps> = ({ className }) => {
  const { game, users } = useGameContext();
  const user = useUserContext();
  const { onError } = useAlertContext();
  const { gameService } = useServiceContext();
  const [isLoading, setIsLoading] = useState(false);

  const onEndRound = async (cardId: CardId) => {
    try {
      setIsLoading(true);
      await gameService.endRound(new EndRoundEvent(game.id, user.id, cardId));
    } catch (err) {
      onError(err);
    } finally {
      setIsLoading(false);
    }
  };

  const playerCardView = (
    <Card>
      <h1>Players</h1>
      <div
        className={css({
          display: "grid",
          gridTemplateColumns: "1fr",
          rowGap: "1rem",
          columnGap: "1rem",
        })}
      >
        {users.map((user2) => {
          const cardId = game.round?.playerCards.items.find((_) =>
            _.playerId.equals(user2.id),
          )?.cardId;
          const card = game.cards.items.find((_) =>
            _.id.equals(cardId as CardId),
          );

          return (
            <Card>
              {user2.name}
              <Tag large>{card?.name}</Tag>
            </Card>
          );
        })}
      </div>
    </Card>
  );

  return (
    <>
      {playerCardView}
      <Card className={className}>
        {isLoading ? (
          <Spinner />
        ) : (
          <>
            <h1>Result card</h1>
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
                  <Card onClick={() => onEndRound(item.id)}>{item.name}</Card>
                );
              })}
            </div>
          </>
        )}
      </Card>
    </>
  );
};
