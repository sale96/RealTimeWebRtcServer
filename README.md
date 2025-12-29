# WebRTC Conference Server

A real-time communication server for WebRTC client discovery and signaling. This ASP.NET Core API provides the backend infrastructure for managing video conferences, facilitating peer-to-peer connections through WebRTC signaling, and serving the frontend application.

## Features

- **Meeting Management**: Create and manage video conference meetings with configurable participant limits
- **Real-time Signaling**: SignalR hub for WebRTC offer/answer exchange and ICE candidate negotiation
- **Client Discovery**: Automatic client registration and connection management
- **Password Protection**: Optional password protection for meetings
- **Participant Tracking**: Real-time participant count and status updates
- **Connection Management**: Automatic handling of client connections and disconnections

## Technology Stack

- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API endpoints
- **SignalR**: Real-time bidirectional communication
- **Entity Framework Core**: ORM for database operations
- **PostgreSQL**: Relational database
- **OpenAPI**: API documentation (Swagger)

## Project Structure

```
src/WebRtc.Api/
├── Core/                    # Core abstractions and extensions
│   ├── Extensions/          # Endpoint registration extensions
│   └── IEndpoint.cs         # Endpoint interface
├── DataAccess/              # Database layer
│   ├── Entities/            # Domain entities (Meeting, Client)
│   ├── Migrations/          # EF Core migrations
│   └── ServerDbContext.cs   # DbContext
├── Features/                # Feature modules
│   └── Meetings/            # Meeting feature
│       ├── CreateMeeting.cs # Meeting creation endpoint
│       ├── Hubs/            # SignalR hubs
│       │   └── MeetingHub.cs
│       └── MeetingServicesExtension.cs
└── Program.cs               # Application entry point
```

## Prerequisites

- .NET 9.0 SDK
- PostgreSQL 12+ (or compatible version)
- Git

## Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd server
```

### 2. Configure Database

Update the connection string in `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=web-rtc"
  }
}
```

### 3. Create Database

```bash
# Ensure PostgreSQL is running, then apply migrations
dotnet ef database update --project src/WebRtc.Api
```

### 4. Run the Application

```bash
cd src/WebRtc.Api
dotnet run
```

The API will be available at `https://localhost:5001` (or the port configured in `launchSettings.json`).

## API Endpoints

### Create Meeting

**POST** `/api/meeting/create`

Creates a new meeting and registers the organizer as a client.

**Request Body:**
```json
{
  "clientName": "John Doe",
  "maxParticipants": 10,
  "startTime": "2024-01-01T10:00:00Z",
  "password": "optional-password"
}
```

**Response:**
```json
{
  "id": "guid",
  "clientId": "guid"
}
```

### SignalR Hub

**Endpoint:** `/meetingHub`

#### Methods

- **JoinMeeting(meetingId, clientId)**: Join a meeting. Creates a new client if `clientId` is empty.
- **LeaveMeeting(meetingId)**: Leave a meeting.
- **SendOffer(targetId, offer)**: Send WebRTC offer to a specific client.
- **SendAnswer(targetId, sdp)**: Send WebRTC answer to a specific client.
- **SendIceCandidate(targetId, candidate)**: Send ICE candidate to a specific client.
- **SendMessage(meetingId, message)**: Broadcast a message to all meeting participants.

#### Events

- **JoinedMeeting(clientId)**: Fired when a client successfully joins a meeting.
- **ReceiveNumberOfParticipants(count)**: Broadcasted when participant count changes.
- **ReceiveOffer(offer)**: Receives WebRTC offer from another client.
- **ReceiveAnswer(connectionId, sdp)**: Receives WebRTC answer from another client.
- **ReceiveIceCandidate(connectionId, candidate)**: Receives ICE candidate from another client.
- **ReceiveMessage(message)**: Receives broadcasted message.
- **ReceiveError(message)**: Receives error message.

## Configuration

### CORS

The application is configured to allow requests from `http://localhost:4200` (typical Angular development port). To modify CORS settings, update `Program.cs`:

```csharp
builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("CorsPolicy", policyBuilder =>
        policyBuilder
            .WithOrigins("http://localhost:4200")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});
```

### Database Migrations

To create a new migration:

```bash
dotnet ef migrations add MigrationName --project src/WebRtc.Api
```

To apply migrations:

```bash
dotnet ef database update --project src/WebRtc.Api
```

## Data Models

### Meeting

- `Id`: Unique identifier (Guid)
- `IdempotencyKey`: Hash for idempotent operations
- `MaxParticipants`: Maximum number of participants
- `Status`: Meeting status (Pending, Active, Finished)
- `Password`: Hashed password (optional)
- `StartTime`: Meeting start time
- `EndTime`: Meeting end time (nullable)
- `Clients`: List of connected clients

### Client

- `Id`: Unique identifier (Guid)
- `Name`: Client display name
- `IpAddress`: IPv4 address
- `IpV6Address`: IPv6 address (nullable)
- `UserAgent`: Browser user agent
- `ConnectionId`: SignalR connection ID
- `IsOrganizer`: Whether the client is the meeting organizer
- `Status`: Audio status (Muted, UnMuted, NotAllowedToSpeak)
- `CameraStatus`: Camera status (TurnedOff, TurnedOn, NotAllowed)
- `MeetingId`: Associated meeting ID

## Development

### Running in Development Mode

The application runs with OpenAPI (Swagger) enabled in development mode. Access the API documentation at:

```
https://localhost:5001/openapi/v1.json
```

### Logging

Logging is configured in `appsettings.json`. Default log level is `Information` for the application and `Warning` for ASP.NET Core.

## Architecture

The project follows a feature-based architecture:

- **Core**: Shared abstractions and extension methods
- **DataAccess**: Database context and entity definitions
- **Features**: Feature modules with endpoints and SignalR hubs

The endpoint registration uses a custom `IEndpoint` interface pattern, allowing automatic discovery and registration of endpoints.

## Security Considerations

- Passwords are hashed using ASP.NET Core Identity's `PasswordHasher`
- CORS is configured to restrict origins
- Connection strings should be stored securely (use User Secrets or environment variables in production)

## License

[Specify your license here]

## Contributing

[Add contribution guidelines if applicable]

