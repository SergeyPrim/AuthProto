using AuthProto.Business.General.Repository;
using AuthProto.Shared.DI;

namespace AuthProto.Business.Users
{
    internal interface IUserCommandHandler
    {
    }

    [ScopedRegistration]
    internal class UserCommandHandler : IUserCommandHandler
    {
        readonly IGenericRepository _repository;

        public UserCommandHandler(IGenericRepository repository)
        {
            _repository = repository;
        }
    }
}
