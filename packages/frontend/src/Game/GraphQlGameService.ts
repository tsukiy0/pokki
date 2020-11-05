import {
  AddPlayerEvent,
  AddPlayerEventSerializer,
  EndRoundEvent,
  EndRoundEventSerializer,
  Game,
  GameJson,
  GameSerializer,
  GetGameRequest,
  GetGameRequestSerializer,
  NewGameEvent,
  NewGameEventSerializer,
  NewRoundEvent,
  NewRoundEventSerializer,
  PlayCardEvent,
  PlayCardEventSerializer,
} from "@pokki/core";
import AWSAppSyncClient from "aws-appsync";
import {
  AddPlayer,
  AddPlayerMutation,
  AddPlayerMutationVariables,
  EndRound,
  EndRoundMutation,
  EndRoundMutationVariables,
  GetGame,
  GetGameQuery,
  GetGameQueryVariables,
  NewGame,
  NewGameMutation,
  NewGameMutationVariables,
  NewRound,
  NewRoundMutation,
  NewRoundMutationVariables,
  OnGame,
  OnGameSubscription,
  OnGameSubscriptionVariables,
  PlayCard,
  PlayCardMutation,
  PlayCardMutationVariables,
} from "../generated/graphql";
import { OnGameRequest, OnGameRequestSerializer } from "./OnGameRequest";

export class GraphQlGameService {
  constructor(private readonly client: AWSAppSyncClient<any>) {}

  async newGame(request: NewGameEvent): Promise<Game> {
    const res = await this.client.mutate<
      NewGameMutation,
      NewGameMutationVariables
    >({
      mutation: NewGame,
      variables: NewGameEventSerializer.serialize(request),
    });

    return GameSerializer.deserialize(res.data?.NewGame as GameJson);
  }

  async addPlayer(request: AddPlayerEvent): Promise<Game> {
    const res = await this.client.mutate<
      AddPlayerMutation,
      AddPlayerMutationVariables
    >({
      mutation: AddPlayer,
      variables: AddPlayerEventSerializer.serialize(request),
    });

    return GameSerializer.deserialize(res.data?.AddPlayer as GameJson);
  }

  async newRound(request: NewRoundEvent): Promise<Game> {
    const res = await this.client.mutate<
      NewRoundMutation,
      NewRoundMutationVariables
    >({
      mutation: NewRound,
      variables: NewRoundEventSerializer.serialize(request),
    });

    return GameSerializer.deserialize(res.data?.NewRound as GameJson);
  }

  async playCard(request: PlayCardEvent): Promise<Game> {
    const res = await this.client.mutate<
      PlayCardMutation,
      PlayCardMutationVariables
    >({
      mutation: PlayCard,
      variables: PlayCardEventSerializer.serialize(request),
    });

    return GameSerializer.deserialize(res.data?.PlayCard as GameJson);
  }

  async endRound(request: EndRoundEvent): Promise<Game> {
    const res = await this.client.mutate<
      EndRoundMutation,
      EndRoundMutationVariables
    >({
      mutation: EndRound,
      variables: EndRoundEventSerializer.serialize(request),
    });

    return GameSerializer.deserialize(res.data?.EndRound as GameJson);
  }

  async getGame(request: GetGameRequest): Promise<Game> {
    const res = await this.client.query<GetGameQuery, GetGameQueryVariables>({
      query: GetGame,
      variables: GetGameRequestSerializer.serialize(request),
    });

    return GameSerializer.deserialize(res.data?.GetGame as GameJson);
  }

  onGame(
    request: OnGameRequest,
    callback: (response: Game) => void,
  ): () => void {
    const subscription = this.client
      .subscribe<{ data: OnGameSubscription }, OnGameSubscriptionVariables>({
        query: OnGame,
        variables: OnGameRequestSerializer.serialize(request),
      })
      .subscribe((value) => {
        callback(GameSerializer.deserialize(value.data.OnGame as GameJson));
      });

    return () => subscription.unsubscribe();
  }
}
