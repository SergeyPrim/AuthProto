using ASK.MongoService;
using AuthProto.Model.Db.Collections;
using AuthProto.Shared.DI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthProto.Model.Db
{
    public interface IDatabaseContext
    {
        DatabaseContext Context { get; }
    }

    [SingletonRegistration]
    public class DatabaseContext : MongoService, IDatabaseContext
    {
        public UserCollection UserCollection { get; }
        public RolePermissionsCollection RolePermissionsCollection { get; }
        public DroneCollection DroneCollection { get; }

        public DatabaseContext(
            IOptions<MongoDBSettings> op,
            ILogger<MongoService> logger)
            : base(op, logger)
        {
            UserCollection = new UserCollection(this);
            RolePermissionsCollection = new RolePermissionsCollection(this);
            DroneCollection = new DroneCollection(this);
        }

        public DatabaseContext Context { get { return this; } }
    }
}
