using AuthProto.Business.Users;
using AuthProto.Business.Users.Payloads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthProto.Web.Razor.Pages.Auth
{
    public class LoginModel : PageModel
    {
        readonly IUserProvider _userProvider;

        public LoginModel(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        [BindProperty]
        public SignInRequest Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _userProvider.SignInAsync(Input);
                if (signInResult.IsFailure)
                {
                    ModelState.AddModelError(string.Empty, signInResult.Failures[0].ToString());
                }
                else
                {
                    Response.Cookies.Append(
                        "AuthProto",
                        signInResult.Value.Token,
                        new CookieOptions
                        {
                            MaxAge = TimeSpan.FromDays(14)
                        });

                    return RedirectToPage("../index");
                }
            }
            return Page();
        }
    }
}
