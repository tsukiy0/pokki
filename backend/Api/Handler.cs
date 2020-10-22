using System.Text.Json;
using System.Threading.Tasks;

namespace Api
{
    public interface IHandler
    {
        Task<string> Run(string requestString);
    }

    public abstract class BaseHandler<TRequest, TResponse> : IHandler
    {
        protected abstract Task<TResponse> Handle(TRequest request);

        public async Task<string> Run(string requestString)
        {
            var request = JsonSerializer.Deserialize<TRequest>(requestString);

            var response = await Handle(request);

            return JsonSerializer.Serialize(response);
        }
    }
}
