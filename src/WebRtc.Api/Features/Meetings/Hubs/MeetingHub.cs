using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebRtc.Api.DataAccess;
using WebRtc.Api.DataAccess.Entities;

namespace WebRtc.Api.Features.Meetings.Hubs;

public class MeetingHub : Hub
{
    private readonly ILogger<MeetingHub> _logger;
    private readonly ServerDbContext _context;

    public MeetingHub(ILogger<MeetingHub> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendOffer(string targetId, string offer)
    {
        await Clients.Client(targetId).SendAsync("ReceiveOffer", offer);
    }
    
    public async Task SendAnswer(string targetId, string sdp)
    {
        await Clients.Client(targetId)
            .SendAsync("ReceiveAnswer", Context.ConnectionId, sdp);
    }

    public async Task SendIceCandidate(string targetId, string candidate)
    {
        await Clients.Client(targetId)
            .SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
    }

    public async Task JoinMeeting(string meetingId, string clientId)
    {
        _logger.LogInformation($"Joining meeting: {meetingId} {Context.ConnectionId}");
        
        var meeting = await _context.Meetings
            .Include(x => x.Clients)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(meetingId));

        if (meeting == null)
        {
            await SendError(meetingId, "There is no meeting with this meeting");

            Context.Abort();
            return;
        }

        var returnClientId = "";
        if (string.IsNullOrEmpty(clientId))
        {
            var httpContext = Context.GetHttpContext();
            var client = new Client
            {
                IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString(),
                IpV6Address = httpContext?.Connection.RemoteIpAddress?.ToString(),
                Name = Context.ConnectionId,
                IsOrganizer = false,
                UserAgent = httpContext.Request.Headers.UserAgent.ToString(),
                ConnectionId = Context.ConnectionId,
            };
            
            meeting.Clients.Add(client);
            await _context.SaveChangesAsync();
            returnClientId = client.Id.ToString();
        }
        else
        {
            var client = meeting.Clients.FirstOrDefault(x => x.Id == Guid.Parse(clientId));
            client!.ConnectionId = Context.ConnectionId;
            returnClientId = client.Id.ToString();
        }
        
        await Clients.Client(Context.ConnectionId).SendAsync("JoinedMeeting", returnClientId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);
        await Clients.Group(meetingId).SendAsync("ReceiveNumberOfParticipants", meeting.Clients.Count);
    }

    public async Task LeaveMeeting(string meetingId)
    {
        _logger.LogInformation($"Leaving meeting: {meetingId}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);
    }

    public async Task SendMessage(string meetingId, string message)
    {
        _logger.LogInformation($"Sending meeting: {meetingId}, message: {message}");
        await Clients.Group(meetingId).SendAsync("ReceiveMessage", message);
    }

    private async Task SendError(string meetingId, string message)
    {
        await Clients.Group(meetingId).SendAsync("ReceiveError", message);
    }
}