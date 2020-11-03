import {
  Button,
  Card,
  ControlGroup,
  FormGroup,
  InputGroup,
  Spinner,
} from "@blueprintjs/core";
import { AddPlayerEvent, GameId } from "@pokki/core";
import { css } from "emotion";
import React, { useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { useUserContext } from "../contexts/UserContext";

export const JoinGameForm: React.FC<BaseProps> = ({ className }) => {
  const { onError } = useAlertContext();
  const { gameService } = useServiceContext();
  const user = useUserContext();
  const [gameIdStr, setGameIdStr] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const onJoin = async () => {
    try {
      setIsLoading(true);
      const gameId = new GameId(gameIdStr);
      await gameService.addPlayer(new AddPlayerEvent(gameId, user.id));
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
      <FormGroup label="ID" labelFor="gameId" inline>
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
          onJoin();
        }}
        large
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
