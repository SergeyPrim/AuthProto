using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Drones;
using AuthProto.Shared.Payloads;

namespace AuthProto.Business.Drones.Payloads
{
    public class UpdateDroneRequest : IUpdateRequest<UpdateDroneRequest, Drone>
    {
        public Guid Id { get; set; }

        public R<N> SanitizeAndValidate()
        {
            throw new NotImplementedException();
        }

        void IUpdateRequest<UpdateDroneRequest, Drone>.CopyTo(Drone t)
        {
            throw new NotImplementedException();
        }

    }
}
