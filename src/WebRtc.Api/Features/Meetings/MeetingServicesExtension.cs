using WebRtc.Api.Features.Meetings.Hubs;

namespace WebRtc.Api.Features.Meetings;

public static class MeetingServicesExtension
{
    public static IServiceCollection AddMeetingServices(this IServiceCollection services)
    {
        return services;
    }

    public static IApplicationBuilder UseMeetingServices(this WebApplication app)
    {
        app.MapHub<MeetingHub>("/meetingHub")
            .RequireCors("CorsPolicy");
        
        return app;
    }
}