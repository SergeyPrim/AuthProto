using AuthProto.Model.Db;
using AuthProto.Model.General;
using AuthProto.Model.Permissions;
using AuthProto.Model.Users;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;
using MongoDB.Driver;

namespace AuthProto.Business.Permissions
{
    public interface IPermissionProvider
    {
        Task<R<User>> GetUserIfPermissionAllowedAsync(Guid userId, PermissionEnum permission);
        Task<R<(User user, TOut entity)>> GetUserAndEntityIfPermissionAllowedAsync<TOut>(Guid userId, Guid entityId, PermissionEnum permission)
            where TOut : class, IEntity;
    }

    [ScopedRegistration]
    internal class PermissionProvider : IPermissionProvider
    {
        readonly DatabaseContext _dbContext;

        public PermissionProvider(IDatabaseContext dbContext)
        {
            _dbContext = dbContext.Context;
        }

        public async Task<R<User>> GetUserIfPermissionAllowedAsync(Guid userId, PermissionEnum permission)
        {
            var user = await _dbContext.UserCollection.Get(x => x.Id == userId);
            if (user is null)
                return ("ea0ba53c-ed9a-4a5b-9104-5a70c5af2ce8", "Пользователь не найден.");

            if (user.Role == RoleEnum.SystemAdmin)
                return user;

            var role = await _dbContext.RolePermissionsCollection.Get(x => x.Role == user.Role);
            if (role is null)
                return ("16cf970a-9ddc-44e2-b463-d64bdf5a3cb4", "Пользователь не имеет роли.");

            if (role.Permissions.Contains(permission))
                return user;

            if (user.Permissions.Contains(permission))
                return user;

            return ("02fd1768-4cc7-4226-92de-672db39c7154", "Отказано в доступе.");
        }

        public async Task<R<(User user, TOut entity)>> GetUserAndEntityIfPermissionAllowedAsync<TOut>(Guid userId, Guid entityId, PermissionEnum permission)
            where TOut : class, IEntity
        {
            var user = await _dbContext.UserCollection.Get(x => x.Id == userId);
            if (user is null)
                return ("ea0ba53c-ed9a-4a5b-9104-5a70c5af2ce8", "Пользователь не найден.");

            var entity = await _dbContext.Database
                .GetCollection<TOut>(typeof(TOut).Name)
                .Find(x => x.Id == entityId)
                .FirstOrDefaultAsync();

            if (entity is null || entity.ProjectId != user.ProjectId)
                return ("c6e5a0d3-b3fa-40c6-93aa-793f07623f1a", "Объект не найден.");

            if (user.Role == RoleEnum.SystemAdmin)
                return (user, entity);

            var role = await _dbContext.RolePermissionsCollection.Get(x => x.Role == user.Role);
            if (role is null)
                return ("16cf970a-9ddc-44e2-b463-d64bdf5a3cb4", "Пользователь не имеет роли.");

            if (role.Permissions.Contains(permission))
                return (user, entity);

            if (user.Permissions.Contains(permission))
                return (user, entity);

            return ("02fd1768-4cc7-4226-92de-672db39c7154", "Отказано в доступе.");
        }
    }
}
