import {
  Button,
  Card,
  FormGroup,
  InputGroup,
  Spinner,
} from "@blueprintjs/core";
import { GameId, NewRoundEvent, RoundIdRandomizer } from "@pokki/core";
import { css } from "emotion";
import React, { useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { useUserContext } from "../contexts/UserContext";
import { BaseProps } from "./BaseProps";

export const NewRoundForm: React.FC<BaseProps<{
  gameId: GameId;
}>> = ({ className, gameId }) => {
  const { onError, onSuccess } = useAlertContext();
  const { gameService } = useServiceContext();
  const user = useUserContext();
  const [roundName, setRoundName] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const onSubmit = async () => {
    try {
      setIsLoading(true);
      await gameService.newRound(
        new NewRoundEvent(
          gameId,
          user.id,
          RoundIdRandomizer.random(),
          roundName,
        ),
      );
      onSuccess("round started");
      setRoundName("");
    } catch (err) {
      onError(err);
    } finally {
      setIsLoading(false);
    }
  };

  const formView = (
    <form>
      <FormGroup
        label="Name"
        labelFor="roundName"
        inline
        contentClassName={css({
          width: "100%",
        })}
      >
        <InputGroup
          id="roundName"
          value={roundName}
          onChange={(e: any) => setRoundName(e.target.value)}
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
        Start Round
      </Button>
    </form>
  );

  return (
    <Card className={className}>{isLoading ? <Spinner /> : formView}</Card>
  );
};
