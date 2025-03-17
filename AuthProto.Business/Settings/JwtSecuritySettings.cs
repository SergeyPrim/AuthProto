using AuthProto.Shared.DI;

namespace AuthProto.Business.Settings
{
    [OptionsRegistration]
    public class JwtSecuritySettings : IBaseSettings
    {
        public string ConfigurationSectionName => nameof(JwtSecuritySettings);

        public string JwtKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ValidPeriodInHours { get; set; }
    }
}
