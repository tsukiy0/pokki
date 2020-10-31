import React from "react";
import { css, cx } from "emotion";
import { Colors } from "@blueprintjs/core";
import "normalize.css";
import "@blueprintjs/core/lib/css/blueprint.css";

export const Blueprint: React.FC<{
  isDark: boolean;
}> = ({ children, isDark }) => {
  return (
    <div
      className={cx(
        css({
          background: Colors.DARK_GRAY4,
          height: "100vh",
          width: "100vw",
        }),
        {
          "bp3-dark": isDark,
        },
      )}
    >
      {children}
    </div>
  );
};
