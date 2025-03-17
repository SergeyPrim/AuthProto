using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Drones;
using AuthProto.Shared.Constants;
using AuthProto.Shared.Mapping;
using AuthProto.Shared.Payloads;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AuthProto.Business.Drones.Payloads
{
    public class CreateDroneRequest : IRequest<CreateDroneRequest, Drone>
    {
        public string Name { get; set; }
        internal Guid ProjectId { get; set; }
        internal Guid CreatorId { get; set; }

        public R<N> SanitizeAndValidate()
        {
            Name = Name?.Trim();
            if (string.IsNullOrEmpty(Name))
                return ("bea239a2-a450-43ab-9c4b-aea9f3f37e77", "Укажите название дрона.");

            if (Name.Length > Metadata.NameMaxLenght)
                return ("90225708-b89f-40bb-8d53-fa7264d60c87", $"Название дрона не может быть длинее {Metadata.NameMaxLenght} символов.");

            return nothing.Nothing;
        }

        Drone IRequest<CreateDroneRequest, Drone>.MapTo()
            => PropMapper<CreateDroneRequest, Drone>.From(this);
    }
}
