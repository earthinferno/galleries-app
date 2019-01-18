using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GalleriesServer.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 256, nullable: true),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: true),
                    ExternalUserId = table.Column<string>(maxLength: 256, nullable: false),
                    ExternalIdentityProvider = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MediaContainers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 2048, nullable: true),
                    OwnerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaContainers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MediaContainers_Owners_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Owners",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(maxLength: 256, nullable: false),
                    ImageUri = table.Column<string>(maxLength: 256, nullable: false),
                    Comment = table.Column<string>(maxLength: 2048, nullable: true),
                    MediaContainerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MediaItems_MediaContainers_MediaContainerID",
                        column: x => x.MediaContainerID,
                        principalTable: "MediaContainers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaContainers_OwnerID",
                table: "MediaContainers",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MediaContainerID",
                table: "MediaItems",
                column: "MediaContainerID");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_ExternalUserId",
                table: "Owners",
                column: "ExternalUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropTable(
                name: "MediaContainers");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
