using AuthProto.Business.General.Repository;
using AuthProto.Business.Projects.Payloads;
using AuthProto.Model.Projects;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthProto.Business.Projects
{
    internal interface IProjectQueryHandler
    {
        Task<R<ListResult<ProjectProfile>>> GetProjectsAsync(int skip = 0, int take = 10);
    }

    [ScopedRegistration]
    internal class ProjectQueryHandler : IProjectQueryHandler
    {
        readonly IGenericRepository _genericRepository;

        public ProjectQueryHandler(
            IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<R<ListResult<ProjectProfile>>> GetProjectsAsync(int skip = 0, int take = 10)
            => await _genericRepository.GetManyAsync<Project, ProjectProfile>(x => true, skip, take);
    }
}
