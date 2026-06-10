using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeNest.Migrations
{
    /// <inheritdoc />
    public partial class SplitUserRatingsAndAddListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "UserReviews",
                newName: "ResponseTimeRating");

            migrationBuilder.AddColumn<int>(
                name: "DescriptionRating",
                table: "UserReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ListingId",
                table: "UserReviews",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PolitenessRating",
                table: "UserReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_ListingId",
                table: "UserReviews",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_Listings_ListingId",
                table: "UserReviews",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_Listings_ListingId",
                table: "UserReviews");

            migrationBuilder.DropIndex(
                name: "IX_UserReviews_ListingId",
                table: "UserReviews");

            migrationBuilder.DropColumn(
                name: "DescriptionRating",
                table: "UserReviews");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "UserReviews");

            migrationBuilder.DropColumn(
                name: "PolitenessRating",
                table: "UserReviews");

            migrationBuilder.RenameColumn(
                name: "ResponseTimeRating",
                table: "UserReviews",
                newName: "Rating");
        }
    }
}
