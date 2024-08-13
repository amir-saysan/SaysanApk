using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Polly;
using Quartz;
using SaysanPwa.Api.Modules;
using SaysanPwa.Api.Quartz;
using SaysanPwa.Application;
using SaysanPwa.Domain.CommonExceptions;
using SaysanPwa.Infrastructure;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace SaysanPwa.Api.Configurations;

public abstract class ApiApplicationBootstrapper : ApplicationBootstrapper
{
    Assembly[] _assemblies;

    public ApiApplicationBootstrapper()
    {
        _assemblies = GetAssemblies();
    }


    public virtual Assembly[] GetAssemblies() =>
        Assembly
        .GetExecutingAssembly()
        .GetReferencedAssemblies()
        .Where(asm => asm.Name!.ToLower().Contains("saysanpwa"))
        .Select(asm => Assembly.Load(asm))
        .Concat(new[] {GetType().Assembly, typeof(InfrastructureApplicationReference).Assembly, typeof(ApplicationReference).Assembly})
        .Distinct()
        .ToArray();

    public override void ConfigureService(WebApplicationBuilder builder)
    {
        base.ConfigureService(builder);

        LogManager.Setup()
            .LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "Logging", "NLog", "NLog.config"));

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(cfg =>
            {
                cfg.RegisterAssemblyModules(_assemblies);
            });

        var services = builder.Services;

        AddEndpointsApiExplorer(services);

        AddSwaggerGen(services);

        AddControllers(services);

        ConfigureApiControllerBehavior(services);

        AddRazorPages(services);

        AddCors(services);

        AddCookieAuthentication(services);

        AddAutoMapper(services);

        AddMediatR(services);

        AddQuartz(services);

        AddDistributedMemoryCache(services);

        AddResiliencePipelineProvider(services);

        AddInMomoryRateLimiting(services);

        ConfigureRateLimiting(services);

        AddResponseCache(services);

        AddSessions(services);
    }

    public override async Task Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwaggerUI();
            app.UseSwagger();
        }

        app.UseSession();
        app.UseIpRateLimiting();
        HandleExceptions(app);

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseResponseCaching();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapDefaultControllerRoute();
        app.MapRazorPages();

        await ScheduleJobs(app);

        app.Use(async (context, next) =>
        {
            if (context.User.Identity.IsAuthenticated)
            {
                string sessionId = context.User.FindFirstValue("S_ID");
                if (SessionManager.IsSessionExist(sessionId))
                {
                    await next();
                }
                else
                {
                    await context.SignOutAsync();
                    context.Response.Redirect("/");
                }
            }
            else
            {
                await next();
            }

        });

        await base.Configure(app, env);
    }


    #region Services

    public virtual IServiceCollection AddEndpointsApiExplorer(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        return services;
    }

    public virtual IServiceCollection AddSwaggerGen(IServiceCollection services)
    {
        services.AddSwaggerGen();
        return services;
    }

    public virtual IServiceCollection AddCookieAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme ,cfg =>
            {
                cfg.LoginPath = "/Login";
                cfg.LogoutPath = "/Logout";
                cfg.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });

        return services;
    }

    public virtual IServiceCollection AddControllers(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.WriteIndented = true;
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .AddMvcOptions(opt =>
            {
                opt.RespectBrowserAcceptHeader = true;
                opt.ReturnHttpNotAcceptable = true;
            })
            .AddXmlSerializerFormatters();
        return services;
    }

    public virtual IServiceCollection ConfigureApiControllerBehavior(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(cfg =>
        {
            cfg.SuppressModelStateInvalidFilter = true;
        });
        return services;
    }

    public virtual IServiceCollection AddRazorPages(IServiceCollection services)
    {
        services.AddRazorPages(options =>
        {
            options.Conventions.AuthorizeFolder("/");
        });
        return services;
    }

    public virtual IServiceCollection AddCors(IServiceCollection services)
    {
        services.AddCors(cfg =>
        {
            cfg.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:5170", "https://localhost:7221", "http://localhost:49765", "http://127.0.0.1:5500");
            });
        });
        return services;
    }

    public virtual IServiceCollection AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(_assemblies);
        return services;
    }

    public virtual IServiceCollection AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(_assemblies);
        });

        return services;
    }

    public virtual IServiceCollection AddQuartz(IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(cfg =>
        {
            cfg.WaitForJobsToComplete = true;
        });
        return services;
    }

    public virtual IServiceCollection AddDistributedMemoryCache(IServiceCollection services)
    {
        services.AddDistributedMemoryCache(cfg =>
        {
            cfg.ExpirationScanFrequency = TimeSpan.FromSeconds(10);
        });
        return services;
    }

    public virtual IServiceCollection AddSessions(IServiceCollection services)
    {
        services.AddSession(cfg =>
        {
            cfg.IdleTimeout = TimeSpan.FromMinutes(20);
        });
        return services;
    }

    public virtual IServiceCollection AddResiliencePipelineProvider(IServiceCollection services)
    {
        services.AddResiliencePipeline<string>(Configuration["ResiliencePipelineKey"]!, cfg =>
        {
            cfg.AddRetry(new()
            {
                Delay = TimeSpan.FromSeconds(3),
                MaxDelay = TimeSpan.FromMinutes(2),
                MaxRetryAttempts = 5,
                OnRetry = (token) =>
                {
                    Console.WriteLine($"Retry attampt: \"{token.AttemptNumber}\"");
                    return default;
                }
            })
            .AddTimeout(TimeSpan.FromMinutes(5));
        });
        return services;
    }

    public virtual IServiceCollection AddInMomoryRateLimiting(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddInMemoryRateLimiting();
        return services;
    }

    public virtual IServiceCollection ConfigureRateLimiting(IServiceCollection services)
    {
        services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
        return services;
    }

    public virtual IServiceCollection AddResponseCache(IServiceCollection services)
    {
        services.AddResponseCaching();
        return services;
    }


    #endregion


    #region Configures
    public void HandleExceptions(WebApplication app)
    {
        app.UseExceptionHandler(cfg =>
        {
            cfg.Run(async (context) =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandlerFeature != null)
                {
                    switch (exceptionHandlerFeature.Error)
                    {
                        case AccessDeniedException accessDeniedException:
                            {
                                await context.Response.WriteAsJsonAsync(
                                    new ExceptionHandling.ApplicationException(StatusCodes.Status403Forbidden, new() { accessDeniedException.Message }));
                                break;
                            }
                            case ItemNotFoundException itemNotFoundException:
                            {
                                await context.Response.WriteAsJsonAsync(
                                    new ExceptionHandling.ApplicationException(StatusCodes.Status404NotFound, new() { itemNotFoundException.Message }));
                                break;
                            }
                        case UnProcessableEntityException unProcessableEntityException:
                            {
                                await context.Response.WriteAsJsonAsync(
                                   new ExceptionHandling.ApplicationException(StatusCodes.Status422UnprocessableEntity, new() { unProcessableEntityException.Message }));
                                break;
                            }
                        default:
                            {
                                await context.Response.WriteAsJsonAsync(
                                    new ExceptionHandling.ApplicationException(StatusCodes.Status500InternalServerError, new() { "مشگلی در سرور پیش آمده است." }));
                                break;
                            }
                    };
                }
            });
        });
    }

    public async Task ScheduleJobs(WebApplication app)
    {
        var services = app.Services.CreateAsyncScope().ServiceProvider;
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

        var schedulerFactory = services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        await scheduler.ScheduleJobs(QuartzJobs.Jobs, true, cancellationToken);
    }

    #endregion
}