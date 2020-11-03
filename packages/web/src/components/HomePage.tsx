import React from "react";
import { css, cx } from "emotion";
import { Button, Card, ControlGroup } from "@blueprintjs/core";
import { useRouter } from "next/router";
import { BaseProps } from "./BaseProps";

export const HomePage: React.FC<BaseProps> = ({ className }) => {
  const router = useRouter();

  const goToNew = () => {
    router.push({
      pathname: "/game/new",
    });
  };

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
      <Card
        className={css({
          display: "flex",
          flexDirection: "column",
        })}
      >
        <Button
          className={css({
            marginBottom: "1rem",
            width: "10rem",
          })}
          onClick={goToNew}
          intent="success"
          large
        >
          Create
        </Button>
        <ControlGroup>
          <Button
            className={css({
              width: "10rem",
            })}
            onClick={console.log}
            intent="primary"
            large
          >
            Join
          </Button>
        </ControlGroup>
      </Card>
    </div>
  );
};
