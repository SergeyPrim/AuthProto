using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Drones;
using AuthProto.Shared.Mapping;

namespace AuthProto.Business.Drones.Payloads
{
    public class DroneProfile : IProfile<Drone, DroneProfile>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public static DroneProfile MapFrom(Drone t)
            => PropMapper<Drone, DroneProfile>.From(t);
    }
}
