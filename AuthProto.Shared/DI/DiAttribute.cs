using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.Loader;

namespace AuthProto.Shared.DI
{
    public class ScopedRegistrationAttribute : Attribute { }

    public class SingletonRegistrationAttribute : Attribute { }

    public class TransientRegistrationAttribute : Attribute { }

    public class OptionsRegistrationAttribute : Attribute { }

    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection AddAutowiringDependencies(this IServiceCollection services, IConfiguration configuration, string assemblyNamePrefix = "")
        {
            Type scopedRegistration = typeof(ScopedRegistrationAttribute);
            Type singletonRegistration = typeof(SingletonRegistrationAttribute);
            Type transientRegistration = typeof(TransientRegistrationAttribute);
            Type optionsRegistration = typeof(OptionsRegistrationAttribute);

            var entryAssembly = Assembly.GetEntryAssembly();
            var uriPath = new Uri(entryAssembly.Location).LocalPath;
            var path = Path.GetDirectoryName(uriPath);
            var assemblies = Directory
                .GetFiles(path, assemblyNamePrefix)
                .Where(x => !x.Contains(".Tests."))
                .Select(x => AssemblyLoadContext.Default.LoadFromAssemblyPath(x));

            var types = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p =>
                    p.IsDefined(scopedRegistration, true) ||
                    p.IsDefined(transientRegistration, true) ||
                    p.IsDefined(singletonRegistration, false) ||
                    p.IsDefined(optionsRegistration, true) && !p.IsInterface).Select(s => new
                    {
                        Service = s.GetInterface($"I{s.Name}"),
                        Options = s.GetInterface("IBaseSettings"),
                        Implementation = s
                    }).Where(x => x.Service != null || x.Options != null);

            foreach (var type in types)
            {
                if (type.Implementation.IsDefined(scopedRegistration, false))
                {
                    if (type.Service != null) services.AddScoped(type.Service, type.Implementation);
                }
                else if (type.Implementation.IsDefined(transientRegistration, false))
                {
                    if (type.Service != null) services.AddTransient(type.Service, type.Implementation);
                }
                else if (type.Implementation.IsDefined(singletonRegistration, false))
                {
                    if (type.Service != null) services.AddSingleton(type.Service, type.Implementation);
                }
                else if (type.Implementation.IsDefined(optionsRegistration, false))
                {
                    dynamic instance = Activator.CreateInstance(type.Implementation);
                    var sectionName = (instance as IBaseSettings).ConfigurationSectionName;

                    var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions).GetMethods()
                        .Where(x => x.Name == "Configure")
                        .Single(m => m.GetParameters().Length == 2)
                        .MakeGenericMethod(type.Implementation);

                    configureMethod.Invoke(null, [services, configuration.GetSection(sectionName)]);

                }
            }
            return services;
        }
    }

    public interface IBaseSettings
    {
        string ConfigurationSectionName { get; }
    }
}
