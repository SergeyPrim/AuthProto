using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Permissions;
using AuthProto.Shared.Constants;
using AuthProto.Shared.Payloads;
using AuthProto.Shared.Validation;

namespace AuthProto.Business.Users.Payloads
{
    public class CreateUserRequest : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }

        public R<N> SanitizeAndValidate()
        {
            Email = Email.Trim()?.ToLower();
            Password = Password?.Trim();

            if (!Email.IsEmailAddressValid()) return "Укажите корректный [Email].";

            if (string.IsNullOrEmpty(Password)) return "Необходимо указать [Пароль].";

            if (Password.Length < 8 && Password.Length > Metadata.MaxEmailLength) return $"Пароль должен быть не короче 8 и не длиннее {Metadata.MaxEmailLength} символов.";

            if (!Enum.IsDefined(Role)) return "Укажите роль пользователя.";

            return nothing.Nothing;
        }
    }
}
