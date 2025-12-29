using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebRtc.Api.Core;
using WebRtc.Api.DataAccess;
using WebRtc.Api.DataAccess.Entities;

namespace WebRtc.Api.Features.Meetings;

public class CreateMeeting : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/meeting/create", async (
            [FromBody] CreateMeetingDto createMeetingDto,
            HttpContext httpContext,
            ServerDbContext context) =>
        {
            var meeting = new Meeting();
            meeting.StartTime = createMeetingDto.StartTime;
            meeting.MaxParticipants = createMeetingDto.MaxParticipants;

            var hasher = new PasswordHasher<object>();
            if (createMeetingDto.Password is not null)
            {
                var password = hasher.HashPassword(null, createMeetingDto.Password);
                meeting.Password = password;
            }

            var client = new Client
            {
                Name = createMeetingDto.ClientName,
                IpAddress = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "0.0.0.0",
                IpV6Address = httpContext.Connection.RemoteIpAddress?.MapToIPv6().ToString() ?? "0.0.0.0",
                UserAgent = httpContext.Request.Headers.UserAgent.ToString(),
                IsOrganizer = true
            };
            
            meeting.Clients = new List<Client> { client };

            var idempotencyKeyData = JsonConvert.SerializeObject(new
            {
                Client = client,
                Meeting = new
                {
                    meeting.StartTime,
                    meeting.MaxParticipants,
                    meeting.Password,
                }
            });
            
            var idempotencyKey = hasher.HashPassword(null, idempotencyKeyData);
            
            meeting.IdempotencyKey = idempotencyKey;
            
            context.Meetings.Add(meeting);
            await context.SaveChangesAsync();
            
            return Results.Created("api/meeting/create",
                new {
                    Id = meeting.Id,
                    ClientId = client.Id,
                });
        });
    }
}

public class CreateMeetingDto
{
    public string ClientName { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public string? Password { get; set; }
}