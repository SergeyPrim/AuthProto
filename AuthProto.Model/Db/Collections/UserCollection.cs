using ASK.MongoService;
using AuthProto.Model.Users;

namespace AuthProto.Model.Db.Collections
{
    public class UserCollection : MongoBaseService<User>
    {
        public UserCollection(MongoService mongo) : base(mongo, typeof(User).Name)
        {
        }
    }
}
