using System;
using System.Threading.Tasks;
using Core.UserDomain;

namespace Api
{
    public struct GetUserRequest
    {
        public string Id { get; set; }
    }

    public struct GetUserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class GetUserHandler : BaseHandler<GetUserRequest, GetUserResponse>
    {
        private readonly IUserRepository userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        protected override async Task<GetUserResponse> Handle(GetUserRequest request)
        {
            var user = await userRepository.GetUser(new UserId(Guid.Parse(request.Id)));

            return new GetUserResponse
            {
                Id = user.Id.Value.ToString(),
                Name = user.Name
            };
        }
    }
}