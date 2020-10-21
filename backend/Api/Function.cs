using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace Api
{
    public class Function
    {
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

        public class GraphQlRequest
        {
            [JsonPropertyName("info")]
            public GraphQlRequestInfo Info { get; set; }
        }

        public class HandlerNotFoundException : Exception { }

        public async Task<Stream> FunctionHandler(Stream inStream, ILambdaContext context)
        {
            var input = new StreamReader(inStream).ReadToEnd();
            Console.WriteLine(input);
            var graphQlRequest = JsonSerializer.Deserialize<GraphQlRequest>(input);

            var handlerMap = new Dictionary<GraphQlRequestInfo, Func<string, Task<string>>> { };

            if (!handlerMap.TryGetValue(graphQlRequest.Info, out var p))
            {
                throw new HandlerNotFoundException();
            };

            var output = await p.Invoke(input);

            return new MemoryStream(Encoding.UTF8.GetBytes(output));
        }
    }
}
