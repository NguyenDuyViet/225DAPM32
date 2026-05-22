using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    id_Message = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sender_Id = table.Column<int>(type: "int", nullable: false),
                    sender_Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    sender_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    order_Id = table.Column<int>(type: "int", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sent_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_Read = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.id_Message);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");
        }
    }
}
