using ASK.MongoService;
using AuthProto.Model.Drones;

namespace AuthProto.Model.Db.Collections
{
    public class DroneCollection : MongoBaseService<Drone>
    {
        public DroneCollection(MongoService mongo) : base(mongo, typeof(Drone).Name)
        {
        }
    }
}
