using AuthProto.Business.Drones.Payloads;
using AuthProto.Model.Db;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;

namespace AuthProto.Business.Drones
{
    internal interface IDroneQueryHandler
    {
        Task<R<List<DroneProfile>>> GetDronesAsync(Guid projectId);
        Task<R<DroneProfile>> GetDroneAsync(Guid droneId);
    }

    [ScopedRegistration]
    internal class DroneQueryHandler : IDroneQueryHandler
    {
        readonly DatabaseContext _dbContext;

        public DroneQueryHandler(
            IDatabaseContext dbContext)
        {
            _dbContext = dbContext.Context;
        }

        public async Task<R<DroneProfile>> GetDroneAsync(Guid droneId)
        {
            var drone = await _dbContext.DroneCollection.Get(x => x.Id == droneId);
            if (drone == null)
                return ("63b18fae-0848-4769-9bf1-160ed71c62aa", "Дрон не найден.");

            return DroneProfile.MapFrom(drone);
        }

        public async Task<R<List<DroneProfile>>> GetDronesAsync(Guid projectId)
        {
            var drones = await _dbContext.DroneCollection
                .Query(x => x.ProjectId == projectId);

            return drones.Select(x => DroneProfile.MapFrom(x)).ToList();
        }
    }
}
