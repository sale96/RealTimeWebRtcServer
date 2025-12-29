namespace WebRtc.Api.DataAccess.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public string IpAddress { get; set; } = string.Empty;
    public string? IpV6Address { get; set; }

    public string UserAgent { get; set; } = string.Empty;
    
    public string ConnectionId { get; set; } = string.Empty;
    
    public bool IsOrganizer { get; set; }

    public ClientStatusEnum Status { get; set; } = ClientStatusEnum.Muted;
    public CameraStatusEnum CameraStatus { get; set; } = CameraStatusEnum.TurnedOff;
    
    public Guid MeetingId { get; set; }
    public Meeting? Meeting { get; set; }
}

public enum ClientStatusEnum
{
    Muted,
    UnMuted,
    NotAllowedToSpeak
}

public enum CameraStatusEnum
{
    TurnedOff,
    TurnedOn,
    NotAllowed,
}