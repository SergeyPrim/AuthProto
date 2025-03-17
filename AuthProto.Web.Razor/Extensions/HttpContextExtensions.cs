using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace AuthProto.Web.Razor.Extensions
{
    public static class HttpContextExtensions
    {
        private static string? GetHeaderValueAsString(this HttpContext context, string headerName)
        {
            StringValues values;
            if (!(context.Request?.Headers?.TryGetValue(headerName, out values) ?? false)) return default;

            string rawValues = values.ToString();

            if (!string.IsNullOrWhiteSpace(rawValues))
                return (string)Convert.ChangeType(values.ToString(), typeof(string));

            return null;
        }

        public static string GetRemoteIpAddress(this HttpContext context)
        {
            var ipAddress = GetHeaderValueAsString(context, "CF-Connecting-IP") ?? GetHeaderValueAsString(context, "X-Forwarded-For") ?? context.Connection.RemoteIpAddress.ToString();
            return ipAddress == "::1" ? "127.0.0.1" : ipAddress;
        }


        public static string? GetUserAgent(this HttpContext context) => GetHeaderValueAsString(context, "User-Agent");

        public static Guid GetJwtUserGuid(this HttpContext context)
        {
            if (!context.User!.Identity!.IsAuthenticated)
                return Guid.Empty;

            return Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userGuid) ? userGuid : Guid.Empty;
        }
    }
}
