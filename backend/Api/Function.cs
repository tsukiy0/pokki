using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Api.GameDomain;
using Api.UserDomain;
using Core.GameDomain;
using Infrastructure.Config;
using Infrastructure.GameDomain;
using Infrastructure.UserDomain;

namespace Api
{
    public class Function
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum GraphQlRequestParentType
        {
            Query,
            Mutation,
            Subscription
        }

        public struct GraphQlRequestInfo
        {
            [JsonPropertyName("fieldName")]
            public string FieldName { get; set; }

            [JsonPropertyName("parentTypeName")]
            public GraphQlRequestParentType ParentType { get; set; }
        }

        public struct GraphQlArguments
        {
            [JsonPropertyName("request")]
            public JsonElement Request { get; set; }
        }

        public struct GraphQlRequest
        {
            [JsonPropertyName("info")]
            public GraphQlRequestInfo Info { get; set; }

            [JsonPropertyName("arguments")]
            public GraphQlArguments Arguments { get; set; }
        }

        public class HandlerNotFoundException : Exception { }

        public async Task<Stream> FunctionHandler(Stream stream, ILambdaContext context)
        {
            var input = new StreamReader(stream).ReadToEnd();
            Console.WriteLine(input);
            var graphQlRequest = JsonSerializer.Deserialize<GraphQlRequest>(input);
            var config = new SystemConfig();
            var userRepository = DynamoUserRepository.Default(config.Get("USER_TABLE_NAME"));
            var eventRepository = DynamoEventRepository.Default(config.Get("GAME_TABLE_NAME"));
            var gameService = new GameService(eventRepository);

            var handlerMap = new Dictionary<GraphQlRequestInfo, IHandler>
            {
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Mutation, FieldName = "CreateUser" }, new CreateUserHandler(userRepository) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Query, FieldName = "GetUser" }, new GetUserHandler(userRepository) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Query, FieldName = "HealthCheck" }, new HealthCheckHandler() },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Mutation, FieldName = "New" }, new NewEventHandler(gameService) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Mutation, FieldName = "AddPlayer" }, new AddPlayerEventHandler(gameService) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Mutation, FieldName = "NewRound" }, new NewRoundEventHandler(gameService) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Mutation, FieldName = "SelectCard" }, new SelectCardEventHandler(gameService) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Mutation, FieldName = "EndRound" }, new EndRoundEventHandler(gameService) },
            };

            if (!handlerMap.TryGetValue(graphQlRequest.Info, out var handler))
            {
                throw new HandlerNotFoundException();
            };

            var output = await handler.Run(graphQlRequest.Arguments.Request.ToString());

            return new MemoryStream(Encoding.UTF8.GetBytes(output));
        }
    }
}
