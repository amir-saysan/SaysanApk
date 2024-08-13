using AspNetCoreRateLimit;
using Autofac;
using SaysanPwa.Api.Logging;
using SaysanPwa.Api.Logging.NLog;
using SaysanPwa.Application;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using SaysanPwa.Domain.SeedWorker;
using SaysanPwa.Infrastructure;
using SaysanPwa.Infrastructure.Repositories;
using SqlServerWrapper.Wrapper.Connection;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Reflection;

namespace SaysanPwa.Api.Modules;

public class AutofacModule : Autofac.Module
{
       
    protected override void Load(ContainerBuilder builder)
    {

        var assemblies = Assembly
        .GetExecutingAssembly()
        .GetReferencedAssemblies()
        .Where(asm => asm.Name!.ToLower().Contains("saysanpwa"))
        .Select(asm => Assembly.Load(asm))
        .Concat(new[] { GetType().Assembly, typeof(InfrastructureApplicationReference).Assembly, typeof(ApplicationReference).Assembly })
        .Distinct()
        .ToArray();

        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), false, true).Build();

        builder.RegisterType<NLogLoggerService>().As<ILoggerService>().SingleInstance();
        builder.RegisterType<RateLimitConfiguration>().As<IRateLimitConfiguration>().SingleInstance();
        //builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IRepository<>)).InstancePerLifetimeScope();

        IConnectionString connectionString = new DefaultConnectionString(configuration.GetConnectionString("Default")!);
        builder.RegisterInstance(connectionString).As<IConnectionString>().SingleInstance();
        builder.RegisterType<DefaultSqlServerConnection>().As<ISqlServerConnection>().InstancePerLifetimeScope();
        builder.RegisterType<DefaultDatabaseManager>().As<IDbManager>().InstancePerLifetimeScope();
    }
}
