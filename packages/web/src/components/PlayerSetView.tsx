import { Card } from "@blueprintjs/core";
import { PlayerRoleSet } from "@pokki/core";
import { css } from "emotion";
import React from "react";
import { BaseProps } from "./BaseProps";

export const PlayerSetView: React.FC<BaseProps<{
  value: PlayerRoleSet;
}>> = ({ className, value }) => {
  return (
    <Card className={className}>
      <h1>Players</h1>
      <div
        className={css({
          display: "grid",
          gridTemplateColumns: "1fr 1fr 1fr",
          rowGap: "1rem",
          columnGap: "1rem",
        })}
      >
        {value.items.map((item) => {
          return <Card>{item.playerId.toString()}</Card>;
        })}
      </div>
    </Card>
  );
};
