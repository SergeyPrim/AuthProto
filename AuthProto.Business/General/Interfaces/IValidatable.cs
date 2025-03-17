using AuthProto.Shared.Payloads;

namespace AuthProto.Business.General.Interfaces
{
    public interface IValidatable
    {
        R<N> SanitizeAndValidate();
    }
}
