using Microsoft.AspNetCore.Builder;
using UserManagement.Infrastructure.Middleware;

namespace UserManagement.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseMiddleware<EventualConsistencyMiddleware>();

        return app;
    }
}
