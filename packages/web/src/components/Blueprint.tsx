import React from "react";
import classNames from "classnames";
import "normalize.css";
import "@blueprintjs/core/lib/css/blueprint.css";

export const Blueprint: React.FC<{
  isDark: boolean;
}> = ({ children, isDark }) => {
  return (
    <div
      className={classNames({
        "bp3-dark": isDark,
      })}
    >
      {children}
    </div>
  );
};
