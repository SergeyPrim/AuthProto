using AuthProto.Model.General;
using AuthProto.Model.Permissions;
using AuthProto.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace AuthProto.Model.Users
{
    public class User : Entity, IEntity
    {
        [MaxLength(Metadata.MaxEmailLength)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public RoleEnum Role { get; set; }
        public List<PermissionEnum> Permissions { get; set; }
    }
}
