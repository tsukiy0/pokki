import { BaseError } from "@tsukiy0/tscore";
import {
  AddPlayerEvent,
  EndRoundEvent,
  NewGameEvent,
  NewRoundEvent,
  PlayCardEvent,
} from "./Event";
import { Game, GameStatus } from "./Game";
import { PlayerCard } from "./PlayerCard";
import { PlayerCardSet } from "./PlayerCardSet";
import { PlayerRole } from "./PlayerRole";
import { PlayerRoleSet } from "./PlayerRoleSet";
import { Role } from "./Role";
import { Round } from "./Round";

export class NoNewError extends BaseError {}
export class GameActiveError extends BaseError {}
export class GameInactiveError extends BaseError {}
export class NoCardError extends BaseError {}
export class NoPlayerError extends BaseError {}
export class NotAllPlayersPlayedError extends BaseError {}
export class UnsupportedEventError extends BaseError {}

export class GameService {
  private eventsToGame(events: Event[]): Game {
    const [newGameEvent, ...restOfEvents] = events;

    if (!(newGameEvent instanceof NewGameEvent)) {
      throw new NoNewError();
    }

    return restOfEvents.reduce((game, event) => {
      if (event instanceof AddPlayerEvent) {
        if (game.status === GameStatus.ACTIVE) {
          throw new GameActiveError();
        }

        return new Game(
          event.gameId,
          GameStatus.INACTIVE,
          game.cards,
          new PlayerRoleSet([
            ...game.players.items,
            new PlayerRole(event.playerId, Role.PLAYER),
          ]),
        );
      }

      if (event instanceof NewRoundEvent) {
        if (game.status === GameStatus.ACTIVE) {
          throw new GameActiveError();
        }

        return new Game(
          event.gameId,
          GameStatus.ACTIVE,
          game.cards,
          game.players,
          new Round(event.roundId, event.roundName, new PlayerCardSet([])),
        );
      }

      if (event instanceof PlayCardEvent) {
        if (game.status === GameStatus.INACTIVE || !game.round) {
          throw new GameInactiveError();
        }

        if (game.cards.hasCard(event.cardId)) {
          throw new NoCardError();
        }

        if (game.players.hasPlayer(event.playerId)) {
          throw new NoPlayerError();
        }

        return new Game(
          event.gameId,
          GameStatus.INACTIVE,
          game.cards,
          game.players,
          new Round(
            game.round.id,
            game.round.name,
            new PlayerCardSet([
              ...game.round.playerCards.items,
              new PlayerCard(event.playerId, event.cardId),
            ]),
          ),
        );
      }

      if (event instanceof EndRoundEvent) {
        if (game.status === GameStatus.INACTIVE || !game.round) {
          throw new GameInactiveError();
        }

        const hasAllPlayersPlayed =
          game.players.items.length === game.round.playerCards.items.length;

        if (!hasAllPlayersPlayed) {
          throw new NotAllPlayersPlayedError();
        }

        return new Game(
          event.gameId,
          GameStatus.INACTIVE,
          game.cards,
          game.players,
        );
      }

      throw new UnsupportedEventError();
    }, new Game(newGameEvent.gameId, GameStatus.INACTIVE, newGameEvent.cards, new PlayerRoleSet([new PlayerRole(newGameEvent.playerId, Role.ADMIN)])));
  }
}
