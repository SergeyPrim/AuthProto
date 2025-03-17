using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Projects;
using AuthProto.Shared.Mapping;

namespace AuthProto.Business.Projects.Payloads
{
    public class ProjectProfile : IProfile<Project, ProjectProfile>
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Name { get; set; }

        public static ProjectProfile MapFrom(Project t)
            => PropMapper<Project, ProjectProfile>.From(t);
    }
}
