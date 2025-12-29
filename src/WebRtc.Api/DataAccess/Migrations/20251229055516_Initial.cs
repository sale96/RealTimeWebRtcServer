using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRtc.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    IpV6Address = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CameraStatus = table.Column<int>(type: "integer", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    MaxParticipants = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MeetingOrganizerId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingOrganizerId1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_Clients_MeetingOrganizerId1",
                        column: x => x.MeetingOrganizerId1,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_MeetingId",
                table: "Clients",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_IdempotencyKey",
                table: "Meetings",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_MeetingOrganizerId1",
                table: "Meetings",
                column: "MeetingOrganizerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Meetings_MeetingId",
                table: "Clients",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Meetings_MeetingId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
