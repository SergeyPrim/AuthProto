using AuthProto.Model.Db;
using AuthProto.Model.Permissions;
using AuthProto.Model.Users;
using AuthProto.Shared.Utilities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthProto.Business.Infra
{
    [DisallowConcurrentExecution]
    public class SysadminCreateJob : IJob
    {
        readonly DatabaseContext _dbContext;

        public SysadminCreateJob(IDatabaseContext dbContext)
        {
            _dbContext = dbContext.Context;
        }

        public async Task Execute(IJobExecutionContext context)
            => await CreateSysadminAsync();

        public static void Schedule(IServiceCollectionQuartzConfigurator q)
        {
            q.ScheduleJob<SysadminCreateJob>(j => j.StartNow());
        }

        async Task CreateSysadminAsync()
        {
            var admin = await _dbContext.UserCollection.Get(x => x.Email == "admin@admin.ru");
            if (admin is null)
            {
                var user = new User
                {
                    Email = "admin@admin.ru",
                    PasswordHash = HashService.HashPasword("11111111", out string salt),
                    PasswordSalt = salt,
                    Role = RoleEnum.SystemAdmin
                };

                await _dbContext.UserCollection.Insert(user);
            }
        }
    }
}
