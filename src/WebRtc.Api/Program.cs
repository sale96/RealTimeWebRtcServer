using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebRtc.Api.Core.Extensions;
using WebRtc.Api.DataAccess;
using WebRtc.Api.Features.Meetings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddSignalR();

builder.Services.AddDbContext<ServerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b =>
    {
        b.MigrationsAssembly(typeof(ServerDbContext).Assembly.FullName);
        b.MigrationsHistoryTable("__EFMigrationsHistory", "public");
        b.EnableRetryOnFailure(10);
        b.CommandTimeout(100);
    });
});

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("CorsPolicy");

app.UseMeetingServices();
app.MapEndpoints();
app.Run();
