import React from "react";
import { css, cx } from "emotion";
import { BaseProps } from "./BaseProps";
import { JoinGameForm } from "./JoinGameForm";
import { NewGameForm } from "./NewGameForm";

export const HomePage: React.FC<BaseProps> = ({ className }) => {
  return (
    <div
      className={cx(
        css({
          height: "100vh",
          width: "100vw",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }),
        className,
      )}
    >
      <div
        className={css({
          display: "grid",
          gridTemplateColumns: "1fr",
          columnGap: "1rem",
          rowGap: "1rem",
          width: "25rem",
        })}
      >
        <NewGameForm />
        <JoinGameForm />
      </div>
    </div>
  );
};
