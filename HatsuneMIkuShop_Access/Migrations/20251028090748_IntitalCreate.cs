using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifetimeLiveHouse.Access.Migrations
{
    /// <inheritdoc />
    public partial class IntitalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BandRole",
                columns: table => new
                {
                    BandRoleID = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandRole", x => x.BandRoleID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CateID = table.Column<string>(type: "nchar(5)", maxLength: 5, nullable: false),
                    CateName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CateID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRole",
                columns: table => new
                {
                    RoleCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRole", x => x.RoleCode);
                });

            migrationBuilder.CreateTable(
                name: "EventStatus",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatus", x => x.StatusCode);
                });

            migrationBuilder.CreateTable(
                name: "MemberStatus",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatus", x => x.StatusCode);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsTitle = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsID);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.StatusCode);
                });

            migrationBuilder.CreateTable(
                name: "PayType",
                columns: table => new
                {
                    PayCode = table.Column<string>(type: "nchar(2)", maxLength: 2, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ShippingFee = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayType", x => x.PayCode);
                });

            migrationBuilder.CreateTable(
                name: "ProductStatus",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatus", x => x.StatusCode);
                });

            migrationBuilder.CreateTable(
                name: "ShippingMethod",
                columns: table => new
                {
                    ShippingMethodCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false),
                    Method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMethod", x => x.ShippingMethodCode);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    StoreID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    RentFeePerHour = table.Column<decimal>(type: "money", nullable: false),
                    sTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.StoreID);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cash = table.Column<decimal>(type: "money", nullable: false),
                    CellphoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MemberPoint = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Member_MemberStatus_StatusCode",
                        column: x => x.StatusCode,
                        principalTable: "MemberStatus",
                        principalColumn: "StatusCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ProductNum = table.Column<long>(type: "bigint", nullable: false),
                    Pricing = table.Column<decimal>(type: "money", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "money", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CateID = table.Column<string>(type: "nchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_Category_CateID",
                        column: x => x.CateID,
                        principalTable: "Category",
                        principalColumn: "CateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_ProductStatus_StatusCode",
                        column: x => x.StatusCode,
                        principalTable: "ProductStatus",
                        principalColumn: "StatusCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderID = table.Column<long>(type: "bigint", nullable: false),
                    ProductID = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShippingMethodCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => new { x.OrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_OrderDetail_ShippingMethod_ShippingMethodCode",
                        column: x => x.ShippingMethodCode,
                        principalTable: "ShippingMethod",
                        principalColumn: "ShippingMethodCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    StoreID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employee_EmployeeRole_RoleCode",
                        column: x => x.RoleCode,
                        principalTable: "EmployeeRole",
                        principalColumn: "RoleCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationFee = table.Column<decimal>(type: "money", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventPicture = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StoreID = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    StatusCode = table.Column<string>(type: "nchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Event_EventStatus_StatusCode",
                        column: x => x.StatusCode,
                        principalTable: "EventStatus",
                        principalColumn: "StatusCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                columns: table => new
                {
                    InstrumentID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstrumentName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    RentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutRentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentFeePerHour = table.Column<decimal>(type: "money", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstrumentPhoto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StoreID = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.InstrumentID);
                    table.ForeignKey(
                        name: "FK_Instrument_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Instrument_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Live",
                columns: table => new
                {
                    LiveID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Admission = table.Column<decimal>(type: "money", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveSong = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    BandRoleID = table.Column<string>(type: "nchar(1)", nullable: false),
                    StoreID = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventStatusStatusCode = table.Column<string>(type: "nchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Live", x => x.LiveID);
                    table.ForeignKey(
                        name: "FK_Live_BandRole_BandRoleID",
                        column: x => x.BandRoleID,
                        principalTable: "BandRole",
                        principalColumn: "BandRoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Live_EventStatus_EventStatusStatusCode",
                        column: x => x.EventStatusStatusCode,
                        principalTable: "EventStatus",
                        principalColumn: "StatusCode");
                    table.ForeignKey(
                        name: "FK_Live_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Live_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginRecord",
                columns: table => new
                {
                    RecordID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Record = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContinuousLoginDate = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginRecord", x => x.RecordID);
                    table.ForeignKey(
                        name: "FK_LoginRecord_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberAccount",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAccount", x => x.Email);
                    table.ForeignKey(
                        name: "FK_MemberAccount_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberEmailVerificationStatus",
                columns: table => new
                {
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerificationTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailVerificationTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberEmailVerificationStatus", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_MemberEmailVerificationStatus_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberHeadPicture",
                columns: table => new
                {
                    Picture = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberHeadPicture", x => x.Picture);
                    table.ForeignKey(
                        name: "FK_MemberHeadPicture_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberPhoneVerificationStatus",
                columns: table => new
                {
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    IsPhoneVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPhoneVerificationStatus", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_MemberPhoneVerificationStatus_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberPicture",
                columns: table => new
                {
                    Picture = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPicture", x => x.Picture);
                    table.ForeignKey(
                        name: "FK_MemberPicture_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Readed = table.Column<bool>(type: "bit", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notification_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetToken",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetToken_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RehearsalStudio",
                columns: table => new
                {
                    RehearsalStudioID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RehearsalStudioName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StartRentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutRentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentFeePerHour = table.Column<decimal>(type: "money", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RehearsalStudioPhoto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StoreID = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RehearsalStudio", x => x.RehearsalStudioID);
                    table.ForeignKey(
                        name: "FK_RehearsalStudio_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_RehearsalStudio_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    SeatID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    StoreID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.SeatID);
                    table.ForeignKey(
                        name: "FK_Seat_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seat_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartID);
                    table.ForeignKey(
                        name: "FK_Cart_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    cNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Discount = table.Column<decimal>(type: "money", nullable: false),
                    ProductID = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.cNo);
                    table.ForeignKey(
                        name: "FK_Coupon_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Coupon_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetailProduct",
                columns: table => new
                {
                    ProductsProductID = table.Column<long>(type: "bigint", nullable: false),
                    OrderDetailOrderID = table.Column<long>(type: "bigint", nullable: false),
                    OrderDetailProductID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetailProduct", x => new { x.ProductsProductID, x.OrderDetailOrderID, x.OrderDetailProductID });
                    table.ForeignKey(
                        name: "FK_OrderDetailProduct_OrderDetail_OrderDetailOrderID_OrderDetailProductID",
                        columns: x => new { x.OrderDetailOrderID, x.OrderDetailProductID },
                        principalTable: "OrderDetail",
                        principalColumns: new[] { "OrderID", "ProductID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetailProduct_Product_ProductsProductID",
                        column: x => x.ProductsProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecord",
                columns: table => new
                {
                    AttendanceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PunchInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PunchOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecord", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_AttendanceRecord_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAccount",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmployeeID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAccount", x => x.Email);
                    table.ForeignKey(
                        name: "FK_EmployeeAccount_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    oTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MemberID = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeID = table.Column<long>(type: "bigint", nullable: true),
                    PayCode = table.Column<string>(type: "nchar(2)", maxLength: 2, nullable: true),
                    StatusCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Order_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_OrderStatus_StatusCode",
                        column: x => x.StatusCode,
                        principalTable: "OrderStatus",
                        principalColumn: "StatusCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_PayType_PayCode",
                        column: x => x.PayCode,
                        principalTable: "PayType",
                        principalColumn: "PayCode");
                });

            migrationBuilder.CreateTable(
                name: "RegisteredEvent",
                columns: table => new
                {
                    RecordID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<long>(type: "bigint", nullable: false),
                    MemberID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredEvent", x => x.RecordID);
                    table.ForeignKey(
                        name: "FK_RegisteredEvent_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredEvent_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "OrderOrderDetail",
                columns: table => new
                {
                    OrdersOrderID = table.Column<long>(type: "bigint", nullable: false),
                    OrderDetailOrderID = table.Column<long>(type: "bigint", nullable: false),
                    OrderDetailProductID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOrderDetail", x => new { x.OrdersOrderID, x.OrderDetailOrderID, x.OrderDetailProductID });
                    table.ForeignKey(
                        name: "FK_OrderOrderDetail_OrderDetail_OrderDetailOrderID_OrderDetailProductID",
                        columns: x => new { x.OrderDetailOrderID, x.OrderDetailProductID },
                        principalTable: "OrderDetail",
                        principalColumns: new[] { "OrderID", "ProductID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderOrderDetail_Order_OrdersOrderID",
                        column: x => x.OrdersOrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecord_EmployeeID",
                table: "AttendanceRecord",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_MemberID",
                table: "Cart",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductID",
                table: "Cart",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_MemberID",
                table: "Coupon",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_ProductID",
                table: "Coupon",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_RoleCode",
                table: "Employee",
                column: "RoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_StoreID",
                table: "Employee",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAccount_EmployeeID",
                table: "EmployeeAccount",
                column: "EmployeeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_MemberID",
                table: "Event",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_StatusCode",
                table: "Event",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_Event_StoreID",
                table: "Event",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_MemberID",
                table: "Instrument",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_StoreID",
                table: "Instrument",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Live_BandRoleID",
                table: "Live",
                column: "BandRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Live_EventStatusStatusCode",
                table: "Live",
                column: "EventStatusStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_Live_MemberID",
                table: "Live",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Live_StoreID",
                table: "Live",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_LoginRecord_MemberID",
                table: "LoginRecord",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Member_StatusCode",
                table: "Member",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAccount_MemberID",
                table: "MemberAccount",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberHeadPicture_MemberID",
                table: "MemberHeadPicture",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberPicture_MemberID",
                table: "MemberPicture",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_MemberID",
                table: "Notification",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EmployeeID",
                table: "Order",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MemberID",
                table: "Order",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PayCode",
                table: "Order",
                column: "PayCode");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StatusCode",
                table: "Order",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ShippingMethodCode",
                table: "OrderDetail",
                column: "ShippingMethodCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailProduct_OrderDetailOrderID_OrderDetailProductID",
                table: "OrderDetailProduct",
                columns: new[] { "OrderDetailOrderID", "OrderDetailProductID" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderOrderDetail_OrderDetailOrderID_OrderDetailProductID",
                table: "OrderOrderDetail",
                columns: new[] { "OrderDetailOrderID", "OrderDetailProductID" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_MemberID",
                table: "PasswordResetToken",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CateID",
                table: "Product",
                column: "CateID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_StatusCode",
                table: "Product",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredEvent_EventID",
                table: "RegisteredEvent",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredEvent_MemberID",
                table: "RegisteredEvent",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_RehearsalStudio_MemberID",
                table: "RehearsalStudio",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_RehearsalStudio_StoreID",
                table: "RehearsalStudio",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_MemberID",
                table: "Seat",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seat_StoreID",
                table: "Seat",
                column: "StoreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRecord");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "EmployeeAccount");

            migrationBuilder.DropTable(
                name: "Instrument");

            migrationBuilder.DropTable(
                name: "Live");

            migrationBuilder.DropTable(
                name: "LoginRecord");

            migrationBuilder.DropTable(
                name: "MemberAccount");

            migrationBuilder.DropTable(
                name: "MemberEmailVerificationStatus");

            migrationBuilder.DropTable(
                name: "MemberHeadPicture");

            migrationBuilder.DropTable(
                name: "MemberPhoneVerificationStatus");

            migrationBuilder.DropTable(
                name: "MemberPicture");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderDetailProduct");

            migrationBuilder.DropTable(
                name: "OrderOrderDetail");

            migrationBuilder.DropTable(
                name: "PasswordResetToken");

            migrationBuilder.DropTable(
                name: "RegisteredEvent");

            migrationBuilder.DropTable(
                name: "RehearsalStudio");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "BandRole");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ProductStatus");

            migrationBuilder.DropTable(
                name: "ShippingMethod");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "PayType");

            migrationBuilder.DropTable(
                name: "EventStatus");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "EmployeeRole");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "MemberStatus");
        }
    }
}
