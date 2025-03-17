using AuthProto.Business.Permissions;
using AuthProto.Business.Projects.Payloads;
using AuthProto.Model.Permissions;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;

namespace AuthProto.Business.Projects
{
    public interface IProjectProvider
    {
        Task<R<ListResult<ProjectProfile>>> AdminGetProjectsAsync(Guid adminId, int skip = 0, int take = 10);
        Task<R<ProjectProfile>> AdminCreateProjectAsync(Guid adminId, CreateProjectRequest request);
    }

    [ScopedRegistration]
    internal class ProjectProvider : IProjectProvider
    {
        readonly IProjectCommandHandler _ch;
        readonly IProjectQueryHandler _qh;
        readonly IPermissionProvider _permissionProvider;

        public ProjectProvider(
            IProjectCommandHandler ch, 
            IProjectQueryHandler qh,
            IPermissionProvider permissionProvider)
        {
            _ch = ch;
            _qh = qh;
            _permissionProvider = permissionProvider;
        }

        public async Task<R<ProjectProfile>> AdminCreateProjectAsync(Guid adminId, CreateProjectRequest request)
        {
            var allowedUser = await _permissionProvider.GetUserIfPermissionAllowedAsync(adminId, PermissionEnum.ListProjects);
            if (allowedUser.IsFailure) return allowedUser.Failures;

            request.CreatorId = allowedUser.Value.Id;

            return await _ch.CreateProjectAsync(request);
        }

        public async Task<R<ListResult<ProjectProfile>>> AdminGetProjectsAsync(Guid adminId, int skip = 0, int take = 10)
        {
            var allowedUser = await _permissionProvider.GetUserIfPermissionAllowedAsync(adminId, PermissionEnum.ListProjects);
            if (allowedUser.IsFailure) return allowedUser.Failures;

            return await _qh.GetProjectsAsync(skip, take);
        }

    }
}
