using AuthProto.Business.General.Interfaces;
using AuthProto.Model.Db;
using AuthProto.Model.General;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace AuthProto.Business.General.Repository
{
    internal interface IGenericRepository
    {
        Task<R<TOutProfile>> AddAsync<TEntity, TRequest, TOutProfile>(TRequest request)
            where TEntity : class, IEntity
            where TRequest : class, IRequest<TRequest, TEntity>
            where TOutProfile : class, IProfile<TEntity, TOutProfile>;

        Task<R<ListResult<TOutProfile>>> GetManyAsync<TEntity, TOutProfile>(
            Expression<Func<TEntity, bool>> filter,
            int skip = 0,
            int take = 100000)
            where TOutProfile : class, IProfile<TEntity, TOutProfile>
            where TEntity : class, IEntity;
    }

    [TransientRegistration]
    internal class GenericRepository : IGenericRepository
    {
        readonly DatabaseContext _dbContext;

        public GenericRepository(IDatabaseContext dbContext)
        {
            _dbContext = dbContext.Context;
        }

        public async Task<R<TOutProfile>> AddAsync<TEntity, TRequest, TOutProfile>(TRequest request)
            where TEntity : class, IEntity
            where TRequest : class, IRequest<TRequest, TEntity>
            where TOutProfile : class, IProfile<TEntity, TOutProfile>
        {
            var validation = request.SanitizeAndValidate();
            if (validation.IsFailure) return validation.Failures;

            var item = request.MapTo();

            await _dbContext.Database.GetCollection<TEntity>(typeof(TEntity).Name).InsertOneAsync(item);

            return TOutProfile.MapFrom(item);
        }

        public async Task<R<ListResult<TOutProfile>>> GetManyAsync<TEntity, TOutProfile>(
            Expression<Func<TEntity, bool>> filter,
            int skip = 0,
            int take = 10)
            where TOutProfile : class, IProfile<TEntity, TOutProfile>
            where TEntity : class, IEntity
        {
            var result = new ListResult<TOutProfile>();

            result.Count = _dbContext.Database
                .GetCollection<TEntity>(typeof(TEntity).Name)
                .AsQueryable<TEntity>()
                .Where(filter).Count();

            var items = _dbContext.Database
                .GetCollection<TEntity>(typeof(TEntity).Name)
                .AsQueryable<TEntity>()
                .Where(filter)
                .Skip(skip)
                .Take(take)
                .ToList();

            result.Items = items
                .Select(x => TOutProfile.MapFrom(x))
                .ToList();

            return result;
        }

    }
}
