using AuthProto.Business.Projects;
using AuthProto.Business.Projects.Payloads;
using AuthProto.Web.Razor.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthProto.Web.Razor.Pages.Projects
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IndexModel : PageModel
    {
        public List<ProjectProfile> Projects { get; set; } = new List<ProjectProfile>();

        readonly IProjectProvider _projectProvider;

        public IndexModel(IProjectProvider projectProvider)
        {
            _projectProvider = projectProvider;
        }

        public async Task OnGetAsync()
        {
            var projects = await _projectProvider.AdminGetProjectsAsync(HttpContext.GetJwtUserGuid());
            if (projects.IsFailure)
            {
                ModelState.AddModelError(String.Empty, projects.Failures[0].ToString());
                return;
            }

            Projects = projects.Value.Items.ToList();
        }
    }
}
