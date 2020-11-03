import { Button, Card, Spinner } from "@blueprintjs/core";
import { CardSet, GameIdRandomizer, NewGameEvent } from "@pokki/core";
import { css } from "emotion";
import { useRouter } from "next/router";
import React, { useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";
import { CardSetInput } from "./CardSetInput";

export const NewGameForm: React.FC<BaseProps> = ({ className }) => {
  const { gameService } = useServiceContext();
  const { onError, onSuccess } = useAlertContext();
  const router = useRouter();
  const user = useUserContext();
  const [cardSet, setCardSet] = useState(new CardSet([]));
  const [isLoading, setIsLoading] = useState(false);

  const onSubmit = async () => {
    try {
      setIsLoading(true);
      const newGameId = GameIdRandomizer.random();
      await gameService.newGame(new NewGameEvent(newGameId, user.id, cardSet));
      onSuccess("game created");
      router.push({
        pathname: "/game",
        query: {
          id: newGameId.toString(),
        },
      });
    } catch (err) {
      onError(err);
    } finally {
      setIsLoading(false);
    }
  };

  const formView = (
    <form>
      <CardSetInput value={cardSet} onChange={setCardSet} />
      <Button
        className={css({
          width: "100%",
        })}
        type="submit"
        onClick={(e: any) => {
          e.preventDefault();
          onSubmit();
        }}
        intent="success"
      >
        Create
      </Button>
    </form>
  );

  const loadingView = <Spinner />;

  return (
    <Card className={className}>{isLoading ? loadingView : formView}</Card>
  );
};
