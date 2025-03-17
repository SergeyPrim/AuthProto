using AuthProto.Business.General.Utilities;
using AuthProto.Business.Permissions;
using AuthProto.Business.Users.Payloads;
using AuthProto.Model.Db;
using AuthProto.Model.Permissions;
using AuthProto.Model.Users;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;
using AuthProto.Shared.Utilities;

namespace AuthProto.Business.Users
{
    public interface IUserProvider
    {
        Task<R<UserProfile>> CreateUserAsync(Guid userId, CreateUserRequest request);
        Task<R<UserProfile>> AdminCreateUserAsync(Guid adminId, Guid projectId, CreateUserRequest request);
        Task<R<SignInUpResponse>> SignInAsync(SignInRequest request);
    }

    [ScopedRegistration]
    internal class UserProvider : IUserProvider
    {
        readonly DatabaseContext _dbContext;
        readonly IPermissionProvider _permissionProvider;
        readonly IJwtService _jwtService;

        public UserProvider(
            IDatabaseContext dbContext,
            IPermissionProvider permissionProvider,
            IJwtService jwtService)
        {
            _dbContext = dbContext.Context;
            _permissionProvider = permissionProvider;
            _jwtService = jwtService;
        }

        public async Task<R<UserProfile>> CreateUserAsync(Guid userId, CreateUserRequest request)
        {
            var allowedUser = await _permissionProvider.GetUserIfPermissionAllowedAsync(userId, PermissionEnum.CreateUser);
            if (allowedUser.IsFailure) return allowedUser.Failures;

            var validation = request.SanitizeAndValidate();
            if (validation.IsFailure) return validation.Failures;

            if (request.Role < allowedUser.Value.Role)
                return ("79913b84-6331-486d-9fd8-e7c38997174b", "Роль создаваемого пользователя не может быть выше роли создателя.");

            var user = new User
            {
                Email = request.Email,
                PasswordHash = HashService.HashPasword(request.Password, out string salt),
                PasswordSalt = salt,
                Role = request.Role,
                CreatorId = allowedUser.Value.Id,
                ProjectId = allowedUser.Value.ProjectId
            };

            await _dbContext.UserCollection.Insert(user);

            return UserProfile.MapFrom(user);
        }

        public async Task<R<UserProfile>> AdminCreateUserAsync(Guid adminId, Guid projectId, CreateUserRequest request)
        {
            var allowedUser = await _permissionProvider.GetUserIfPermissionAllowedAsync(adminId, PermissionEnum.CreateUser);
            if (allowedUser.IsFailure) return allowedUser.Failures;

            var validation = request.SanitizeAndValidate();
            if (validation.IsFailure) return validation.Failures;

            var user = new User
            {
                Email = request.Email,
                PasswordHash = HashService.HashPasword(request.Password, out string salt),
                PasswordSalt = salt,
                Role = request.Role,
                CreatorId = allowedUser.Value.Id,
                ProjectId = projectId
            };

            await _dbContext.UserCollection.Insert(user);

            return UserProfile.MapFrom(user);
        }

        public async Task<R<SignInUpResponse>> SignInAsync(SignInRequest request)
        {
            var validationResult = request.SanitizeAndValidate();
            if (validationResult.IsFailure) return validationResult.Failures;

            var user = await _dbContext.UserCollection.Get(x => x.Email == request.Email);
            if (user is null)
                return ("ea0ba53c-ed9a-4a5b-9104-5a70c5af2ce8", "Пользователь не найден.");

            if (!HashService.CheckPasword(request.Password, user.PasswordHash, user.PasswordSalt))
                return "Неверный email или пароль";

            var token = _jwtService.IssueJwt(new() { UserId = user.Id });

            return new SignInUpResponse(user.Id, token.Token, token.ExpiresInUtc);
        }
    }
}
