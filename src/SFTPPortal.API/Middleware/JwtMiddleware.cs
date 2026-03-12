namespace SFTPPortal.API.Middleware;
using SFTPPortal.Domain.Interfaces;
public class JwtMiddleware {
    private readonly RequestDelegate _next;
    public JwtMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService) {
        var token = context.Request.Headers["Authorization"] // get token
            .FirstOrDefault()?.Split(" ").Last();

        if (token != null) {
            if (authService.ValidateToken(token))
            {
                context.Items["IsAuthenticated"] = true;
            }
        }

        await _next(context);
    }
}