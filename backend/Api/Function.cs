using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Infrastructure.Config;
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

        public class GraphQlRequestInfo
        {
            [JsonPropertyName("fieldName")]
            public string FieldName { get; set; }

            [JsonPropertyName("parentTypeName")]
            public GraphQlRequestParentType ParentType { get; set; }
        }

        public class GraphQlArguments
        {
            [JsonPropertyName("request")]
            public JsonElement Request { get; set; }
        }

        public class GraphQlRequest
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

            var handlerMap = new Dictionary<GraphQlRequestInfo, IHandler>
            {
                { new GraphQlRequestInfo { ParentType = GraphQlRequestParentType.Mutation, FieldName = "CreateUser" }, new CreateUserHandler(userRepository) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Query, FieldName = "GetUser" }, new GetUserHandler(userRepository) },
                { new GraphQlRequestInfo{ ParentType = GraphQlRequestParentType.Query, FieldName = "HealthCheck" }, new HealthCheckHandler() }
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
