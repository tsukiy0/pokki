import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { Card, CardIdRandomizer } from "./Card";
import { CardSet } from "./CardSet";
import { Game, GameIdRandomizer, GameSerializer, GameStatus } from "./Game";
import { PlayerCard } from "./PlayerCard";
import { PlayerCardSet } from "./PlayerCardSet";
import { PlayerRole } from "./PlayerRole";
import { PlayerRoleSet } from "./PlayerRoleSet";
import { Role } from "./Role";
import { Round, RoundIdRandomizer } from "./Round";

describe("Game", () => {
  testComparable(
    () =>
      new Game(
        GameIdRandomizer.random(),
        GameStatus.ACTIVE,
        new CardSet([new Card(CardIdRandomizer.random(), "card1")]),
        new PlayerRoleSet([
          new PlayerRole(UserIdRandomizer.random(), Role.ADMIN),
          new PlayerRole(UserIdRandomizer.random(), Role.PLAYER),
        ]),
        new Round(
          RoundIdRandomizer.random(),
          "round1",
          new PlayerCardSet([
            new PlayerCard(
              UserIdRandomizer.random(),
              CardIdRandomizer.random(),
            ),
          ]),
        ),
      ),
  );

  testSerializer(
    GameSerializer,
    () =>
      new Game(
        GameIdRandomizer.random(),
        GameStatus.ACTIVE,
        new CardSet([new Card(CardIdRandomizer.random(), "card1")]),
        new PlayerRoleSet([
          new PlayerRole(UserIdRandomizer.random(), Role.ADMIN),
          new PlayerRole(UserIdRandomizer.random(), Role.PLAYER),
        ]),
        new Round(
          RoundIdRandomizer.random(),
          "round1",
          new PlayerCardSet([
            new PlayerCard(
              UserIdRandomizer.random(),
              CardIdRandomizer.random(),
            ),
          ]),
        ),
      ),
  );
});
