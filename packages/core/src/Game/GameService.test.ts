import { EventRepository } from "./EventRepository";
import {
  GameActiveError,
  GameInactiveError,
  GameService,
  NoCardError,
  NoNewGameError,
  NoPlayerError,
  NotAdminError,
  NotAllPlayersPlayedError,
} from "./GameService";
import {
  AddPlayerEvent,
  EndRoundEvent,
  Event,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
} from "./Event";
import { Game, GameIdRandomizer, GameStatus } from "./Game";
import { UserIdRandomizer } from "../User/User";
import { CardSet } from "./CardSet";
import { Card, CardIdRandomizer } from "./Card";
import { DuplicatePlayerRoleError, PlayerRoleSet } from "./PlayerRoleSet";
import { PlayerRole } from "./PlayerRole";
import { Role } from "./Role";
import { Round, RoundIdRandomizer } from "./Round";
import { DuplicatePlayerCardError, PlayerCardSet } from "./PlayerCardSet";
import { PlayerCard } from "./PlayerCard";

describe("GameService", () => {
  const gameId = GameIdRandomizer.random();
  const adminId = UserIdRandomizer.random();
  const playerId = UserIdRandomizer.random();
  const cards = new CardSet([
    new Card(CardIdRandomizer.random(), "card1"),
    new Card(CardIdRandomizer.random(), "card2"),
  ]);
  const roundId = RoundIdRandomizer.random();

  const getEventRepository = (events: readonly Event[]): EventRepository => ({
    appendEvent: jest.fn().mockResolvedValue(undefined),
    listEvents: jest.fn().mockResolvedValue(events),
  });

  const withNewGame = (): Event[] => {
    return [new NewGameEvent(gameId, adminId, cards)];
  };

  const withPlayers = (): Event[] => {
    return [...withNewGame(), new AddPlayerEvent(gameId, playerId)];
  };

  const withNewRound = (): Event[] => {
    return [
      ...withPlayers(),
      new NewRoundEvent(gameId, adminId, roundId, "round1"),
    ];
  };

  const withSomePlayersPlayed = (): Event[] => {
    return [
      ...withNewRound(),
      new PlayCardEvent(gameId, playerId, cards.items[0].id),
    ];
  };

  const withAllPlayersPlayed = (): Event[] => {
    return [
      ...withNewRound(),
      new PlayCardEvent(gameId, adminId, cards.items[0].id),
      new PlayCardEvent(gameId, playerId, cards.items[1].id),
    ];
  };

  describe("newGame", () => {
    it("creates new game", async () => {
      const eventRepository = getEventRepository([]);
      const service = new GameService(eventRepository);
      const event = new NewGameEvent(gameId, adminId, cards);

      const actual = await service.newGame(event);

      expect(eventRepository.appendEvent).toHaveBeenCalledWith(event);
      expect(
        actual.equals(
          new Game(
            gameId,
            GameStatus.INACTIVE,
            cards,
            new PlayerRoleSet([new PlayerRole(adminId, Role.ADMIN)]),
          ),
        ),
      ).toBeTruthy();
    });

    it("throw when no new game", async () => {
      const eventRepository = getEventRepository([]);
      const service = new GameService(eventRepository);
      const event = new AddPlayerEvent(gameId, playerId);

      await expect(service.addPlayer(event)).rejects.toThrow(NoNewGameError);
    });
  });

  describe("addPlayer", () => {
    it("adds player", async () => {
      const eventRepository = getEventRepository(withNewGame());
      const service = new GameService(eventRepository);
      const event = new AddPlayerEvent(gameId, playerId);

      const actual = await service.addPlayer(event);

      expect(eventRepository.appendEvent).toHaveBeenCalledWith(event);
      expect(actual.players.getPlayer(playerId));
    });

    it("throw when duplicate player", async () => {
      const eventRepository = getEventRepository(withNewGame());
      const service = new GameService(eventRepository);
      const event = new AddPlayerEvent(gameId, adminId);

      await expect(service.addPlayer(event)).rejects.toThrow(
        DuplicatePlayerRoleError,
      );
    });

    it("throws when active", async () => {
      const eventRepository = getEventRepository(withNewRound());
      const service = new GameService(eventRepository);
      const event = new AddPlayerEvent(gameId, UserIdRandomizer.random());

      await expect(service.addPlayer(event)).rejects.toThrow(GameActiveError);
    });
  });

  describe("newRound", () => {
    it("start new round", async () => {
      const eventRepository = getEventRepository(withPlayers());
      const service = new GameService(eventRepository);
      const event = new NewRoundEvent(gameId, adminId, roundId, "round1");

      const actual = await service.newRound(event);

      expect(eventRepository.appendEvent).toHaveBeenCalledWith(event);
      expect(actual.status).toEqual(GameStatus.ACTIVE);
      expect(
        actual.round?.equals(
          new Round(roundId, "round1", new PlayerCardSet([])),
        ),
      ).toBeTruthy();
    });

    it("throws when not admin", async () => {
      const eventRepository = getEventRepository(withPlayers());
      const service = new GameService(eventRepository);
      const event = new NewRoundEvent(gameId, playerId, roundId, "round1");

      await expect(service.newRound(event)).rejects.toThrow(NotAdminError);
    });

    it("throws when active", async () => {
      const eventRepository = getEventRepository(withNewRound());
      const service = new GameService(eventRepository);
      const event = new NewRoundEvent(
        gameId,
        adminId,
        RoundIdRandomizer.random(),
        "round2",
      );

      await expect(service.addPlayer(event)).rejects.toThrow(GameActiveError);
    });
  });

  describe("playCard", () => {
    it("plays card", async () => {
      const eventRepository = getEventRepository(withNewRound());
      const service = new GameService(eventRepository);
      const event = new PlayCardEvent(gameId, playerId, cards.items[0].id);

      const actual = await service.playCard(event);

      expect(eventRepository.appendEvent).toHaveBeenCalledWith(event);
      expect(
        actual.round?.playerCards.equals(
          new PlayerCardSet([new PlayerCard(playerId, cards.items[0].id)]),
        ),
      ).toBeTruthy();
    });

    it("throws when player already played", async () => {
      const eventRepository = getEventRepository(withSomePlayersPlayed());
      const service = new GameService(eventRepository);
      const event = new PlayCardEvent(gameId, playerId, cards.items[0].id);

      await expect(service.playCard(event)).rejects.toThrow(
        DuplicatePlayerCardError,
      );
    });

    it("throws when no player", async () => {
      const eventRepository = getEventRepository(withNewRound());
      const service = new GameService(eventRepository);
      const event = new PlayCardEvent(
        gameId,
        UserIdRandomizer.random(),
        cards.items[0].id,
      );

      await expect(service.playCard(event)).rejects.toThrow(NoPlayerError);
    });

    it("throws when no card", async () => {
      const eventRepository = getEventRepository(withNewRound());
      const service = new GameService(eventRepository);
      const event = new PlayCardEvent(
        gameId,
        adminId,
        CardIdRandomizer.random(),
      );

      await expect(service.playCard(event)).rejects.toThrow(NoCardError);
    });

    it("throws when inactive", async () => {
      const eventRepository = getEventRepository(withPlayers());
      const service = new GameService(eventRepository);
      const event = new PlayCardEvent(gameId, adminId, cards.items[0].id);

      await expect(service.playCard(event)).rejects.toThrow(GameInactiveError);
    });
  });

  describe("endRound", () => {
    it("ends round", async () => {
      const eventRepository = getEventRepository(withAllPlayersPlayed());
      const service = new GameService(eventRepository);
      const event = new EndRoundEvent(gameId, adminId, cards.items[0].id);

      const actual = await service.endRound(event);

      expect(eventRepository.appendEvent).toHaveBeenCalledWith(event);
      expect(actual.status).toEqual(GameStatus.INACTIVE);
      expect(actual.round).toBeUndefined();
    });

    it("throws when not admin", async () => {
      const eventRepository = getEventRepository(withAllPlayersPlayed());
      const service = new GameService(eventRepository);
      const event = new EndRoundEvent(
        gameId,
        playerId,
        CardIdRandomizer.random(),
      );

      await expect(service.endRound(event)).rejects.toThrow(NotAdminError);
    });

    it("throws when no card", async () => {
      const eventRepository = getEventRepository(withAllPlayersPlayed());
      const service = new GameService(eventRepository);
      const event = new EndRoundEvent(
        gameId,
        adminId,
        CardIdRandomizer.random(),
      );

      await expect(service.endRound(event)).rejects.toThrow(NoCardError);
    });

    it("throws when inactive", async () => {
      const eventRepository = getEventRepository(withPlayers());
      const service = new GameService(eventRepository);
      const event = new EndRoundEvent(gameId, adminId, cards.items[0].id);

      await expect(service.endRound(event)).rejects.toThrow(GameInactiveError);
    });

    it("throws when not all players played", async () => {
      const eventRepository = getEventRepository(withSomePlayersPlayed());
      const service = new GameService(eventRepository);
      const event = new EndRoundEvent(gameId, adminId, cards.items[0].id);

      await expect(service.endRound(event)).rejects.toThrow(
        NotAllPlayersPlayedError,
      );
    });
  });
});
