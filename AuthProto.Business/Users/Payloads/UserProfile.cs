using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Permissions;
using AuthProto.Model.Users;
using AuthProto.Shared.Mapping;

namespace AuthProto.Business.Users.Payloads
{
    public class UserProfile : IProfile<User, UserProfile>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CreatorId { get; set; }
        public RoleEnum Role { get; set; }

        public static UserProfile MapFrom(User t)
            => PropMapper<User, UserProfile>.From(t);
    }
}
