using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeNest.Migrations
{
    /// <inheritdoc />
    public partial class AddedMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingReport_Listings_ListingId",
                table: "ListingReport");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingReport_Users_ReporterId",
                table: "ListingReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingReport",
                table: "ListingReport");

            migrationBuilder.RenameTable(
                name: "ListingReport",
                newName: "ListingReports");

            migrationBuilder.RenameIndex(
                name: "IX_ListingReport_ReporterId",
                table: "ListingReports",
                newName: "IX_ListingReports_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_ListingReport_ListingId",
                table: "ListingReports",
                newName: "IX_ListingReports_ListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingReports",
                table: "ListingReports",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingReports_Listings_ListingId",
                table: "ListingReports",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingReports_Users_ReporterId",
                table: "ListingReports",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingReports_Listings_ListingId",
                table: "ListingReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingReports_Users_ReporterId",
                table: "ListingReports");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingReports",
                table: "ListingReports");

            migrationBuilder.RenameTable(
                name: "ListingReports",
                newName: "ListingReport");

            migrationBuilder.RenameIndex(
                name: "IX_ListingReports_ReporterId",
                table: "ListingReport",
                newName: "IX_ListingReport_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_ListingReports_ListingId",
                table: "ListingReport",
                newName: "IX_ListingReport_ListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingReport",
                table: "ListingReport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingReport_Listings_ListingId",
                table: "ListingReport",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingReport_Users_ReporterId",
                table: "ListingReport",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
