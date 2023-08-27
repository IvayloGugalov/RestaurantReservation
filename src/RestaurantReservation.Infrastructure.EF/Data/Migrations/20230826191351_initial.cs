using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Infrastructure.EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customerid = table.Column<Guid>(name: "customer_id", type: "uuid", nullable: false),
                    firstname = table.Column<string>(name: "first_name", type: "character varying(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(name: "last_name", type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    lastmodified = table.Column<DateTime>(name: "last_modified", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<long>(name: "last_modified_by", type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.customerid);
                });

            migrationBuilder.CreateTable(
                name: "restaurants",
                columns: table => new
                {
                    restaurantid = table.Column<Guid>(name: "restaurant_id", type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    website = table.Column<string>(name: "web_site", type: "text", nullable: false),
                    worktimemondayopeningtime = table.Column<TimeSpan>(name: "work_time_monday_opening_time", type: "interval", nullable: false),
                    worktimemondayclosingtime = table.Column<TimeSpan>(name: "work_time_monday_closing_time", type: "interval", nullable: false),
                    worktimetuesdayopeningtime = table.Column<TimeSpan>(name: "work_time_tuesday_opening_time", type: "interval", nullable: false),
                    worktimetuesdayclosingtime = table.Column<TimeSpan>(name: "work_time_tuesday_closing_time", type: "interval", nullable: false),
                    worktimewednesdayopeningtime = table.Column<TimeSpan>(name: "work_time_wednesday_opening_time", type: "interval", nullable: false),
                    worktimewednesdayclosingtime = table.Column<TimeSpan>(name: "work_time_wednesday_closing_time", type: "interval", nullable: false),
                    worktimethursdayopeningtime = table.Column<TimeSpan>(name: "work_time_thursday_opening_time", type: "interval", nullable: false),
                    worktimethursdayclosingtime = table.Column<TimeSpan>(name: "work_time_thursday_closing_time", type: "interval", nullable: false),
                    worktimefridayopeningtime = table.Column<TimeSpan>(name: "work_time_friday_opening_time", type: "interval", nullable: false),
                    worktimefridayclosingtime = table.Column<TimeSpan>(name: "work_time_friday_closing_time", type: "interval", nullable: false),
                    worktimesaturdayopeningtime = table.Column<TimeSpan>(name: "work_time_saturday_opening_time", type: "interval", nullable: false),
                    worktimesaturdayclosingtime = table.Column<TimeSpan>(name: "work_time_saturday_closing_time", type: "interval", nullable: false),
                    worktimesundayopeningtime = table.Column<TimeSpan>(name: "work_time_sunday_opening_time", type: "interval", nullable: false),
                    worktimesundayclosingtime = table.Column<TimeSpan>(name: "work_time_sunday_closing_time", type: "interval", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    lastmodified = table.Column<DateTime>(name: "last_modified", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<long>(name: "last_modified_by", type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_restaurants", x => x.restaurantid);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    reservationid = table.Column<Guid>(name: "reservation_id", type: "uuid", nullable: false),
                    restaurantid = table.Column<Guid>(name: "restaurant_id", type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uuid", nullable: false),
                    tableid = table.Column<Guid>(name: "table_id", type: "uuid", nullable: false),
                    reservationdate = table.Column<DateTime>(name: "reservation_date", type: "date", nullable: false),
                    occupants = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    reviewid = table.Column<Guid>(name: "review_id", type: "uuid", nullable: true),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    lastmodified = table.Column<DateTime>(name: "last_modified", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<long>(name: "last_modified_by", type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reservations", x => x.reservationid);
                    table.ForeignKey(
                        name: "fk_reservations_customers_customer_id",
                        column: x => x.customerid,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reservations_restaurants_restaurant_id",
                        column: x => x.restaurantid,
                        principalTable: "restaurants",
                        principalColumn: "restaurant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    reviewid = table.Column<Guid>(name: "review_id", type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    restaurantid = table.Column<Guid>(name: "restaurant_id", type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uuid", nullable: false),
                    customername = table.Column<string>(name: "customer_name", type: "text", nullable: false),
                    reservationid = table.Column<Guid>(name: "reservation_id", type: "uuid", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    lastmodified = table.Column<DateTime>(name: "last_modified", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<long>(name: "last_modified_by", type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.reviewid);
                    table.ForeignKey(
                        name: "fk_reviews_restaurants_restaurant_id",
                        column: x => x.restaurantid,
                        principalTable: "restaurants",
                        principalColumn: "restaurant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reservations_customer_id",
                table: "reservations",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_restaurant_id",
                table: "reservations",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_restaurant_id",
                table: "reviews",
                column: "restaurant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "restaurants");
        }
    }
}
