using AuthProto.Business.General.Repository;
using AuthProto.Business.Users.Payloads;
using AuthProto.Model.Users;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;

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
