using AuthProto.Business.Drones.Payloads;
using AuthProto.Business.General.Repository;
using AuthProto.Model.Drones;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;

namespace AuthProto.Business.Drones
{
    internal interface IDroneCommandHandler
    {
        Task<R<DroneProfile>> CreateDroneAsync(Guid userId, Guid projectId, CreateDroneRequest request);
    }

    [ScopedRegistration]
    internal class DroneCommandHandler : IDroneCommandHandler
    {
        readonly IGenericRepository _genericRepository;

        public DroneCommandHandler(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<R<DroneProfile>> CreateDroneAsync(Guid userId, Guid projectId, CreateDroneRequest request)
        {
            request.ProjectId = projectId;
            request.CreatorId = userId;

            return await _genericRepository.AddAsync<Drone, CreateDroneRequest, DroneProfile>(request);
        }
    }
}
