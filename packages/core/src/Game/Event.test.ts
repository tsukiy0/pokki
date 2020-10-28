import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { UserIdRandomizer } from "../User/User";
import { Card, CardIdRandomizer } from "./Card";
import { CardSet } from "./CardSet";
import {
  Event,
  AddPlayerEvent,
  EndRoundEvent,
  matchEvent,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
  NoEventMatchedError,
} from "./Event";
import { GameIdRandomizer } from "./Game";
import { RoundIdRandomizer } from "./Round";

describe("NewGameEvent", () => {
  testComparable(
    () =>
      new NewGameEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        new CardSet([
          new Card(CardIdRandomizer.random(), "aaaa"),
          new Card(CardIdRandomizer.random(), "bbbb"),
        ]),
      ),
  );
});

describe("AddPlayerEvent", () => {
  testComparable(
    () =>
      new AddPlayerEvent(GameIdRandomizer.random(), UserIdRandomizer.random()),
  );
});

describe("NewRoundEvent", () => {
  testComparable(
    () =>
      new NewRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        RoundIdRandomizer.random(),
        "aaaa",
      ),
  );
});

describe("PlayCardEvent", () => {
  testComparable(
    () =>
      new PlayCardEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
});

describe("EndRoundEvent", () => {
  testComparable(
    () =>
      new EndRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
  );
});

describe("matchEvent", () => {
  const actions = {
    newGame: () => "newGame",
    addPlayer: () => "addPlayer",
    newRound: () => "newRound",
    playCard: () => "playCard",
    endRound: () => "endRound",
  };

  it("matches new game", () => {
    const actual = matchEvent(
      new NewGameEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        new CardSet([]),
      ),
      actions,
    );

    expect(actual).toEqual("newGame");
  });

  it("matches add player", () => {
    const actual = matchEvent(
      new AddPlayerEvent(GameIdRandomizer.random(), UserIdRandomizer.random()),
      actions,
    );

    expect(actual).toEqual("addPlayer");
  });

  it("matches new round", () => {
    const actual = matchEvent(
      new NewRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        RoundIdRandomizer.random(),
        "round1",
      ),
      actions,
    );

    expect(actual).toEqual("newRound");
  });

  it("matches play card", () => {
    const actual = matchEvent(
      new PlayCardEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
      actions,
    );

    expect(actual).toEqual("playCard");
  });

  it("matches end round", () => {
    const actual = matchEvent(
      new EndRoundEvent(
        GameIdRandomizer.random(),
        UserIdRandomizer.random(),
        CardIdRandomizer.random(),
      ),
      actions,
    );

    expect(actual).toEqual("endRound");
  });

  it("throws when not match event", () => {
    class OtherEvent extends Event {}

    expect(() =>
      matchEvent(
        new OtherEvent(GameIdRandomizer.random(), UserIdRandomizer.random()),
        actions,
      ),
    ).toThrow(NoEventMatchedError);
  });
});
