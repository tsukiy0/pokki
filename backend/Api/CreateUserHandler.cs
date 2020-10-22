using System;
using System.Threading.Tasks;
using Core.UserDomain;

namespace Api
{
    public class CreateUserRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateUserHandler : BaseHandler<CreateUserRequest, VoidResponse>
    {
        private readonly IUserRepository userRepository;

        public CreateUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        protected override async Task<VoidResponse> Handle(CreateUserRequest request)
        {
            await userRepository.CreateUser(
                new User(
                    new UserId(Guid.Parse(request.Id)),
                    request.Name
                )
            );

            return new VoidResponse();
        }
    }

}
