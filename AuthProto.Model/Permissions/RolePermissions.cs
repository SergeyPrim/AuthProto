using AuthProto.Model.General;

namespace AuthProto.Model.Permissions
{
    public class RolePermissions : Entity, IEntity
    {
        public RoleEnum Role { get; set; }
        public List<PermissionEnum> Permissions { get; set; }
    }
}
