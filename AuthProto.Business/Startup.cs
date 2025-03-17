using ASK.MongoService;
using AuthProto.Business.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AuthProto.Business
{
    public static class Startup
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoService(configuration);

            services.AddQuartz(q =>
            {
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();

                SysadminCreateJob.Schedule(q);
            });
            services.AddQuartzHostedService();

            return services;
        }
    }
}
