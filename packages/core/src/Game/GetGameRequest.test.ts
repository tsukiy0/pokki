import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { GameIdRandomizer } from "./Game";
import { GetGameRequest, GetGameRequestSerializer } from "./GetGameRequest";

describe("GetGameRequest", () => {
  testComparable(() => new GetGameRequest(GameIdRandomizer.random()));
  testSerializer(
    GetGameRequestSerializer,
    () => new GetGameRequest(GameIdRandomizer.random()),
  );
});
