using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateForSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id_Category = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id_Category);
                });

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

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    id_Restaurant = table.Column<int>(type: "int", nullable: false),
                    name_Restaurant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    openTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    closeTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    lat = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false),
                    lng = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.id_Restaurant);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id_Role = table.Column<int>(type: "int", nullable: false),
                    role_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id_Role);
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    id_Food = table.Column<int>(type: "int", nullable: false),
                    id_Category = table.Column<int>(type: "int", nullable: false),
                    id_Restaurant = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    discount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    cook_Count = table.Column<int>(type: "int", nullable: true),
                    prep_Time = table.Column<int>(type: "int", nullable: true),
                    daily_Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.id_Food);
                    table.ForeignKey(
                        name: "FK_Food_Category_id_Category",
                        column: x => x.id_Category,
                        principalTable: "Category",
                        principalColumn: "id_Category",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Food_Restaurant_id_Restaurant",
                        column: x => x.id_Restaurant,
                        principalTable: "Restaurant",
                        principalColumn: "id_Restaurant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    id_Promo = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    min_OrderValue = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    max_Discount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    usage_Limit = table.Column<int>(type: "int", nullable: false),
                    used_Count = table.Column<int>(type: "int", nullable: false),
                    start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_Restaurant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.id_Promo);
                    table.ForeignKey(
                        name: "FK_Promotion_Restaurant_id_Restaurant",
                        column: x => x.id_Restaurant,
                        principalTable: "Restaurant",
                        principalColumn: "id_Restaurant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id_User = table.Column<int>(type: "int", nullable: false),
                    id_Role = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastOnline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    current_Lat = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
                    current_Lng = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
                    cancel_Rate = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id_User);
                    table.ForeignKey(
                        name: "FK_Users_Roles_id_Role",
                        column: x => x.id_Role,
                        principalTable: "Roles",
                        principalColumn: "id_Role",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id_Cart = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<int>(type: "int", nullable: false),
                    created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.id_Cart);
                    table.ForeignKey(
                        name: "FK_Cart_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    id_Driver = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    license_plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expRank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    desc_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    current_Lat = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false),
                    current_Lng = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false),
                    is_Busy = table.Column<bool>(type: "bit", nullable: false),
                    rate_Avg = table.Column<decimal>(type: "decimal(2,1)", precision: 2, scale: 1, nullable: false),
                    total_Orders = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.id_Driver);
                    table.ForeignKey(
                        name: "FK_Driver_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    id_Noti = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    orderId = table.Column<int>(type: "int", nullable: true),
                    is_Read = table.Column<bool>(type: "bit", nullable: false),
                    created_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.id_Noti);
                    table.ForeignKey(
                        name: "FK_Notification_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemLog",
                columns: table => new
                {
                    id_Log = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    entity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    entity_Id = table.Column<int>(type: "int", nullable: false),
                    old_Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    new_Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ip_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLog", x => x.id_Log);
                    table.ForeignKey(
                        name: "FK_SystemLog_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    id_Voucher = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    used = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.id_Voucher);
                    table.ForeignKey(
                        name: "FK_Voucher_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart_Food",
                columns: table => new
                {
                    id_CartFood = table.Column<int>(type: "int", nullable: false),
                    id_Cart = table.Column<int>(type: "int", nullable: false),
                    id_Food = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart_Food", x => x.id_CartFood);
                    table.ForeignKey(
                        name: "FK_Cart_Food_Cart_id_Cart",
                        column: x => x.id_Cart,
                        principalTable: "Cart",
                        principalColumn: "id_Cart",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart_Food_Food_id_Food",
                        column: x => x.id_Food,
                        principalTable: "Food",
                        principalColumn: "id_Food",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id_Order = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    id_Restaurant = table.Column<int>(type: "int", nullable: false),
                    id_Driver = table.Column<int>(type: "int", nullable: true),
                    id_Voucher = table.Column<int>(type: "int", nullable: true),
                    order_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    delivery_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    delivery_Lat = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
                    delivery_Lng = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    food_Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    shippingFee = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    discount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    finalTotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    paymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cancel_Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_At = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id_Order);
                    table.ForeignKey(
                        name: "FK_Orders_Driver_id_Driver",
                        column: x => x.id_Driver,
                        principalTable: "Driver",
                        principalColumn: "id_Driver");
                    table.ForeignKey(
                        name: "FK_Orders_Restaurant_id_Restaurant",
                        column: x => x.id_Restaurant,
                        principalTable: "Restaurant",
                        principalColumn: "id_Restaurant",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Voucher_id_Voucher",
                        column: x => x.id_Voucher,
                        principalTable: "Voucher",
                        principalColumn: "id_Voucher");
                });

            migrationBuilder.CreateTable(
                name: "Complaint",
                columns: table => new
                {
                    id_Complaint = table.Column<int>(type: "int", nullable: false),
                    id_Order = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    handled_By = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    received_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    resolved_At = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaint", x => x.id_Complaint);
                    table.ForeignKey(
                        name: "FK_Complaint_Orders_id_Order",
                        column: x => x.id_Order,
                        principalTable: "Orders",
                        principalColumn: "id_Order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaint_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Food",
                columns: table => new
                {
                    id_OrderFood = table.Column<int>(type: "int", nullable: false),
                    id_Order = table.Column<int>(type: "int", nullable: false),
                    id_Food = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    total_Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Food", x => x.id_OrderFood);
                    table.ForeignKey(
                        name: "FK_Order_Food_Food_id_Food",
                        column: x => x.id_Food,
                        principalTable: "Food",
                        principalColumn: "id_Food",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Food_Orders_id_Order",
                        column: x => x.id_Order,
                        principalTable: "Orders",
                        principalColumn: "id_Order",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Promotion",
                columns: table => new
                {
                    id_Order = table.Column<int>(type: "int", nullable: false),
                    id_Promo = table.Column<int>(type: "int", nullable: false),
                    discount_Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Promotion", x => new { x.id_Order, x.id_Promo });
                    table.ForeignKey(
                        name: "FK_Order_Promotion_Orders_id_Order",
                        column: x => x.id_Order,
                        principalTable: "Orders",
                        principalColumn: "id_Order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Promotion_Promotion_id_Promo",
                        column: x => x.id_Promo,
                        principalTable: "Promotion",
                        principalColumn: "id_Promo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    id_Transaction = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    id_Order = table.Column<int>(type: "int", nullable: false),
                    method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.id_Transaction);
                    table.ForeignKey(
                        name: "FK_PaymentMethod_Orders_id_Order",
                        column: x => x.id_Order,
                        principalTable: "Orders",
                        principalColumn: "id_Order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentMethod_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    id_Review = table.Column<int>(type: "int", nullable: false),
                    id_User = table.Column<int>(type: "int", nullable: false),
                    id_Order = table.Column<int>(type: "int", nullable: false),
                    id_Restaurant = table.Column<int>(type: "int", nullable: false),
                    food_rating = table.Column<float>(type: "real", nullable: false),
                    driver_rating = table.Column<float>(type: "real", nullable: false),
                    comment_ForRes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comment_ForShipper = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.id_Review);
                    table.ForeignKey(
                        name: "FK_Review_Orders_id_Order",
                        column: x => x.id_Order,
                        principalTable: "Orders",
                        principalColumn: "id_Order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Restaurant_id_Restaurant",
                        column: x => x.id_Restaurant,
                        principalTable: "Restaurant",
                        principalColumn: "id_Restaurant",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Users_id_User",
                        column: x => x.id_User,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review_Food",
                columns: table => new
                {
                    id_ReviewFood = table.Column<int>(type: "int", nullable: false),
                    id_Review = table.Column<int>(type: "int", nullable: false),
                    id_Food = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    video = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review_Food", x => x.id_ReviewFood);
                    table.ForeignKey(
                        name: "FK_Review_Food_Food_id_Food",
                        column: x => x.id_Food,
                        principalTable: "Food",
                        principalColumn: "id_Food",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Food_Review_id_Review",
                        column: x => x.id_Review,
                        principalTable: "Review",
                        principalColumn: "id_Review",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_id_User",
                table: "Cart",
                column: "id_User");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_Food_id_Cart",
                table: "Cart_Food",
                column: "id_Cart");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_Food_id_Food",
                table: "Cart_Food",
                column: "id_Food");

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_id_Order",
                table: "Complaint",
                column: "id_Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_id_User",
                table: "Complaint",
                column: "id_User");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_id_User",
                table: "Driver",
                column: "id_User",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Food_id_Category",
                table: "Food",
                column: "id_Category");

            migrationBuilder.CreateIndex(
                name: "IX_Food_id_Restaurant",
                table: "Food",
                column: "id_Restaurant");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_id_User",
                table: "Notification",
                column: "id_User");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Food_id_Food",
                table: "Order_Food",
                column: "id_Food");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Food_id_Order",
                table: "Order_Food",
                column: "id_Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Promotion_id_Promo",
                table: "Order_Promotion",
                column: "id_Promo");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_Driver",
                table: "Orders",
                column: "id_Driver");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_Restaurant",
                table: "Orders",
                column: "id_Restaurant");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_User",
                table: "Orders",
                column: "id_User");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_Voucher",
                table: "Orders",
                column: "id_Voucher");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_id_Order",
                table: "PaymentMethod",
                column: "id_Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_id_User",
                table: "PaymentMethod",
                column: "id_User");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_id_Restaurant",
                table: "Promotion",
                column: "id_Restaurant");

            migrationBuilder.CreateIndex(
                name: "IX_Review_id_Order",
                table: "Review",
                column: "id_Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_id_Restaurant",
                table: "Review",
                column: "id_Restaurant");

            migrationBuilder.CreateIndex(
                name: "IX_Review_id_User",
                table: "Review",
                column: "id_User");

            migrationBuilder.CreateIndex(
                name: "IX_Review_Food_id_Food",
                table: "Review_Food",
                column: "id_Food");

            migrationBuilder.CreateIndex(
                name: "IX_Review_Food_id_Review",
                table: "Review_Food",
                column: "id_Review");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLog_id_User",
                table: "SystemLog",
                column: "id_User");

            migrationBuilder.CreateIndex(
                name: "IX_Users_id_Role",
                table: "Users",
                column: "id_Role");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_id_User",
                table: "Voucher",
                column: "id_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart_Food");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Complaint");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Order_Food");

            migrationBuilder.DropTable(
                name: "Order_Promotion");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Review_Food");

            migrationBuilder.DropTable(
                name: "SystemLog");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
