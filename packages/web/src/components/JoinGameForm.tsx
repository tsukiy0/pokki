import {
  Button,
  Card,
  FormGroup,
  InputGroup,
  Spinner,
} from "@blueprintjs/core";
import { AddPlayerEvent, GameId } from "@pokki/core";
import { css } from "emotion";
import { useRouter } from "next/router";
import React, { useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";

export const JoinGameForm: React.FC<BaseProps> = ({ className }) => {
  const { onError, onSuccess } = useAlertContext();
  const { gameService } = useServiceContext();
  const router = useRouter();
  const user = useUserContext();
  const [gameIdStr, setGameIdStr] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const onSubmit = async () => {
    try {
      setIsLoading(true);
      const gameId = new GameId(gameIdStr);
      await gameService.addPlayer(new AddPlayerEvent(gameId, user.id));
      onSuccess("game joined");
      router.push({
        pathname: "/game",
        query: {
          id: gameId.toString(),
        },
      });
      setGameIdStr("");
    } catch (err) {
      onError(err);
    } finally {
      setIsLoading(false);
    }
  };

  const loadingView = <Spinner />;

  const formView = (
    <form>
      <FormGroup
        label="ID"
        labelFor="gameId"
        inline
        contentClassName={css({
          width: "100%",
        })}
      >
        <InputGroup
          id="gameId"
          value={gameIdStr}
          onChange={(e: any) => setGameIdStr(e.target.value)}
        />
      </FormGroup>
      <Button
        className={css({
          width: "100%",
        })}
        type="submit"
        onClick={(e: any) => {
          e.preventDefault();
          onSubmit();
        }}
        intent="primary"
      >
        Join
      </Button>
    </form>
  );

  return (
    <Card className={className}>{isLoading ? loadingView : formView}</Card>
  );
};
