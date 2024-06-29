using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

namespace WebAPI.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Burda Sql Bağlantısını Connection String sayesinde gerçekleştiriyoruz
        services.AddDbContext<RepositoryContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        //AddSingleton ile nesnenin tek bir defa oluşması sağlanmış olur
        services.AddSingleton<ILoggerService, LoggerManager>();
    }

    public static void ConfigureActionFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidationAttributeFilter>();
        services.AddScoped<LogFilterAttribute>();
    }
}