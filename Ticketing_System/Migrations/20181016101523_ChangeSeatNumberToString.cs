using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ticketing_System.Migrations
{
    public partial class ChangeSeatNumberToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SeatNumber",
                table: "Attendees",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SeatNumber",
                table: "Attendees",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
