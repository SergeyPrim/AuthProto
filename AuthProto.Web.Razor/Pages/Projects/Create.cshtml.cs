using AuthProto.Business.Projects;
using AuthProto.Business.Projects.Payloads;
using AuthProto.Web.Razor.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quartz.Impl.AdoJobStore.Common;

namespace AuthProto.Web.Razor.Pages.Projects
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateProjectRequest Input { get; set; } = new();

        readonly IProjectProvider _projectProvider;

        public CreateModel(IProjectProvider projectProvider)
        {
            _projectProvider = projectProvider;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var creationResult = await _projectProvider.AdminCreateProjectAsync(HttpContext.GetJwtUserGuid(), Input);

                if (creationResult.IsFailure)
                {
                    ModelState.AddModelError(String.Empty, $"{creationResult.Failures[0].Id} : {creationResult.Failures[0].Description}");
                    return Page();
                }

                return LocalRedirect("/Projects/Index");
            }

            return Page();
        }
    }
}
