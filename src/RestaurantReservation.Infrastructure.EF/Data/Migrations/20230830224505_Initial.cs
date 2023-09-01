using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RestaurantReservation.Infrastructure.EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    openingtime = table.Column<TimeSpan>(name: "opening_time", type: "interval", nullable: false),
                    closingtime = table.Column<TimeSpan>(name: "closing_time", type: "interval", nullable: false),
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
                name: "customer_favourite_restaurant_ids",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerfavouriterestaurantid = table.Column<Guid>(name: "customer_favourite_restaurant_id", type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_favourite_restaurant_ids", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_favourite_restaurant_ids_customers_customer_id",
                        column: x => x.customerid,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_reservation_ids",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerreservationid = table.Column<Guid>(name: "customer_reservation_id", type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_reservation_ids", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_reservation_ids_customers_customer_id",
                        column: x => x.customerid,
                        principalTable: "customers",
                        principalColumn: "customer_id",
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
                    reservationid = table.Column<Guid>(name: "reservation_id", type: "uuid", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "tables",
                columns: table => new
                {
                    tableid = table.Column<Guid>(name: "table_id", type: "uuid", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    restaurantid = table.Column<Guid>(name: "restaurant_id", type: "uuid", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "timestamp with time zone", nullable: true),
                    createdby = table.Column<long>(name: "created_by", type: "bigint", nullable: true),
                    lastmodified = table.Column<DateTime>(name: "last_modified", type: "timestamp with time zone", nullable: true),
                    lastmodifiedby = table.Column<long>(name: "last_modified_by", type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tables", x => x.tableid);
                    table.ForeignKey(
                        name: "fk_tables_restaurants_restaurant_id",
                        column: x => x.restaurantid,
                        principalTable: "restaurants",
                        principalColumn: "restaurant_id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "fk_reservations_tables_table_id",
                        column: x => x.tableid,
                        principalTable: "tables",
                        principalColumn: "table_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_customer_favourite_restaurant_ids_customer_id",
                table: "customer_favourite_restaurant_ids",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_reservation_ids_customer_id",
                table: "customer_reservation_ids",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_table_id",
                table: "reservations",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_restaurant_id",
                table: "reviews",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "ix_tables_restaurant_id",
                table: "tables",
                column: "restaurant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_favourite_restaurant_ids");

            migrationBuilder.DropTable(
                name: "customer_reservation_ids");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "tables");

            migrationBuilder.DropTable(
                name: "restaurants");
        }
    }
}
