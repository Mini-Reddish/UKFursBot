using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class updatedmodelsformoderation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WasSentToUser",
                table: "Warnings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ModerationLoggingChannel",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BanLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserID = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ModID = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    WasSentToUser = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanLogs");

            migrationBuilder.DropColumn(
                name: "WasSentToUser",
                table: "Warnings");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "BotConfigurations");

            migrationBuilder.DropColumn(
                name: "ModerationLoggingChannel",
                table: "BotConfigurations");
        }
    }
}
