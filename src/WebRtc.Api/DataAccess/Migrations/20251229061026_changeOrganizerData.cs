using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRtc.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changeOrganizerData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Clients_MeetingOrganizerId1",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_MeetingOrganizerId1",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "MeetingOrganizerId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "MeetingOrganizerId1",
                table: "Meetings");

            migrationBuilder.AddColumn<bool>(
                name: "IsOrganizer",
                table: "Clients",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrganizer",
                table: "Clients");

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingOrganizerId",
                table: "Meetings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingOrganizerId1",
                table: "Meetings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_MeetingOrganizerId1",
                table: "Meetings",
                column: "MeetingOrganizerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Clients_MeetingOrganizerId1",
                table: "Meetings",
                column: "MeetingOrganizerId1",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
