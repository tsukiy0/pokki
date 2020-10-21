using System;
using System.Threading.Tasks;

namespace Api
{
    public class Handler<TRequest, TResponse>
    {
        private readonly Func<string, TRequest> deserializeRequest;
        private readonly Func<TResponse, string> serializeReponse;
        private readonly Func<TRequest, Task<TResponse>> innerHandler;

        public Handler(Func<string, TRequest> deserializeRequest, Func<TResponse, string> serializeReponse, Func<TRequest, Task<TResponse>> innerHandler)
        {
            this.deserializeRequest = deserializeRequest;
            this.serializeReponse = serializeReponse;
            this.innerHandler = innerHandler;
        }

        public async Task<string> Run(string requestString)
        {
            var request = deserializeRequest(requestString);

            var response = await innerHandler(request);

            return serializeReponse(response);
        }
    }
}
