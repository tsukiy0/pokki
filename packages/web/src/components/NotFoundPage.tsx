import React from "react";
import { css, cx } from "emotion";
import { BaseProps } from "./BaseProps";

export const NotFoundPage: React.FC<BaseProps> = ({ className }) => {
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
      <div>
        <h1>nothing found :(</h1>
      </div>
    </div>
  );
};
