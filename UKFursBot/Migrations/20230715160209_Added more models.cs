using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class Addedmoremodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BansOnJoin",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserID = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ModID = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BansOnJoin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotConfigurations",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AnnouncementChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogging",
                columns: table => new
                {
                    ServerId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogging", x => x.ServerId);
                });

            migrationBuilder.CreateTable(
                name: "ModMails",
                columns: table => new
                {
                    ServerId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ModRoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModMails", x => x.ServerId);
                });

            migrationBuilder.CreateTable(
                name: "UserNotes",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Forgiven = table.Column<bool>(type: "boolean", nullable: false),
                    ForgivenBy = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ModeratorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warnings",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Forgiven = table.Column<bool>(type: "boolean", nullable: false),
                    ForgivenBy = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ModeratorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warnings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BansOnJoin");

            migrationBuilder.DropTable(
                name: "BotConfigurations");

            migrationBuilder.DropTable(
                name: "ErrorLogging");

            migrationBuilder.DropTable(
                name: "ModMails");

            migrationBuilder.DropTable(
                name: "UserNotes");

            migrationBuilder.DropTable(
                name: "Warnings");
        }
    }
}
