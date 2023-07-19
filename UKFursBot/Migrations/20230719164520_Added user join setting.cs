using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class Addeduserjoinsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UserJoinLoggingChannelId",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "BotConfigurations");

            migrationBuilder.DropColumn(
                name: "UserJoinLoggingChannelId",
                table: "BotConfigurations");
        }
    }
}
