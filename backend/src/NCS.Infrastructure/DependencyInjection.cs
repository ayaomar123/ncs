using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NCS.Application.Interfaces.Auth;
using NCS.Application.Interfaces.Payments;
using NCS.Application.Interfaces.Repositories;
using NCS.Application.Interfaces.Storage;
using NCS.Infrastructure.Persistence;
using NCS.Infrastructure.Repositories;
using NCS.Infrastructure.Services;
using NCS.Infrastructure.Storage;

namespace NCS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NcsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAppealRepository, AppealRepository>();
        services.AddScoped<IBlogPostRepository, BlogPostRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IDonationRepository, DonationRepository>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();

        services.AddSingleton<IPasswordHasher, PasswordHasherService>();

        services.AddSingleton<IPaymentProvider, PaymentProviderStub>();

        services.AddOptions<LocalFileStorageOptions>()
            .Bind(configuration.GetSection(LocalFileStorageOptions.SectionName))
            .ValidateDataAnnotations();

        services.AddSingleton<IFileStorage, LocalFileStorage>();

        return services;
    }
}
