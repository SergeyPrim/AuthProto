using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthProto.Web.Razor.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            Response.Cookies.Delete("AuthProto");
        }
    }
}
