﻿using AuthProto.Business.General.Repository;
using AuthProto.Business.Projects.Payloads;
using AuthProto.Model.Projects;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;

namespace AuthProto.Business.Projects
{
    internal interface IProjectCommandHandler
    {
        Task<R<ProjectProfile>> CreateProjectAsync(CreateProjectRequest request);
    }

    [ScopedRegistration]
    internal class ProjectCommandHandler : IProjectCommandHandler
    {
        readonly IGenericRepository _genericRepository;

        public ProjectCommandHandler(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<R<ProjectProfile>> CreateProjectAsync(CreateProjectRequest request)
            => await _genericRepository.AddAsync<Project, CreateProjectRequest, ProjectProfile>(request);
    }
}
