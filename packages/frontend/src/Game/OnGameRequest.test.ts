import { GameIdRandomizer } from "@pokki/core";
import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { OnGameRequest, OnGameRequestSerializer } from "./OnGameRequest";

describe("OnGameRequest", () => {
  testComparable(() => new OnGameRequest(GameIdRandomizer.random()));
  testSerializer(
    OnGameRequestSerializer,
    () => new OnGameRequest(GameIdRandomizer.random()),
  );
});
