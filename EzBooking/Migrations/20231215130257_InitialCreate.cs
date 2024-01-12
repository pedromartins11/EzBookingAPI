using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzBooking.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentStates",
                columns: table => new
                {
                    id_paymentStates = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStates", x => x.id_paymentStates);
                });

            migrationBuilder.CreateTable(
                name: "PostalCodes",
                columns: table => new
                {
                    postalCode = table.Column<int>(type: "int", nullable: false),
                    concelho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalCodes", x => x.postalCode);
                });

            migrationBuilder.CreateTable(
                name: "ReservationStates",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationStates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RevokedTokens",
                columns: table => new
                {
                    id_revokedToken = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    revocationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokedTokens", x => x.id_revokedToken);
                });

            migrationBuilder.CreateTable(
                name: "StatusHouses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusHouses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    id_userType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.id_userType);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userTypeid_userType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id_user);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_userTypeid_userType",
                        column: x => x.userTypeid_userType,
                        principalTable: "UserTypes",
                        principalColumn: "id_userType");
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    id_house = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    doorNumber = table.Column<int>(type: "int", nullable: false),
                    floorNumber = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: true),
                    priceyear = table.Column<double>(type: "float", nullable: true),
                    guestsNumber = table.Column<int>(type: "int", nullable: false),
                    rooms = table.Column<int>(type: "int", nullable: true),
                    road = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    propertyAssessment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codDoor = table.Column<int>(type: "int", nullable: true),
                    sharedRoom = table.Column<bool>(type: "bit", nullable: false),
                    postalCode = table.Column<int>(type: "int", nullable: false),
                    StatusHouseid = table.Column<int>(type: "int", nullable: true),
                    Userid_user = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.id_house);
                    table.ForeignKey(
                        name: "FK_Houses_PostalCodes_postalCode",
                        column: x => x.postalCode,
                        principalTable: "PostalCodes",
                        principalColumn: "postalCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Houses_StatusHouses_StatusHouseid",
                        column: x => x.StatusHouseid,
                        principalTable: "StatusHouses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Houses_Users_Userid_user",
                        column: x => x.Userid_user,
                        principalTable: "Users",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    id_image = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Houseid_house = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.id_image);
                    table.ForeignKey(
                        name: "FK_Images_Houses_Houseid_house",
                        column: x => x.Houseid_house,
                        principalTable: "Houses",
                        principalColumn: "id_house",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    id_reservation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    init_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    guestsNumber = table.Column<int>(type: "int", nullable: false),
                    Userid_user = table.Column<int>(type: "int", nullable: true),
                    Houseid_house = table.Column<int>(type: "int", nullable: true),
                    ReservationStatesid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.id_reservation);
                    table.ForeignKey(
                        name: "FK_Reservations_Houses_Houseid_house",
                        column: x => x.Houseid_house,
                        principalTable: "Houses",
                        principalColumn: "id_house");
                    table.ForeignKey(
                        name: "FK_Reservations_ReservationStates_ReservationStatesid",
                        column: x => x.ReservationStatesid,
                        principalTable: "ReservationStates",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Reservations_Users_Userid_user",
                        column: x => x.Userid_user,
                        principalTable: "Users",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    id_feedback = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    classification = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reservationid_reservation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.id_feedback);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Reservations_Reservationid_reservation",
                        column: x => x.Reservationid_reservation,
                        principalTable: "Reservations",
                        principalColumn: "id_reservation");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id_payment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    creationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paymentValue = table.Column<float>(type: "real", nullable: false),
                    stateid_paymentStates = table.Column<int>(type: "int", nullable: false),
                    Reservationid_reservation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id_payment);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentStates_stateid_paymentStates",
                        column: x => x.stateid_paymentStates,
                        principalTable: "PaymentStates",
                        principalColumn: "id_paymentStates",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Reservations_Reservationid_reservation",
                        column: x => x.Reservationid_reservation,
                        principalTable: "Reservations",
                        principalColumn: "id_reservation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Reservationid_reservation",
                table: "Feedbacks",
                column: "Reservationid_reservation");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_postalCode",
                table: "Houses",
                column: "postalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_StatusHouseid",
                table: "Houses",
                column: "StatusHouseid");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_Userid_user",
                table: "Houses",
                column: "Userid_user");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Houseid_house",
                table: "Images",
                column: "Houseid_house");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Reservationid_reservation",
                table: "Payments",
                column: "Reservationid_reservation");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_stateid_paymentStates",
                table: "Payments",
                column: "stateid_paymentStates");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Houseid_house",
                table: "Reservations",
                column: "Houseid_house");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationStatesid",
                table: "Reservations",
                column: "ReservationStatesid");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Userid_user",
                table: "Reservations",
                column: "Userid_user");

            migrationBuilder.CreateIndex(
                name: "IX_Users_userTypeid_userType",
                table: "Users",
                column: "userTypeid_userType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
<<<<<<<< HEAD:EzBooking/Migrations/20231211193728_InitialCreate.cs
                name: "PaymentStates");
========
                name: "RevokedTokens");
>>>>>>>> 7c5b44991fd4f4cc0aa53488f63f4e2698806586:EzBooking/Migrations/20231215130257_InitialCreate.cs

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "ReservationStates");

            migrationBuilder.DropTable(
                name: "PostalCodes");

            migrationBuilder.DropTable(
                name: "StatusHouses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
