namespace AuthProto.Web.Razor.Extensions
{
    public class JwtHeaderMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var headerTokenExists = context.Request.Headers.TryGetValue("Authorization", out var headerToken);

            if (!headerTokenExists || string.IsNullOrWhiteSpace(headerToken.ToString().Replace("Bearer", "")))
            {
                var cookieTokenExists = context.Request.Cookies.TryGetValue("AuthProto", out var cookieToken);
                if (cookieTokenExists && !string.IsNullOrWhiteSpace(cookieToken))
                {
                    context.Request.Headers["Authorization"] = ValidateHeaderToken(cookieToken);
                }
            }

            await next(context);
        }

        private static string ValidateHeaderToken(string token)
        {
            if (!token.StartsWith("Bearer"))
                token = "Bearer " + token;

            return token;
        }
    }
}
