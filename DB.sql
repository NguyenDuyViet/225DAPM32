-- Xóa database nếu tồn tại
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'MonNgonTaiNha')
BEGIN
    ALTER DATABASE MonNgonTaiNha SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MonNgonTaiNha;
END
GO

CREATE DATABASE MonNgonTaiNha;
GO

USE MonNgonTaiNha;
GO

-- USER
CREATE TABLE Users (
    id_User INT PRIMARY KEY,
    username VARCHAR(50),
    password VARCHAR(15),
    phone VARCHAR(15),
    email VARCHAR(50),
    avatar VARCHAR(255),
    status VARCHAR(10) CHECK (status IN ('active','inactive')),
    created_At DATETIME,
    lastOnline DATETIME,
    update_bio VARCHAR(255),
    update_avatar VARCHAR(255),
    update_bg VARCHAR(255),
    current_Lat DECIMAL(10,7),
    current_Lng DECIMAL(10,7),
    cancel_Rate FLOAT
);

-- ADDRESS
CREATE TABLE Address (
    id_Address INT PRIMARY KEY,
    id_User INT,
    name VARCHAR(45),
    phone VARCHAR(15),
    address VARCHAR(255),
    lat DECIMAL(10,7),
    lng DECIMAL(10,7),
    note VARCHAR(255),
    is_Default BIT,
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

CREATE TABLE User_Address (
    id_Address INT,
    id_User INT,
    PRIMARY KEY (id_Address, id_User),
    FOREIGN KEY (id_Address) REFERENCES Address(id_Address),
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

-- NOTIFICATION
CREATE TABLE Notification (
    id_Noti INT PRIMARY KEY,
    id_User INT,
    title VARCHAR(100),
    body NVARCHAR(MAX),
    orderId INT,
    is_Read BIT,
    created_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

CREATE TABLE User_Notification (
    id_Noti INT,
    id_User INT,
    PRIMARY KEY (id_Noti, id_User),
    FOREIGN KEY (id_Noti) REFERENCES Notification(id_Noti),
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

-- VOUCHER
CREATE TABLE Voucher (
    id_Voucher INT PRIMARY KEY,
    code VARCHAR(20),
    id_User INT,
    value DECIMAL(10,2),
    expiry DATETIME,
    used BIT,
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

CREATE TABLE User_Voucher (
    id_Voucher INT,
    id_User INT,
    PRIMARY KEY (id_Voucher, id_User),
    FOREIGN KEY (id_Voucher) REFERENCES Voucher(id_Voucher),
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

-- RESTAURANT
CREATE TABLE Restaurant (
    id_Restaurant INT PRIMARY KEY,
    name_Restaurant NVARCHAR(100),
    description NVARCHAR(MAX),
    image NVARCHAR(255),
    address NVARCHAR(255),
    openTime TIME,
    closeTime TIME,
    lat DECIMAL(10,7),
    lng DECIMAL(10,7)
);

-- CATEGORY
CREATE TABLE Category (
    id_Category INT PRIMARY KEY,
    name NVARCHAR(100),
    icon NVARCHAR(255)
);

-- FOOD
CREATE TABLE Food (
    id_Food INT PRIMARY KEY,
    id_Category INT,
    id_Restaurant INT,
    name NVARCHAR(100),
    description NVARCHAR(MAX),
    image NVARCHAR(255),
    video VARCHAR(255),
    price DECIMAL(10,2),
    discount DECIMAL(10,2),
    cook_Count INT,
    prep_Time INT,
    FOREIGN KEY (id_Category) REFERENCES Category(id_Category),
    FOREIGN KEY (id_Restaurant) REFERENCES Restaurant(id_Restaurant)
);

CREATE TABLE Food_Image (
    id_Foodimage INT PRIMARY KEY,
    id_Food INT,
    image VARCHAR(255),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

-- CART
CREATE TABLE Cart (
    id_Cart INT PRIMARY KEY,
    id_User INT,
    total INT,
    created_At DATETIME,
    update_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

CREATE TABLE Cart_Food (
    id_CartFood INT PRIMARY KEY,
    id_Cart INT,
    id_Food INT,
    quantity INT,
    note NVARCHAR(255),
    FOREIGN KEY (id_Cart) REFERENCES Cart(id_Cart),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

-- PROMOTION
CREATE TABLE Promotion (
    id_Promo INT PRIMARY KEY,
    type VARCHAR(20),
    value DECIMAL(10,2),
    min_OrderValue DECIMAL(10,2),
    max_Discount DECIMAL(10,2),
    usage_Limit INT,
    used_Count INT,
    start_Date DATETIME,
    end_Date DATETIME,
    id_Restaurant INT,
    FOREIGN KEY (id_Restaurant) REFERENCES Restaurant(id_Restaurant)
);

CREATE TABLE Order_Promotion (
    id_Order INT,
    id_Promo INT,
    discount_Amount DECIMAL(10,2),
    PRIMARY KEY (id_Order, id_Promo)
);

-- ORDER
CREATE TABLE Orders (
    id_Order INT PRIMARY KEY,
    id_User INT,
    id_Address INT,
    id_Driver INT,
    paymentMethod VARCHAR(50),
    total DECIMAL(10,2),
    shippingFee DECIMAL(10,2),
    discount DECIMAL(10,2),
    finalTotal DECIMAL(10,2),
    status VARCHAR(20) CHECK (status IN ('pending','confirmed','delivering','completed','canceled')),
    note VARCHAR(255),
    created_At DATETIME,
    confirmed_At DATETIME,
    delivering_At DATETIME,
    delivered_At DATETIME,
    canceled_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES Users(id_User),
    FOREIGN KEY (id_Address) REFERENCES Address(id_Address)
);

CREATE TABLE Order_Restaurant (
    id_Order INT PRIMARY KEY,
    id_Restaurant INT,
    shipFee DECIMAL(10,2),
    status VARCHAR(20) CHECK (status IN ('pending','accepted','completed')),
    FOREIGN KEY (id_Restaurant) REFERENCES Restaurant(id_Restaurant)
);

CREATE TABLE Order_Food (
    id_OrderFood INT PRIMARY KEY,
    id_Order INT,
    id_Food INT,
    quantity INT,
    unit_Price DECIMAL(10,2),
    note NVARCHAR(255),
    FOREIGN KEY (id_Order) REFERENCES Orders(id_Order),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

-- REVIEW
CREATE TABLE Review (
    id_Review INT PRIMARY KEY,
    id_User INT,
    id_Order INT,
    id_Restaurant INT,
    food_rating FLOAT,
    driver_rating FLOAT,
    comment_ForRes NVARCHAR(255),
    comment_ForShipper NVARCHAR(255),
    created_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

CREATE TABLE Review_Food (
    id_ReviewFood INT PRIMARY KEY,
    id_Review INT,
    id_Food INT,
    rating FLOAT,
    comment NVARCHAR(255),
    image VARCHAR(255),
    video VARCHAR(115),
    FOREIGN KEY (id_Review) REFERENCES Review(id_Review),
    FOREIGN KEY (id_Food) REFERENCES Food(id_Food)
);

-- DRIVER
CREATE TABLE Driver (
    id_Driver INT PRIMARY KEY,
    id_User INT,
    license_plate VARCHAR(20),
    address VARCHAR(255),
    expRank VARCHAR(50),
    desc_Status NVARCHAR(MAX),
    current_Lat DECIMAL(10,7),
    current_Lng DECIMAL(10,7),
    is_Busy BIT,
    rate_Avg DECIMAL(2,1),
    total_Orders INT,
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

-- PAYMENT
CREATE TABLE PaymentMethod (
    id_Transaction INT PRIMARY KEY,
    id_User INT,
    id_Order INT,
    method VARCHAR(50),
    amount DECIMAL(10,2),
    FOREIGN KEY (id_User) REFERENCES Users(id_User),
    FOREIGN KEY (id_Order) REFERENCES Orders(id_Order)
);

-- COMPLAINT
CREATE TABLE Complaint (
    id_Complaint INT PRIMARY KEY,
    id_Order INT,
    id_User INT,
    type VARCHAR(20) CHECK (type IN ('food','driver','other')),
    description NVARCHAR(MAX),
    image VARCHAR(255),
    status VARCHAR(20) CHECK (status IN ('open','closed')),
    handled_By VARCHAR(255),
    received_At DATETIME,
    resolved_At DATETIME,
    FOREIGN KEY (id_Order) REFERENCES Orders(id_Order),
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);

-- LOG
CREATE TABLE SystemLog (
    id_Log INT PRIMARY KEY,
    id_User INT,
    action VARCHAR(100),
    entity VARCHAR(100),
    entity_Id INT,
    old_Value NVARCHAR(MAX),
    new_Value NVARCHAR(MAX),
    ip_Address VARCHAR(45),
    created_At DATETIME,
    FOREIGN KEY (id_User) REFERENCES Users(id_User)
);