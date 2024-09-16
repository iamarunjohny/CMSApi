using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Passwords = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Users__1788CC4C2F1C2AF2", x => x.UserId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Contacts",
            //    columns: table => new
            //    {
            //        ContactId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserId = table.Column<int>(type: "int", nullable: true),
            //        FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Company = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Contacts__5C66259B07BC09AA", x => x.ContactId);
            //        table.ForeignKey(
            //            name: "FK__Contacts__UserId__4BAC3F29",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "UserId");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Contacts_UserId",
            //    table: "Contacts",
            //    column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
