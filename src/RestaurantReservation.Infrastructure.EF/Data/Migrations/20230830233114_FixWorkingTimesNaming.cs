using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Infrastructure.EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixWorkingTimesNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "opening_time",
                table: "restaurants",
                newName: "wednesday_opening_time");

            migrationBuilder.RenameColumn(
                name: "closing_time",
                table: "restaurants",
                newName: "wednesday_closing_time");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "friday_closing_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "friday_opening_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "monday_closing_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "monday_opening_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "saturday_closing_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "saturday_opening_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "sunday_closing_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "sunday_opening_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "thursday_closing_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "thursday_opening_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "tuesday_closing_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "tuesday_opening_time",
                table: "restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "friday_closing_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "friday_opening_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "monday_closing_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "monday_opening_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "saturday_closing_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "saturday_opening_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "sunday_closing_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "sunday_opening_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "thursday_closing_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "thursday_opening_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "tuesday_closing_time",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "tuesday_opening_time",
                table: "restaurants");

            migrationBuilder.RenameColumn(
                name: "wednesday_opening_time",
                table: "restaurants",
                newName: "opening_time");

            migrationBuilder.RenameColumn(
                name: "wednesday_closing_time",
                table: "restaurants",
                newName: "closing_time");
        }
    }
}
