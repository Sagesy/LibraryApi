using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    txtAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    txtTransactionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    txtAttachmentUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    txtContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    txtNameFile = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    txtOriginalNameFile = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    dtmCreatedDate = table.Column<DateTime>(type: "date", nullable: false),
                    dtmUpdatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.txtAttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    txtBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    txtBookTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    txtBookPublisher = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    txtBookAuthor = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    intPublishedYear = table.Column<int>(type: "int", nullable: false),
                    dtmCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    dtmUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    txtCoverId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.txtBookId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogEvent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    txtOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    txtOrderName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    intDuration = table.Column<int>(type: "int", nullable: false),
                    dtmDueDate = table.Column<DateTime>(type: "date", nullable: false),
                    dtmCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    dtmUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.txtOrderId);
                });

            migrationBuilder.CreateTable(
                name: "BookOrder",
                columns: table => new
                {
                    txtOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    txtBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    txtBookOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    txtStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookOrder", x => new { x.txtBookId, x.txtOrderId });
                    table.ForeignKey(
                        name: "FK_BookOrder_Book_txtBookId",
                        column: x => x.txtBookId,
                        principalTable: "Book",
                        principalColumn: "txtBookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookOrder_Order_txtOrderId",
                        column: x => x.txtOrderId,
                        principalTable: "Order",
                        principalColumn: "txtOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookOrder_txtOrderId",
                table: "BookOrder",
                column: "txtOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "BookOrder");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
