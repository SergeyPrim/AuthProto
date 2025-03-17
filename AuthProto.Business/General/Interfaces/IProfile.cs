
using AuthProto.Model.General;

namespace AuthProto.Business.General.Interfaces
{
    public interface IProfile
    {
        Guid Id { get; internal set; }
    }

    internal interface IProfile<T, out TOut> : IProfile
        where T : class, IEntity
        where TOut : IProfile
    {
        static abstract TOut MapFrom(T t);
    }
}
