import AWSAppSyncClient from 'aws-appsync';
import gql from 'graphql-tag';
import {
  AddPlayerMutation,
  AddPlayerMutationVariables,
  AddPlayerRequest,
  EndRoundMutation,
  EndRoundMutationVariables,
  EndRoundRequest,
  NewGameMutation,
  NewGameMutationVariables,
  NewGameRequest,
  NewRoundMutation,
  NewRoundMutationVariables,
  NewRoundRequest,
  SelectCardMutation,
  SelectCardMutationVariables,
  SelectCardRequest,
} from '../graphql/API';
import {
  addPlayer, endRound, newGame, newRound, selectCard,
} from '../graphql/mutations';

export class GameClient {
  constructor(private readonly client: AWSAppSyncClient<any>) {}

  public async newGame(request: NewGameRequest): Promise<NewGameMutation['NewGame']> {
    const result = await this.client.mutate<NewGameMutation, NewGameMutationVariables>({
      mutation: gql(newGame),
      variables: {
        request,
      },
    });

    return result.data.NewGame;
  }

  public async addPlayer(request: AddPlayerRequest): Promise<AddPlayerMutation['AddPlayer']> {
    const result = await this.client.mutate<AddPlayerMutation, AddPlayerMutationVariables>({
      mutation: gql(addPlayer),
      variables: {
        request,
      },
    });

    return result.data.AddPlayer;
  }

  public async newRound(request: NewRoundRequest): Promise<NewRoundMutation['NewRound']> {
    const result = await this.client.mutate<NewRoundMutation, NewRoundMutationVariables>({
      mutation: gql(newRound),
      variables: {
        request,
      },
    });

    return result.data.NewRound;
  }

  public async selectCard(request: SelectCardRequest): Promise<SelectCardMutation['SelectCard']> {
    const result = await this.client.mutate<SelectCardMutation, SelectCardMutationVariables>({
      mutation: gql(selectCard),
      variables: {
        request,
      },
    });

    return result.data.SelectCard;
  }

  public async endRound(request: EndRoundRequest): Promise<EndRoundMutation['EndRound']> {
    const result = await this.client.mutate<EndRoundMutation, EndRoundMutationVariables>({
      mutation: gql(endRound),
      variables: {
        request,
      },
    });

    return result.data.EndRound;
  }
}
