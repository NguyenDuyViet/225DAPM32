--phan xoa databases neu da ton tai 
use master;
go
if exists (select name from sys.databases where name = 'QLCTGH_NHOM1')
begin
    alter database MonNgonTaiNha set single_user with rollback immediate;
    drop database MonNgonTaiNha;
    
end
--
create database MonNgonTaiNha
go
use MonNgonTaiNha
go

CREATE TABLE User (
    id_User INTEGER PRIMARY KEY,
    username VARCHAR(50),
    password VARCHAR(15),
    phone VARCHAR(15),
    email VARCHAR(50),
    avatar VARCHAR(255),
    status ENUM('active','inactive'),
    created_At DATETIME,
    lastOnline DATETIME,
    update_bio VARCHAR(255),
    update_avatar VARCHAR(255),
    update_bg VARCHAR(255),
    current_Lat DECIMAL(10,7),
    current_Lng DECIMAL(10,7),
    cancel_Rate FLOAT
);

CREATE TABLE Address (
    id_Address INTEGER PRIMARY KEY,
    id_User INTEGER,
    name VARCHAR(45),
    phone VARCHAR(15),
    address VARCHAR(255),
    lat DECIMAL(10,7),
    lng DECIMAL(10,7),
    note VARCHAR(255),
    is_Default BOOLEAN,
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE User_Address (
    id_Address INTEGER,
    id_User INTEGER,
    PRIMARY KEY (id_Address, id_User),
    FOREIGN KEY (id_Address) REFERENCES Address(id_Address),
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE Notification (
    id_Noti INTEGER PRIMARY KEY,
    id_User INTEGER,
    title VARCHAR(100),
    body TEXT,
    orderId INTEGER,
    is_Read BOOLEAN,
    created_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE User_Notification (
    id_Noti INTEGER,
    id_User INTEGER,
    PRIMARY KEY (id_Noti, id_User),
    FOREIGN KEY (id_Noti) REFERENCES Notification(id_Noti),
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE Voucher (
    id_Voucher INTEGER PRIMARY KEY,
    code VARCHAR(20),
    id_User INTEGER,
    value DECIMAL(10,2),
    expiry DATETIME,
    used BOOLEAN,
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE User_Voucher (
    id_Voucher INTEGER,
    id_User INTEGER,
    PRIMARY KEY (id_Voucher, id_User),
    FOREIGN KEY (id_Voucher) REFERENCES Voucher(id_Voucher),
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE Restaurant (
    id_Restaurant INTEGER PRIMARY KEY,
    name_Restaurant NVARCHAR(100),
    description TEXT,
    image NVARCHAR(255),
    address NVARCHAR(255),
    openTime TIME,
    closeTime TIME,
    lat DECIMAL(10,7),
    lng DECIMAL(10,7)
);

CREATE TABLE Category (
    id_Category INTEGER PRIMARY KEY,
    name NVARCHAR(100),
    icon NVARCHAR(255)
);

CREATE TABLE Food (
    id_Food INTEGER PRIMARY KEY,
    id_Category INTEGER,
    id_Restaurant INTEGER,
    name NVARCHAR(100),
    description TEXT,
    image NVARCHAR(255),
    video VARCHAR(255),
    price DECIMAL(10,2),
    discount DECIMAL(10,2),
    cook_Count INTEGER,
    prep_Time INTEGER,
    FOREIGN KEY (id_Category) REFERENCES Category(id_Category),
    FOREIGN KEY (id_Restaurant) REFERENCES Restaurant(id_Restaurant)
);

CREATE TABLE Food_Image (
    id_Foodimage INTEGER PRIMARY KEY,
    id_Food INTEGER,
    image VARCHAR(255),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

CREATE TABLE Cart (
    id_Cart INTEGER PRIMARY KEY,
    id_User INTEGER,
    total INTEGER,
    created_At DATETIME,
    update_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE Cart_Food (
    id_CartFood INTEGER PRIMARY KEY,
    id_Cart INTEGER,
    id_Food INTEGER,
    quantity INTEGER,
    note NVARCHAR(255),
    FOREIGN KEY (id_Cart) REFERENCES Cart(id_Cart),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

CREATE TABLE Promotion (
    id_Promo INTEGER PRIMARY KEY,
    type VARCHAR(20),
    value DECIMAL(10,2),
    min_OrderValue DECIMAL(10,2),
    max_Discount DECIMAL(10,2),
    usage_Limit INTEGER,
    used_Count INTEGER,
    start_Date DATETIME,
    end_Date DATETIME,
    id_Restaurant INTEGER,
    FOREIGN KEY (id_Restaurant) REFERENCES Restaurant(id_Restaurant)
);

CREATE TABLE Order_Promotion (
    id_Order INTEGER,
    id_Promo INTEGER,
    discount_Amount DECIMAL(10,2),
    PRIMARY KEY (id_Order, id_Promo)
);

CREATE TABLE Order_Restaurant (
    id_Order INTEGER PRIMARY KEY,
    id_Restaurant INTEGER,
    shipFee DECIMAL(10,2),
    status ENUM('pending','accepted','completed'),
    FOREIGN KEY (id_Restaurant) REFERENCES Restaurant(id_Restaurant)
);

CREATE TABLE `Order` (
    id_Order INTEGER PRIMARY KEY,
    id_User INTEGER,
    id_Address INTEGER,
    id_Driver INTEGER,
    paymentMethod VARCHAR(50),
    total DECIMAL(10,2),
    shippingFee DECIMAL(10,2),
    discount DECIMAL(10,2),
    finalTotal DECIMAL(10,2),
    status ENUM('pending','confirmed','delivering','completed','canceled'),
    note VARCHAR(255),
    created_At DATETIME,
    confirmed_At DATETIME,
    delivering_At DATETIME,
    delivered_At DATETIME,
    canceled_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES User(id_User),
    FOREIGN KEY (id_Address) REFERENCES Address(id_Address)
);

CREATE TABLE Order_Food (
    id_OrderFood INTEGER PRIMARY KEY,
    id_Order INTEGER,
    id_Food INTEGER,
    quantity INTEGER,
    unit_Price DECIMAL(10,2),
    note NVARCHAR(255),
    FOREIGN KEY (id_Order) REFERENCES `Order`(id_Order),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

CREATE TABLE Review (
    id_Review INTEGER PRIMARY KEY,
    id_User INTEGER,
    id_Order INTEGER,
    id_Restaurant INTEGER,
    food_rating FLOAT,
    driver_rating FLOAT,
    comment_ForRes NVARCHAR(255),
    comment_ForShipper NVARCHAR(255),
    created_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE Review_Food (
    id_ReviewFood INTEGER PRIMARY KEY,
    id_Review INTEGER,
    id_Food INTEGER,
    rating FLOAT,
    comment NVARCHAR(255),
    image VARCHAR(255),
    video VARCHAR(115),
    FOREIGN KEY (id_Review) REFERENCES Review(id_Review),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

CREATE TABLE Driver (
    id_Driver INTEGER PRIMARY KEY,
    id_User INTEGER,
    license_plate VARCHAR(20),
    address VARCHAR(255),
    expRank VARCHAR(50),
    desc_Status TEXT,
    current_Lat DECIMAL(10,7),
    current_Lng DECIMAL(10,7),
    is_Busy BOOLEAN,
    rate_Avg DECIMAL(2,1),
    total_Orders INTEGER,
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE PaymentMethod (
    id_Transaction INTEGER PRIMARY KEY,
    id_User INTEGER,
    id_Order INTEGER,
    method VARCHAR(50),
    amount DECIMAL(10,2),
    FOREIGN KEY (id_User) REFERENCES User(id_User),
    FOREIGN KEY (id_Order) REFERENCES `Order`(id_Order)
);

CREATE TABLE Complaint (
    id_Complaint INTEGER PRIMARY KEY,
    id_Order INTEGER,
    id_User INTEGER,
    type ENUM('food','driver','other'),
    description TEXT,
    image VARCHAR(255),
    status ENUM('open','closed'),
    handled_By VARCHAR(255),
    received_At DATETIME,
    resolved_At DATETIME,
    FOREIGN KEY (id_Order) REFERENCES `Order`(id_Order),
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);

CREATE TABLE SystemLog (
    id_Log INTEGER PRIMARY KEY,
    id_User INTEGER,
    action VARCHAR(100),
    entity VARCHAR(100),
    entity_Id INTEGER,
    old_Value JSON,
    new_Value JSON,
    ip_Address VARCHAR(45),
    created_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES User(id_User)
);
