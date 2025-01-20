using SessionReservation.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace SessionReservation.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseMiddleware<EventualConsistencyMiddleware>();

        return app;
    }
}
