using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Projects;
using AuthProto.Shared.Constants;
using AuthProto.Shared.Mapping;
using AuthProto.Shared.Payloads;

namespace AuthProto.Business.Projects.Payloads
{
    public class CreateProjectRequest : IRequest<CreateProjectRequest, Project>
    {
        public string Name { get; set; }
        internal Guid CreatorId { get; set; }

        public R<N> SanitizeAndValidate()
        {
            Name = Name?.Trim();
            if (string.IsNullOrEmpty(Name))
                return ("fe201e64-ed93-40f1-aa5b-31f745c9b480", "Укажите название проекта.");

            if (Name.Length > Metadata.NameMaxLenght)
                return ("90225708-b89f-40bb-8d53-fa7264d60c87", $"Название проекта не может быть длинее {Metadata.NameMaxLenght} символов.");

            return nothing.Nothing;
        }

        Project IRequest<CreateProjectRequest, Project>.MapTo()
            => PropMapper<CreateProjectRequest, Project>.From(this);
    }
}
