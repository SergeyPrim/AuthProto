using AuthProto.Model.General;
using AuthProto.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace AuthProto.Model.Drones
{
    public class Drone : Entity, IEntity
    {
        [MaxLength(Metadata.NameMaxLenght)]
        public string Name { get; set; }
    }
}
