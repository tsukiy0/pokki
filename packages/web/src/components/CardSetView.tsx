import { Card } from "@blueprintjs/core";
import { CardId, CardSet } from "@pokki/core";
import { css } from "emotion";
import React from "react";
import { BaseProps } from "./BaseProps";

export const CardSetView: React.FC<BaseProps<{
  value: CardSet;
  onClick?: (cardId: CardId) => void;
}>> = ({ className, value, onClick }) => {
  return (
    <Card className={className}>
      <h1>Cards</h1>
      <div
        className={css({
          display: "grid",
          gridTemplateColumns: "1fr",
          rowGap: "1rem",
          columnGap: "1rem",
        })}
      >
        {value.items.map((item) => {
          return (
            <Card onClick={() => onClick && onClick(item.id)}>{item.name}</Card>
          );
        })}
      </div>
    </Card>
  );
};
