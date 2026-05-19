using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Users_IdUser",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Food_Cart_IdCart",
                table: "Cart_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Food_Food_IdFood",
                table: "Cart_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Orders_IdOrder",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Users_IdUser",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_Users_IdUser",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Category_IdCategory",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Restaurant_IdRestaurant",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_IdUser",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Food_Food_IdFood",
                table: "Order_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Food_Orders_IdOrder",
                table: "Order_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Promotion_Orders_IdOrder",
                table: "Order_Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Promotion_Promotion_IdPromo",
                table: "Order_Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_IdAddress",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Driver_IdDriver",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_IdUser",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_Orders_IdOrder",
                table: "PaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_Users_IdUser",
                table: "PaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_Restaurant_IdRestaurant",
                table: "Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Role_RoleIdRole",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Orders_IdOrder",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Restaurant_IdRestaurant",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Users_IdUser",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Food_Food_IdFood",
                table: "Review_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Food_Review_IdReview",
                table: "Review_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemLog_Users_IdUser",
                table: "SystemLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Role_IdRole",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Users_IdUser",
                table: "Voucher");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Food_Image");

            migrationBuilder.DropTable(
                name: "Order_Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_RoleIdRole",
                table: "Restaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "UpdateBg",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdateBio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdRole",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "RoleIdRole",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "CanceledAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveredAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Voucher",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Used",
                table: "Voucher",
                newName: "used");

            migrationBuilder.RenameColumn(
                name: "Expiry",
                table: "Voucher",
                newName: "expiry");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Voucher",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Voucher",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "IdVoucher",
                table: "Voucher",
                newName: "id_Voucher");

            migrationBuilder.RenameIndex(
                name: "IX_Voucher_IdUser",
                table: "Voucher",
                newName: "IX_Voucher_id_User");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Users",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Users",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "LastOnline",
                table: "Users",
                newName: "lastOnline");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "fullName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "Users",
                newName: "avatar");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Users",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "IdRole",
                table: "Users",
                newName: "id_Role");

            migrationBuilder.RenameColumn(
                name: "CurrentLng",
                table: "Users",
                newName: "current_Lng");

            migrationBuilder.RenameColumn(
                name: "CurrentLat",
                table: "Users",
                newName: "current_Lat");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "created_At");

            migrationBuilder.RenameColumn(
                name: "CancelRate",
                table: "Users",
                newName: "cancel_Rate");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Users",
                newName: "id_User");

            migrationBuilder.RenameIndex(
                name: "IX_Users_IdRole",
                table: "Users",
                newName: "IX_Users_id_Role");

            migrationBuilder.RenameColumn(
                name: "Entity",
                table: "SystemLog",
                newName: "entity");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "SystemLog",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "OldValue",
                table: "SystemLog",
                newName: "old_Value");

            migrationBuilder.RenameColumn(
                name: "NewValue",
                table: "SystemLog",
                newName: "new_Value");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "SystemLog",
                newName: "ip_Address");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "SystemLog",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "SystemLog",
                newName: "entity_Id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SystemLog",
                newName: "created_At");

            migrationBuilder.RenameColumn(
                name: "IdLog",
                table: "SystemLog",
                newName: "id_Log");

            migrationBuilder.RenameIndex(
                name: "IX_SystemLog_IdUser",
                table: "SystemLog",
                newName: "IX_SystemLog_id_User");

            migrationBuilder.RenameColumn(
                name: "Video",
                table: "Review_Food",
                newName: "video");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Review_Food",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Review_Food",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Review_Food",
                newName: "comment");

            migrationBuilder.RenameColumn(
                name: "IdReview",
                table: "Review_Food",
                newName: "id_Review");

            migrationBuilder.RenameColumn(
                name: "IdFood",
                table: "Review_Food",
                newName: "id_Food");

            migrationBuilder.RenameColumn(
                name: "IdReviewFood",
                table: "Review_Food",
                newName: "id_ReviewFood");

            migrationBuilder.RenameIndex(
                name: "IX_Review_Food_IdReview",
                table: "Review_Food",
                newName: "IX_Review_Food_id_Review");

            migrationBuilder.RenameIndex(
                name: "IX_Review_Food_IdFood",
                table: "Review_Food",
                newName: "IX_Review_Food_id_Food");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Review",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "IdRestaurant",
                table: "Review",
                newName: "id_Restaurant");

            migrationBuilder.RenameColumn(
                name: "IdOrder",
                table: "Review",
                newName: "id_Order");

            migrationBuilder.RenameColumn(
                name: "FoodRating",
                table: "Review",
                newName: "food_rating");

            migrationBuilder.RenameColumn(
                name: "DriverRating",
                table: "Review",
                newName: "driver_rating");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Review",
                newName: "created_At");

            migrationBuilder.RenameColumn(
                name: "CommentForShipper",
                table: "Review",
                newName: "comment_ForShipper");

            migrationBuilder.RenameColumn(
                name: "CommentForRes",
                table: "Review",
                newName: "comment_ForRes");

            migrationBuilder.RenameColumn(
                name: "IdReview",
                table: "Review",
                newName: "id_Review");

            migrationBuilder.RenameIndex(
                name: "IX_Review_IdUser",
                table: "Review",
                newName: "IX_Review_id_User");

            migrationBuilder.RenameIndex(
                name: "IX_Review_IdRestaurant",
                table: "Review",
                newName: "IX_Review_id_Restaurant");

            migrationBuilder.RenameIndex(
                name: "IX_Review_IdOrder",
                table: "Review",
                newName: "IX_Review_id_Order");

            migrationBuilder.RenameColumn(
                name: "OpenTime",
                table: "Restaurant",
                newName: "openTime");

            migrationBuilder.RenameColumn(
                name: "Lng",
                table: "Restaurant",
                newName: "lng");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "Restaurant",
                newName: "lat");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Restaurant",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Restaurant",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CloseTime",
                table: "Restaurant",
                newName: "closeTime");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Restaurant",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "NameRestaurant",
                table: "Restaurant",
                newName: "name_Restaurant");

            migrationBuilder.RenameColumn(
                name: "IdRestaurant",
                table: "Restaurant",
                newName: "id_Restaurant");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Promotion",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Promotion",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "UsedCount",
                table: "Promotion",
                newName: "used_Count");

            migrationBuilder.RenameColumn(
                name: "UsageLimit",
                table: "Promotion",
                newName: "usage_Limit");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Promotion",
                newName: "start_Date");

            migrationBuilder.RenameColumn(
                name: "MinOrderValue",
                table: "Promotion",
                newName: "min_OrderValue");

            migrationBuilder.RenameColumn(
                name: "MaxDiscount",
                table: "Promotion",
                newName: "max_Discount");

            migrationBuilder.RenameColumn(
                name: "IdRestaurant",
                table: "Promotion",
                newName: "id_Restaurant");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Promotion",
                newName: "end_Date");

            migrationBuilder.RenameColumn(
                name: "IdPromo",
                table: "Promotion",
                newName: "id_Promo");

            migrationBuilder.RenameIndex(
                name: "IX_Promotion_IdRestaurant",
                table: "Promotion",
                newName: "IX_Promotion_id_Restaurant");

            migrationBuilder.RenameColumn(
                name: "Method",
                table: "PaymentMethod",
                newName: "method");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "PaymentMethod",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "PaymentMethod",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "IdOrder",
                table: "PaymentMethod",
                newName: "id_Order");

            migrationBuilder.RenameColumn(
                name: "IdTransaction",
                table: "PaymentMethod",
                newName: "id_Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentMethod_IdUser",
                table: "PaymentMethod",
                newName: "IX_PaymentMethod_id_User");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentMethod_IdOrder",
                table: "PaymentMethod",
                newName: "IX_PaymentMethod_id_Order");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "ShippingFee",
                table: "Orders",
                newName: "shippingFee");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Orders",
                newName: "paymentMethod");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Orders",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "FinalTotal",
                table: "Orders",
                newName: "finalTotal");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Orders",
                newName: "discount");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Orders",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "IdDriver",
                table: "Orders",
                newName: "id_Driver");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Orders",
                newName: "created_At");

            migrationBuilder.RenameColumn(
                name: "IdOrder",
                table: "Orders",
                newName: "id_Order");

            migrationBuilder.RenameColumn(
                name: "IdAddress",
                table: "Orders",
                newName: "id_Restaurant");

            migrationBuilder.RenameColumn(
                name: "DeliveringAt",
                table: "Orders",
                newName: "updated_At");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_IdUser",
                table: "Orders",
                newName: "IX_Orders_id_User");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_IdDriver",
                table: "Orders",
                newName: "IX_Orders_id_Driver");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_IdAddress",
                table: "Orders",
                newName: "IX_Orders_id_Restaurant");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "Order_Promotion",
                newName: "discount_Amount");

            migrationBuilder.RenameColumn(
                name: "IdPromo",
                table: "Order_Promotion",
                newName: "id_Promo");

            migrationBuilder.RenameColumn(
                name: "IdOrder",
                table: "Order_Promotion",
                newName: "id_Order");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Promotion_IdPromo",
                table: "Order_Promotion",
                newName: "IX_Order_Promotion_id_Promo");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Order_Food",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Order_Food",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Order_Food",
                newName: "unit_Price");

            migrationBuilder.RenameColumn(
                name: "IdOrder",
                table: "Order_Food",
                newName: "id_Order");

            migrationBuilder.RenameColumn(
                name: "IdFood",
                table: "Order_Food",
                newName: "id_Food");

            migrationBuilder.RenameColumn(
                name: "IdOrderFood",
                table: "Order_Food",
                newName: "id_OrderFood");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Food_IdOrder",
                table: "Order_Food",
                newName: "IX_Order_Food_id_Order");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Food_IdFood",
                table: "Order_Food",
                newName: "IX_Order_Food_id_Food");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Notification",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Notification",
                newName: "orderId");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Notification",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "Notification",
                newName: "is_Read");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Notification",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Notification",
                newName: "created_At");

            migrationBuilder.RenameColumn(
                name: "IdNoti",
                table: "Notification",
                newName: "id_Noti");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_IdUser",
                table: "Notification",
                newName: "IX_Notification_id_User");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Food",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Food",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Food",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Food",
                newName: "discount");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Food",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "PrepTime",
                table: "Food",
                newName: "prep_Time");

            migrationBuilder.RenameColumn(
                name: "IdRestaurant",
                table: "Food",
                newName: "id_Restaurant");

            migrationBuilder.RenameColumn(
                name: "IdCategory",
                table: "Food",
                newName: "id_Category");

            migrationBuilder.RenameColumn(
                name: "CookCount",
                table: "Food",
                newName: "cook_Count");

            migrationBuilder.RenameColumn(
                name: "IdFood",
                table: "Food",
                newName: "id_Food");

            migrationBuilder.RenameIndex(
                name: "IX_Food_IdRestaurant",
                table: "Food",
                newName: "IX_Food_id_Restaurant");

            migrationBuilder.RenameIndex(
                name: "IX_Food_IdCategory",
                table: "Food",
                newName: "IX_Food_id_Category");

            migrationBuilder.RenameColumn(
                name: "ExpRank",
                table: "Driver",
                newName: "expRank");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Driver",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "TotalOrders",
                table: "Driver",
                newName: "total_Orders");

            migrationBuilder.RenameColumn(
                name: "RateAvg",
                table: "Driver",
                newName: "rate_Avg");

            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "Driver",
                newName: "license_plate");

            migrationBuilder.RenameColumn(
                name: "IsBusy",
                table: "Driver",
                newName: "is_Busy");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Driver",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "DescStatus",
                table: "Driver",
                newName: "desc_Status");

            migrationBuilder.RenameColumn(
                name: "CurrentLng",
                table: "Driver",
                newName: "current_Lng");

            migrationBuilder.RenameColumn(
                name: "CurrentLat",
                table: "Driver",
                newName: "current_Lat");

            migrationBuilder.RenameColumn(
                name: "IdDriver",
                table: "Driver",
                newName: "id_Driver");

            migrationBuilder.RenameIndex(
                name: "IX_Driver_IdUser",
                table: "Driver",
                newName: "IX_Driver_id_User");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Complaint",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Complaint",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Complaint",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Complaint",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "ResolvedAt",
                table: "Complaint",
                newName: "resolved_At");

            migrationBuilder.RenameColumn(
                name: "ReceivedAt",
                table: "Complaint",
                newName: "received_At");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Complaint",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "IdOrder",
                table: "Complaint",
                newName: "id_Order");

            migrationBuilder.RenameColumn(
                name: "HandledBy",
                table: "Complaint",
                newName: "handled_By");

            migrationBuilder.RenameColumn(
                name: "IdComplaint",
                table: "Complaint",
                newName: "id_Complaint");

            migrationBuilder.RenameIndex(
                name: "IX_Complaint_IdUser",
                table: "Complaint",
                newName: "IX_Complaint_id_User");

            migrationBuilder.RenameIndex(
                name: "IX_Complaint_IdOrder",
                table: "Complaint",
                newName: "IX_Complaint_id_Order");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Category",
                newName: "icon");

            migrationBuilder.RenameColumn(
                name: "IdCategory",
                table: "Category",
                newName: "id_Category");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Cart_Food",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Cart_Food",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "IdFood",
                table: "Cart_Food",
                newName: "id_Food");

            migrationBuilder.RenameColumn(
                name: "IdCart",
                table: "Cart_Food",
                newName: "id_Cart");

            migrationBuilder.RenameColumn(
                name: "IdCartFood",
                table: "Cart_Food",
                newName: "id_CartFood");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_Food_IdFood",
                table: "Cart_Food",
                newName: "IX_Cart_Food_id_Food");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_Food_IdCart",
                table: "Cart_Food",
                newName: "IX_Cart_Food_id_Cart");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Cart",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Cart",
                newName: "update_At");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Cart",
                newName: "id_User");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Cart",
                newName: "created_At");

            migrationBuilder.RenameColumn(
                name: "IdCart",
                table: "Cart",
                newName: "id_Cart");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_IdUser",
                table: "Cart",
                newName: "IX_Cart_id_User");

            migrationBuilder.RenameColumn(
                name: "IdRole",
                table: "Roles",
                newName: "id_Role");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "role_Name");

            migrationBuilder.AlterColumn<decimal>(
                name: "value",
                table: "Voucher",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "id_Voucher",
                table: "Voucher",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "fullName",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_Lng",
                table: "Users",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "current_Lat",
                table: "Users",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_User",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id_Log",
                table: "SystemLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id_ReviewFood",
                table: "Review_Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id_Review",
                table: "Review",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "lng",
                table: "Restaurant",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "lat",
                table: "Restaurant",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "id_Restaurant",
                table: "Restaurant",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "value",
                table: "Promotion",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "min_OrderValue",
                table: "Promotion",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_Discount",
                table: "Promotion",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "id_Promo",
                table: "Promotion",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "PaymentMethod",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "id_Transaction",
                table: "PaymentMethod",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "shippingFee",
                table: "Orders",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "Orders",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "finalTotal",
                table: "Orders",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "Orders",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "id_Order",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "cancel_Reason",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "delivery_Address",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "delivery_Lat",
                table: "Orders",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "delivery_Lng",
                table: "Orders",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "food_Amount",
                table: "Orders",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "id_Voucher",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "order_Code",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "paymentStatus",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_Amount",
                table: "Order_Promotion",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "Order_Food",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_Price",
                table: "Order_Food",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "id_OrderFood",
                table: "Order_Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "total_Price",
                table: "Order_Food",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "id_Noti",
                table: "Notification",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "Food",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "Food",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_Food",
                table: "Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "rate_Avg",
                table: "Driver",
                type: "decimal(2,1)",
                precision: 2,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_Lng",
                table: "Driver",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_Lat",
                table: "Driver",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "id_Driver",
                table: "Driver",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id_Complaint",
                table: "Complaint",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id_Category",
                table: "Category",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id_CartFood",
                table: "Cart_Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "Cart_Food",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total",
                table: "Cart_Food",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "id_Cart",
                table: "Cart",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id_Role",
                table: "Roles",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "id_Role");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_Voucher",
                table: "Orders",
                column: "id_Voucher");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Users_id_User",
                table: "Cart",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Food_Cart_id_Cart",
                table: "Cart_Food",
                column: "id_Cart",
                principalTable: "Cart",
                principalColumn: "id_Cart",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Food_Food_id_Food",
                table: "Cart_Food",
                column: "id_Food",
                principalTable: "Food",
                principalColumn: "id_Food",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Orders_id_Order",
                table: "Complaint",
                column: "id_Order",
                principalTable: "Orders",
                principalColumn: "id_Order",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Users_id_User",
                table: "Complaint",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_Users_id_User",
                table: "Driver",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Category_id_Category",
                table: "Food",
                column: "id_Category",
                principalTable: "Category",
                principalColumn: "id_Category",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Restaurant_id_Restaurant",
                table: "Food",
                column: "id_Restaurant",
                principalTable: "Restaurant",
                principalColumn: "id_Restaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_id_User",
                table: "Notification",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Food_Food_id_Food",
                table: "Order_Food",
                column: "id_Food",
                principalTable: "Food",
                principalColumn: "id_Food",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Food_Orders_id_Order",
                table: "Order_Food",
                column: "id_Order",
                principalTable: "Orders",
                principalColumn: "id_Order",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Promotion_Orders_id_Order",
                table: "Order_Promotion",
                column: "id_Order",
                principalTable: "Orders",
                principalColumn: "id_Order",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Promotion_Promotion_id_Promo",
                table: "Order_Promotion",
                column: "id_Promo",
                principalTable: "Promotion",
                principalColumn: "id_Promo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Driver_id_Driver",
                table: "Orders",
                column: "id_Driver",
                principalTable: "Driver",
                principalColumn: "id_Driver");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurant_id_Restaurant",
                table: "Orders",
                column: "id_Restaurant",
                principalTable: "Restaurant",
                principalColumn: "id_Restaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_id_User",
                table: "Orders",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Voucher_id_Voucher",
                table: "Orders",
                column: "id_Voucher",
                principalTable: "Voucher",
                principalColumn: "id_Voucher");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_Orders_id_Order",
                table: "PaymentMethod",
                column: "id_Order",
                principalTable: "Orders",
                principalColumn: "id_Order",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_Users_id_User",
                table: "PaymentMethod",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_Restaurant_id_Restaurant",
                table: "Promotion",
                column: "id_Restaurant",
                principalTable: "Restaurant",
                principalColumn: "id_Restaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Orders_id_Order",
                table: "Review",
                column: "id_Order",
                principalTable: "Orders",
                principalColumn: "id_Order",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Restaurant_id_Restaurant",
                table: "Review",
                column: "id_Restaurant",
                principalTable: "Restaurant",
                principalColumn: "id_Restaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Users_id_User",
                table: "Review",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Food_Food_id_Food",
                table: "Review_Food",
                column: "id_Food",
                principalTable: "Food",
                principalColumn: "id_Food",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Food_Review_id_Review",
                table: "Review_Food",
                column: "id_Review",
                principalTable: "Review",
                principalColumn: "id_Review",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemLog_Users_id_User",
                table: "SystemLog",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_id_Role",
                table: "Users",
                column: "id_Role",
                principalTable: "Roles",
                principalColumn: "id_Role",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Users_id_User",
                table: "Voucher",
                column: "id_User",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Users_id_User",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Food_Cart_id_Cart",
                table: "Cart_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Food_Food_id_Food",
                table: "Cart_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Orders_id_Order",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Users_id_User",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_Users_id_User",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Category_id_Category",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Restaurant_id_Restaurant",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_id_User",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Food_Food_id_Food",
                table: "Order_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Food_Orders_id_Order",
                table: "Order_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Promotion_Orders_id_Order",
                table: "Order_Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Promotion_Promotion_id_Promo",
                table: "Order_Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Driver_id_Driver",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurant_id_Restaurant",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_id_User",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Voucher_id_Voucher",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_Orders_id_Order",
                table: "PaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_Users_id_User",
                table: "PaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_Restaurant_id_Restaurant",
                table: "Promotion");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Orders_id_Order",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Restaurant_id_Restaurant",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Users_id_User",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Food_Food_id_Food",
                table: "Review_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Food_Review_id_Review",
                table: "Review_Food");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemLog_Users_id_User",
                table: "SystemLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_id_Role",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Users_id_User",
                table: "Voucher");

            migrationBuilder.DropIndex(
                name: "IX_Orders_id_Voucher",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "cancel_Reason",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "delivery_Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "delivery_Lat",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "delivery_Lng",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "food_Amount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "id_Voucher",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "order_Code",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "paymentStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "total_Price",
                table: "Order_Food");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Cart_Food");

            migrationBuilder.DropColumn(
                name: "total",
                table: "Cart_Food");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Voucher",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "used",
                table: "Voucher",
                newName: "Used");

            migrationBuilder.RenameColumn(
                name: "expiry",
                table: "Voucher",
                newName: "Expiry");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Voucher",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Voucher",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "id_Voucher",
                table: "Voucher",
                newName: "IdVoucher");

            migrationBuilder.RenameIndex(
                name: "IX_Voucher_id_User",
                table: "Voucher",
                newName: "IX_Voucher_IdUser");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Users",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Users",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "lastOnline",
                table: "Users",
                newName: "LastOnline");

            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "Users",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "avatar",
                table: "Users",
                newName: "Avatar");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Users",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id_Role",
                table: "Users",
                newName: "IdRole");

            migrationBuilder.RenameColumn(
                name: "current_Lng",
                table: "Users",
                newName: "CurrentLng");

            migrationBuilder.RenameColumn(
                name: "current_Lat",
                table: "Users",
                newName: "CurrentLat");

            migrationBuilder.RenameColumn(
                name: "created_At",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cancel_Rate",
                table: "Users",
                newName: "CancelRate");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Users",
                newName: "IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_id_Role",
                table: "Users",
                newName: "IX_Users_IdRole");

            migrationBuilder.RenameColumn(
                name: "entity",
                table: "SystemLog",
                newName: "Entity");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "SystemLog",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "old_Value",
                table: "SystemLog",
                newName: "OldValue");

            migrationBuilder.RenameColumn(
                name: "new_Value",
                table: "SystemLog",
                newName: "NewValue");

            migrationBuilder.RenameColumn(
                name: "ip_Address",
                table: "SystemLog",
                newName: "IpAddress");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "SystemLog",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "entity_Id",
                table: "SystemLog",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "created_At",
                table: "SystemLog",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id_Log",
                table: "SystemLog",
                newName: "IdLog");

            migrationBuilder.RenameIndex(
                name: "IX_SystemLog_id_User",
                table: "SystemLog",
                newName: "IX_SystemLog_IdUser");

            migrationBuilder.RenameColumn(
                name: "video",
                table: "Review_Food",
                newName: "Video");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Review_Food",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Review_Food",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Review_Food",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "id_Review",
                table: "Review_Food",
                newName: "IdReview");

            migrationBuilder.RenameColumn(
                name: "id_Food",
                table: "Review_Food",
                newName: "IdFood");

            migrationBuilder.RenameColumn(
                name: "id_ReviewFood",
                table: "Review_Food",
                newName: "IdReviewFood");

            migrationBuilder.RenameIndex(
                name: "IX_Review_Food_id_Review",
                table: "Review_Food",
                newName: "IX_Review_Food_IdReview");

            migrationBuilder.RenameIndex(
                name: "IX_Review_Food_id_Food",
                table: "Review_Food",
                newName: "IX_Review_Food_IdFood");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Review",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "id_Restaurant",
                table: "Review",
                newName: "IdRestaurant");

            migrationBuilder.RenameColumn(
                name: "id_Order",
                table: "Review",
                newName: "IdOrder");

            migrationBuilder.RenameColumn(
                name: "food_rating",
                table: "Review",
                newName: "FoodRating");

            migrationBuilder.RenameColumn(
                name: "driver_rating",
                table: "Review",
                newName: "DriverRating");

            migrationBuilder.RenameColumn(
                name: "created_At",
                table: "Review",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "comment_ForShipper",
                table: "Review",
                newName: "CommentForShipper");

            migrationBuilder.RenameColumn(
                name: "comment_ForRes",
                table: "Review",
                newName: "CommentForRes");

            migrationBuilder.RenameColumn(
                name: "id_Review",
                table: "Review",
                newName: "IdReview");

            migrationBuilder.RenameIndex(
                name: "IX_Review_id_User",
                table: "Review",
                newName: "IX_Review_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Review_id_Restaurant",
                table: "Review",
                newName: "IX_Review_IdRestaurant");

            migrationBuilder.RenameIndex(
                name: "IX_Review_id_Order",
                table: "Review",
                newName: "IX_Review_IdOrder");

            migrationBuilder.RenameColumn(
                name: "openTime",
                table: "Restaurant",
                newName: "OpenTime");

            migrationBuilder.RenameColumn(
                name: "lng",
                table: "Restaurant",
                newName: "Lng");

            migrationBuilder.RenameColumn(
                name: "lat",
                table: "Restaurant",
                newName: "Lat");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Restaurant",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Restaurant",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "closeTime",
                table: "Restaurant",
                newName: "CloseTime");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Restaurant",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "name_Restaurant",
                table: "Restaurant",
                newName: "NameRestaurant");

            migrationBuilder.RenameColumn(
                name: "id_Restaurant",
                table: "Restaurant",
                newName: "IdRestaurant");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Promotion",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Promotion",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "used_Count",
                table: "Promotion",
                newName: "UsedCount");

            migrationBuilder.RenameColumn(
                name: "usage_Limit",
                table: "Promotion",
                newName: "UsageLimit");

            migrationBuilder.RenameColumn(
                name: "start_Date",
                table: "Promotion",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "min_OrderValue",
                table: "Promotion",
                newName: "MinOrderValue");

            migrationBuilder.RenameColumn(
                name: "max_Discount",
                table: "Promotion",
                newName: "MaxDiscount");

            migrationBuilder.RenameColumn(
                name: "id_Restaurant",
                table: "Promotion",
                newName: "IdRestaurant");

            migrationBuilder.RenameColumn(
                name: "end_Date",
                table: "Promotion",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "id_Promo",
                table: "Promotion",
                newName: "IdPromo");

            migrationBuilder.RenameIndex(
                name: "IX_Promotion_id_Restaurant",
                table: "Promotion",
                newName: "IX_Promotion_IdRestaurant");

            migrationBuilder.RenameColumn(
                name: "method",
                table: "PaymentMethod",
                newName: "Method");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "PaymentMethod",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "PaymentMethod",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "id_Order",
                table: "PaymentMethod",
                newName: "IdOrder");

            migrationBuilder.RenameColumn(
                name: "id_Transaction",
                table: "PaymentMethod",
                newName: "IdTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentMethod_id_User",
                table: "PaymentMethod",
                newName: "IX_PaymentMethod_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentMethod_id_Order",
                table: "PaymentMethod",
                newName: "IX_PaymentMethod_IdOrder");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "shippingFee",
                table: "Orders",
                newName: "ShippingFee");

            migrationBuilder.RenameColumn(
                name: "paymentMethod",
                table: "Orders",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Orders",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "finalTotal",
                table: "Orders",
                newName: "FinalTotal");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "Orders",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Orders",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "id_Driver",
                table: "Orders",
                newName: "IdDriver");

            migrationBuilder.RenameColumn(
                name: "created_At",
                table: "Orders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id_Order",
                table: "Orders",
                newName: "IdOrder");

            migrationBuilder.RenameColumn(
                name: "updated_At",
                table: "Orders",
                newName: "DeliveringAt");

            migrationBuilder.RenameColumn(
                name: "id_Restaurant",
                table: "Orders",
                newName: "IdAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_id_User",
                table: "Orders",
                newName: "IX_Orders_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_id_Restaurant",
                table: "Orders",
                newName: "IX_Orders_IdAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_id_Driver",
                table: "Orders",
                newName: "IX_Orders_IdDriver");

            migrationBuilder.RenameColumn(
                name: "discount_Amount",
                table: "Order_Promotion",
                newName: "DiscountAmount");

            migrationBuilder.RenameColumn(
                name: "id_Promo",
                table: "Order_Promotion",
                newName: "IdPromo");

            migrationBuilder.RenameColumn(
                name: "id_Order",
                table: "Order_Promotion",
                newName: "IdOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Promotion_id_Promo",
                table: "Order_Promotion",
                newName: "IX_Order_Promotion_IdPromo");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Order_Food",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Order_Food",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "unit_Price",
                table: "Order_Food",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "id_Order",
                table: "Order_Food",
                newName: "IdOrder");

            migrationBuilder.RenameColumn(
                name: "id_Food",
                table: "Order_Food",
                newName: "IdFood");

            migrationBuilder.RenameColumn(
                name: "id_OrderFood",
                table: "Order_Food",
                newName: "IdOrderFood");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Food_id_Order",
                table: "Order_Food",
                newName: "IX_Order_Food_IdOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Food_id_Food",
                table: "Order_Food",
                newName: "IX_Order_Food_IdFood");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Notification",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "Notification",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "Notification",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "is_Read",
                table: "Notification",
                newName: "IsRead");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Notification",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "created_At",
                table: "Notification",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id_Noti",
                table: "Notification",
                newName: "IdNoti");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_id_User",
                table: "Notification",
                newName: "IX_Notification_IdUser");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Food",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Food",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Food",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "Food",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Food",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "prep_Time",
                table: "Food",
                newName: "PrepTime");

            migrationBuilder.RenameColumn(
                name: "id_Restaurant",
                table: "Food",
                newName: "IdRestaurant");

            migrationBuilder.RenameColumn(
                name: "id_Category",
                table: "Food",
                newName: "IdCategory");

            migrationBuilder.RenameColumn(
                name: "cook_Count",
                table: "Food",
                newName: "CookCount");

            migrationBuilder.RenameColumn(
                name: "id_Food",
                table: "Food",
                newName: "IdFood");

            migrationBuilder.RenameIndex(
                name: "IX_Food_id_Restaurant",
                table: "Food",
                newName: "IX_Food_IdRestaurant");

            migrationBuilder.RenameIndex(
                name: "IX_Food_id_Category",
                table: "Food",
                newName: "IX_Food_IdCategory");

            migrationBuilder.RenameColumn(
                name: "expRank",
                table: "Driver",
                newName: "ExpRank");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Driver",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "total_Orders",
                table: "Driver",
                newName: "TotalOrders");

            migrationBuilder.RenameColumn(
                name: "rate_Avg",
                table: "Driver",
                newName: "RateAvg");

            migrationBuilder.RenameColumn(
                name: "license_plate",
                table: "Driver",
                newName: "LicensePlate");

            migrationBuilder.RenameColumn(
                name: "is_Busy",
                table: "Driver",
                newName: "IsBusy");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Driver",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "desc_Status",
                table: "Driver",
                newName: "DescStatus");

            migrationBuilder.RenameColumn(
                name: "current_Lng",
                table: "Driver",
                newName: "CurrentLng");

            migrationBuilder.RenameColumn(
                name: "current_Lat",
                table: "Driver",
                newName: "CurrentLat");

            migrationBuilder.RenameColumn(
                name: "id_Driver",
                table: "Driver",
                newName: "IdDriver");

            migrationBuilder.RenameIndex(
                name: "IX_Driver_id_User",
                table: "Driver",
                newName: "IX_Driver_IdUser");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Complaint",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Complaint",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Complaint",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Complaint",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "resolved_At",
                table: "Complaint",
                newName: "ResolvedAt");

            migrationBuilder.RenameColumn(
                name: "received_At",
                table: "Complaint",
                newName: "ReceivedAt");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Complaint",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "id_Order",
                table: "Complaint",
                newName: "IdOrder");

            migrationBuilder.RenameColumn(
                name: "handled_By",
                table: "Complaint",
                newName: "HandledBy");

            migrationBuilder.RenameColumn(
                name: "id_Complaint",
                table: "Complaint",
                newName: "IdComplaint");

            migrationBuilder.RenameIndex(
                name: "IX_Complaint_id_User",
                table: "Complaint",
                newName: "IX_Complaint_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Complaint_id_Order",
                table: "Complaint",
                newName: "IX_Complaint_IdOrder");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Category",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "icon",
                table: "Category",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "id_Category",
                table: "Category",
                newName: "IdCategory");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Cart_Food",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Cart_Food",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "id_Food",
                table: "Cart_Food",
                newName: "IdFood");

            migrationBuilder.RenameColumn(
                name: "id_Cart",
                table: "Cart_Food",
                newName: "IdCart");

            migrationBuilder.RenameColumn(
                name: "id_CartFood",
                table: "Cart_Food",
                newName: "IdCartFood");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_Food_id_Food",
                table: "Cart_Food",
                newName: "IX_Cart_Food_IdFood");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_Food_id_Cart",
                table: "Cart_Food",
                newName: "IX_Cart_Food_IdCart");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "Cart",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "update_At",
                table: "Cart",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "id_User",
                table: "Cart",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "created_At",
                table: "Cart",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id_Cart",
                table: "Cart",
                newName: "IdCart");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_id_User",
                table: "Cart",
                newName: "IX_Cart_IdUser");

            migrationBuilder.RenameColumn(
                name: "id_Role",
                table: "Role",
                newName: "IdRole");

            migrationBuilder.RenameColumn(
                name: "role_Name",
                table: "Role",
                newName: "Name");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Voucher",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IdVoucher",
                table: "Voucher",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Phone",
                keyValue: null,
                column: "Phone",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "FullName",
                keyValue: null,
                column: "FullName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: null,
                column: "Email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Address",
                keyValue: null,
                column: "Address",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLng",
                table: "Users",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldPrecision: 10,
                oldScale: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLat",
                table: "Users",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldPrecision: 10,
                oldScale: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdUser",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBg",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UpdateBio",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "IdLog",
                table: "SystemLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdReviewFood",
                table: "Review_Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdReview",
                table: "Review",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Lng",
                table: "Restaurant",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldPrecision: 10,
                oldScale: 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "Lat",
                table: "Restaurant",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldPrecision: 10,
                oldScale: 7);

            migrationBuilder.AlterColumn<int>(
                name: "IdRestaurant",
                table: "Restaurant",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "IdRole",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleIdRole",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Promotion",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinOrderValue",
                table: "Promotion",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxDiscount",
                table: "Promotion",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IdPromo",
                table: "Promotion",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PaymentMethod",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IdTransaction",
                table: "PaymentMethod",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Note",
                keyValue: null,
                column: "Note",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Orders",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalTotal",
                table: "Orders",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Orders",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IdOrder",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledAt",
                table: "Orders",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "Orders",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredAt",
                table: "Orders",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Order_Promotion",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "Order_Food",
                keyColumn: "Note",
                keyValue: null,
                column: "Note",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Order_Food",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Order_Food",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IdOrderFood",
                table: "Order_Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdNoti",
                table: "Notification",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Food",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Food",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdFood",
                table: "Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "Food",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "RateAvg",
                table: "Driver",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldPrecision: 2,
                oldScale: 1);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLng",
                table: "Driver",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldPrecision: 10,
                oldScale: 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLat",
                table: "Driver",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldPrecision: 10,
                oldScale: 7);

            migrationBuilder.AlterColumn<int>(
                name: "IdDriver",
                table: "Driver",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdComplaint",
                table: "Complaint",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdCategory",
                table: "Category",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdCartFood",
                table: "Cart_Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdCart",
                table: "Cart",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "IdRole",
                table: "Role",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Role",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "IdRole");

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    IdAddress = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    AddressDetail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Lat = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Lng = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.IdAddress);
                    table.ForeignKey(
                        name: "FK_Address_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Food_Image",
                columns: table => new
                {
                    IdFoodimage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdFood = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food_Image", x => x.IdFoodimage);
                    table.ForeignKey(
                        name: "FK_Food_Image_Food_IdFood",
                        column: x => x.IdFood,
                        principalTable: "Food",
                        principalColumn: "IdFood",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Order_Restaurant",
                columns: table => new
                {
                    IdOrder = table.Column<int>(type: "int", nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false),
                    ShipFee = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Restaurant", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_Order_Restaurant_Orders_IdOrder",
                        column: x => x.IdOrder,
                        principalTable: "Orders",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Restaurant_Restaurant_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurant",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_RoleIdRole",
                table: "Restaurant",
                column: "RoleIdRole");

            migrationBuilder.CreateIndex(
                name: "IX_Address_IdUser",
                table: "Address",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Food_Image_IdFood",
                table: "Food_Image",
                column: "IdFood");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Restaurant_IdRestaurant",
                table: "Order_Restaurant",
                column: "IdRestaurant");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Users_IdUser",
                table: "Cart",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Food_Cart_IdCart",
                table: "Cart_Food",
                column: "IdCart",
                principalTable: "Cart",
                principalColumn: "IdCart",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Food_Food_IdFood",
                table: "Cart_Food",
                column: "IdFood",
                principalTable: "Food",
                principalColumn: "IdFood",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Orders_IdOrder",
                table: "Complaint",
                column: "IdOrder",
                principalTable: "Orders",
                principalColumn: "IdOrder",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Users_IdUser",
                table: "Complaint",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_Users_IdUser",
                table: "Driver",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Category_IdCategory",
                table: "Food",
                column: "IdCategory",
                principalTable: "Category",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Restaurant_IdRestaurant",
                table: "Food",
                column: "IdRestaurant",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_IdUser",
                table: "Notification",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Food_Food_IdFood",
                table: "Order_Food",
                column: "IdFood",
                principalTable: "Food",
                principalColumn: "IdFood",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Food_Orders_IdOrder",
                table: "Order_Food",
                column: "IdOrder",
                principalTable: "Orders",
                principalColumn: "IdOrder",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Promotion_Orders_IdOrder",
                table: "Order_Promotion",
                column: "IdOrder",
                principalTable: "Orders",
                principalColumn: "IdOrder",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Promotion_Promotion_IdPromo",
                table: "Order_Promotion",
                column: "IdPromo",
                principalTable: "Promotion",
                principalColumn: "IdPromo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_IdAddress",
                table: "Orders",
                column: "IdAddress",
                principalTable: "Address",
                principalColumn: "IdAddress",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Driver_IdDriver",
                table: "Orders",
                column: "IdDriver",
                principalTable: "Driver",
                principalColumn: "IdDriver");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_IdUser",
                table: "Orders",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_Orders_IdOrder",
                table: "PaymentMethod",
                column: "IdOrder",
                principalTable: "Orders",
                principalColumn: "IdOrder",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_Users_IdUser",
                table: "PaymentMethod",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_Restaurant_IdRestaurant",
                table: "Promotion",
                column: "IdRestaurant",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Role_RoleIdRole",
                table: "Restaurant",
                column: "RoleIdRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Orders_IdOrder",
                table: "Review",
                column: "IdOrder",
                principalTable: "Orders",
                principalColumn: "IdOrder",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Restaurant_IdRestaurant",
                table: "Review",
                column: "IdRestaurant",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Users_IdUser",
                table: "Review",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Food_Food_IdFood",
                table: "Review_Food",
                column: "IdFood",
                principalTable: "Food",
                principalColumn: "IdFood",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Food_Review_IdReview",
                table: "Review_Food",
                column: "IdReview",
                principalTable: "Review",
                principalColumn: "IdReview",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemLog_Users_IdUser",
                table: "SystemLog",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Role_IdRole",
                table: "Users",
                column: "IdRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Users_IdUser",
                table: "Voucher",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
