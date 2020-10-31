import React from "react";
import { css, cx } from "emotion";
import { Spinner } from "@blueprintjs/core";
import { BaseProps } from "./BaseProps";

export const LoadingPage: React.FC<BaseProps> = ({ className }) => {
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
      <Spinner size={Spinner.SIZE_LARGE} />
    </div>
  );
};
