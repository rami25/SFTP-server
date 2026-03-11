namespace SFTPPortal.API.Middleware;

using SFTPPortal.Domain.Interfaces;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        // Extract token from Authorization header
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            // Attach user info to context if token is valid
            if (authService.ValidateToken(token))
            {
                context.Items["IsAuthenticated"] = true;
            }
        }

        await _next(context);
    }
}