using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SFTPPortal.API.Middleware;
using SFTPPortal.Infrastructure;

using SFTPPortal.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers & Swagger ───────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── CORS (allow Angular dev server) ────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ── JWT Authentication ──────────────────────────────────────
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("JWT Secret not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = "SFTPPortal",
            ValidateAudience = true,
            ValidAudience = "SFTPPortal",
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// ── Infrastructure (DB, SFTP, Auth, UseCases) ───────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── Build App ───────────────────────────────────────────────
var app = builder.Build();

// ── Middleware Pipeline ─────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

using (var scope = app.Services.CreateScope()) { // seed database
    var context = scope.ServiceProvider
        .GetRequiredService<SFTPPortal.Infrastructure.Persistence.AppDbContext>();
    var authService = scope.ServiceProvider
        .GetRequiredService<SFTPPortal.Domain.Interfaces.IAuthService>();
    await DbSeeder.SeedAsync(context, authService);
}

app.Run();
