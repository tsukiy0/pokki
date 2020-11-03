import { Card, Spinner } from "@blueprintjs/core";
import { GetUserRequest, PlayerRoleSet, User } from "@pokki/core";
import { css } from "emotion";
import React, { useEffect, useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { useServiceContext } from "../contexts/ServiceContext";
import { BaseProps } from "./BaseProps";

export const PlayerSetView: React.FC<BaseProps<{
  value: PlayerRoleSet;
}>> = ({ className, value }) => {
  const { onError } = useAlertContext();
  const { userService } = useServiceContext();
  const [players, setPlayers] = useState<User[] | undefined>();

  useEffect(() => {
    (async () => {
      try {
        setPlayers(
          await Promise.all(
            value.items.map((item) => {
              return userService.getUser(new GetUserRequest(item.playerId));
            }),
          ),
        );
      } catch (err) {
        onError(err);
      }
    })();
  }, [onError, userService]);

  return (
    <Card className={className}>
      {players ? (
        <>
          <h1>Players</h1>
          <div
            className={css({
              display: "grid",
              gridTemplateColumns: "1fr 1fr 1fr",
              rowGap: "1rem",
              columnGap: "1rem",
            })}
          >
            {players.map((player) => {
              return <Card>{player.name}</Card>;
            })}
          </div>
        </>
      ) : (
        <Spinner />
      )}
    </Card>
  );
};
