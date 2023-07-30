using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UKFursBot.Migrations
{
    /// <inheritdoc />
    public partial class removedguildfornow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "BotConfigurations");

            migrationBuilder.AddColumn<long>(
                name: "MinutesThresholdForMessagesBeforeEditsAreSuspicious",
                table: "BotConfigurations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinutesThresholdForMessagesBeforeEditsAreSuspicious",
                table: "BotConfigurations");

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "BotConfigurations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
