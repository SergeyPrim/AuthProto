using AuthProto.Business;
using AuthProto.Shared.DI;
using Serilog;

namespace AuthProto.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host
                .UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)); ;

            builder.Services.AddAutowiringDependencies(builder.Configuration, "AuthProto.*.dll");
            builder.Services.AddBusiness(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
