using System.Threading.Tasks;

namespace Api
{
    public struct HealthCheckResponse
    {
        public bool IsHealthy { get; set; }
    }

    public class HealthCheckHandler : BaseHandler<VoidRequest, HealthCheckResponse>
    {
        protected override async Task<HealthCheckResponse> Handle(VoidRequest request)
        {
            return new HealthCheckResponse
            {
                IsHealthy = true
            };
        }
    }
}
