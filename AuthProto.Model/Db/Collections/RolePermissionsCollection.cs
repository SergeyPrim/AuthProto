using ASK.MongoService;
using AuthProto.Model.Permissions;

namespace AuthProto.Model.Db.Collections
{
    public class RolePermissionsCollection : MongoBaseService<RolePermissions>
    {
        public RolePermissionsCollection(MongoService mongo) : base(mongo, typeof(RolePermissions).Name)
        {
        }
    }
}
