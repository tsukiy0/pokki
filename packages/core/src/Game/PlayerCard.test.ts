import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { CardIdRandomizer } from "./Card";
import { PlayerCard, PlayerCardSerializer } from "./PlayerCard";

describe("PlayerCard", () => {
  testComparable(
    () => new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
  );

  testSerializer(
    PlayerCardSerializer,
    () => new PlayerCard(UserIdRandomizer.random(), CardIdRandomizer.random()),
  );
});
