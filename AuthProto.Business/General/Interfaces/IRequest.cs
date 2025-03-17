using AuthProto.Model.General;

namespace AuthProto.Business.General.Interfaces
{
    internal interface IRequest : IValidatable { }

    internal interface IRequest<T, TOut> : IRequest
        where T : IRequest
        where TOut : IEntity
    {
        internal TOut MapTo();
    }

    internal interface IUpdateRequest<TRequest, TOut> : IRequest
        where TRequest : IRequest
        where TOut : IEntity
    {
        public Guid Id { get; set; }

        internal void CopyTo(TOut t);
    }
}
