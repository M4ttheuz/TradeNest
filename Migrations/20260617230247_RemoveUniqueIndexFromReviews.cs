using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeNest.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIndexFromReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserReviews_AuthorId_TargetUserId",
                table: "UserReviews");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_AuthorId_TargetUserId",
                table: "UserReviews",
                columns: new[] { "AuthorId", "TargetUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserReviews_AuthorId_TargetUserId",
                table: "UserReviews");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_AuthorId_TargetUserId",
                table: "UserReviews",
                columns: new[] { "AuthorId", "TargetUserId" },
                unique: true);
        }
    }
}
