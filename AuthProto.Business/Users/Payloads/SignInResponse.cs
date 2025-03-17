namespace AuthProto.Business.Users.Payloads
{
    public record SignInUpResponse(Guid UserId, string Token, DateTime ExpiresAtUtc);
}
