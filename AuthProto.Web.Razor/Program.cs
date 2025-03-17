using AuthProto.Business;
using AuthProto.Business.Infra;
using AuthProto.Business.Settings;
using AuthProto.Shared.DI;
using AuthProto.Web.Razor.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace AuthProto.Web.Razor
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

            builder.Services.AddRazorPages();
            builder.Services.AddAuthorization();

            builder.Services.AddSignalR();

            var jwtSecurityConfig = builder.Configuration.GetSection(new JwtSecuritySettings().ConfigurationSectionName);

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = jwtSecurityConfig.GetValue<string>(nameof(JwtSecuritySettings.Audience)),
                        ValidIssuer = jwtSecurityConfig.GetValue<string>(nameof(JwtSecuritySettings.Issuer)),
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSecurityConfig.GetValue<string>(nameof(JwtSecuritySettings.JwtKey))))
                    };
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<JwtHeaderMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.MapHub<DroneHub>("/droneHub");

            app.Run();
        }
    }
}
