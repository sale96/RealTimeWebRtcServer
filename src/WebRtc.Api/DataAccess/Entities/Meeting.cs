namespace WebRtc.Api.DataAccess.Entities;

public class Meeting
{
    public Guid Id { get; set; }
    
    public string IdempotencyKey { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }

    public MeetingStatusEnum Status { get; set; } = MeetingStatusEnum.Pending;

    public string Password { get; set; } = string.Empty;
    
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public List<Client> Clients { get; set; } = null!;
}

public enum MeetingStatusEnum
{
    Pending,
    Active,
    Finished,
}