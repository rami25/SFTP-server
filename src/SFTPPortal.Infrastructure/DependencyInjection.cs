namespace SFTPPortal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFTPPortal.Application.Validators;
using SFTPPortal.Domain.Interfaces;
using SFTPPortal.Infrastructure.Auth;
using SFTPPortal.Infrastructure.Persistence;
using SFTPPortal.Infrastructure.Persistence.Repositories;
using SFTPPortal.Infrastructure.Sftp;
using SFTPPortal.Application.UseCases.Auth;
using SFTPPortal.Application.UseCases.Files;
using SFTPPortal.Application.UseCases.Folders;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // SQLite database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")
            ));

        // ── Repositories ────────────────────────────────────────────
        services.AddScoped<IUserRepository, UserRepository>();

        // ── Auth Service ────────────────────────────────────────────
        var jwtSecret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("JWT Secret not configured.");
        var jwtExpiryHours = int.Parse(configuration["Jwt:ExpiryHours"] ?? "8");

        services.AddSingleton<IAuthService>(
            new AuthService(jwtSecret, jwtExpiryHours));

        // ── SFTP Service ────────────────────────────────────────────
        var sftpHost = configuration["Sftp:Host"]
            ?? throw new InvalidOperationException("SFTP Host not configured.");
        var sftpPort = int.Parse(configuration["Sftp:Port"] ?? "22");
        var sftpUsername = configuration["Sftp:Username"]
            ?? throw new InvalidOperationException("SFTP Username not configured.");
        var sftpPassword = configuration["Sftp:Password"]
            ?? throw new InvalidOperationException("SFTP Password not configured.");

        services.AddSingleton<ISftpService>(
            new SftpService(sftpHost, sftpPort, sftpUsername, sftpPassword));

        // ── File Naming Validator ───────────────────────────────────
        services.AddSingleton<IFileNamingService, FileNameValidator>();

        // ── Use Cases ───────────────────────────────────────────────
        services.AddScoped<LoginUseCase>();
        services.AddScoped<ListFoldersUseCase>();
        services.AddScoped<ListFilesUseCase>();
        services.AddScoped<UploadFileUseCase>();
        services.AddScoped<DownloadFileUseCase>();

        return services;
    }
}