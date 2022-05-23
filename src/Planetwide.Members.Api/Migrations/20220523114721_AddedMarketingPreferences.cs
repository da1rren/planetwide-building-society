using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planetwide.Members.Api.Migrations
{
    public partial class AddedMarketingPreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberMarketingPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ByPost = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ByTelephone = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ByEmail = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ByOnline = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    FaceToFace = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ByPhone = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    BySmsService = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    BySmsMarketing = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberMarketingPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberMarketingPreferences_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberMarketingPreferences_MemberId",
                table: "MemberMarketingPreferences",
                column: "MemberId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberMarketingPreferences");
        }
    }
}
