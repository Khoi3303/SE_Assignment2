USE master;
GO

-- 1. Xóa Database cũ nếu đang tồn tại để làm mới hoàn toàn
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'SE_Assignment2_DB')
BEGIN
    ALTER DATABASE SE_Assignment2_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SE_Assignment2_DB;
END
GO

-- 2. Tạo Database mới
CREATE DATABASE SE_Assignment2_DB;
GO
USE SE_Assignment2_DB;
GO

-- 3. Tạo các cấu trúc bảng (Tables)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName VARCHAR(50) NOT NULL,
    Email VARCHAR(100),
    Password VARCHAR(50) NOT NULL,
    [Lock] BIT DEFAULT 0
);

CREATE TABLE Agent (
    AgentID INT IDENTITY(1,1) PRIMARY KEY,
    AgentName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255)
);

CREATE TABLE Item (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100) NOT NULL,
    Size VARCHAR(50),
    Price FLOAT -- Cột Giá tiền rất quan trọng cho Form Đơn hàng
);

CREATE TABLE [Order] (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATETIME DEFAULT GETDATE(),
    AgentID INT FOREIGN KEY REFERENCES Agent(AgentID)
);

CREATE TABLE OrderDetail (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT FOREIGN KEY REFERENCES [Order](OrderID),
    ItemID INT FOREIGN KEY REFERENCES Item(ItemID),
    Quantity INT,
    UnitAmount FLOAT
);
GO

-- 4. Chèn dữ liệu mẫu (Đạt chuẩn 15-20 dòng cho mỗi bảng để làm báo cáo)

-- Bảng Users (15 tài khoản)
INSERT INTO Users (UserName, Email, Password, [Lock]) VALUES
('admin', 'admin@tdtu.edu.vn', '123456', 0),
('khoi_manager', 'khoi@tdtu.edu.vn', '123', 0),
('staff01', 'staff1@test.com', '123', 0),
('staff02', 'staff2@test.com', '123', 0),
('staff03', 'staff3@test.com', '123', 0),
('staff04', 'staff4@test.com', '123', 0),
('staff05', 'staff5@test.com', '123', 1), -- Bị khóa
('staff06', 'staff6@test.com', '123', 0),
('staff07', 'staff7@test.com', '123', 0),
('staff08', 'staff8@test.com', '123', 0),
('staff09', 'staff9@test.com', '123', 0),
('staff10', 'staff10@test.com', '123', 0),
('staff11', 'staff11@test.com', '123', 0),
('staff12', 'staff12@test.com', '123', 0),
('staff13', 'staff13@test.com', '123', 0);

-- Bảng Agent (15 Đại lý)
INSERT INTO Agent (AgentName, Address) VALUES
(N'Đại lý Bình Minh', N'Quận 1, TP.HCM'),
(N'Đại lý Thành Công', N'Quận 3, TP.HCM'),
(N'Cửa hàng Tiện Lợi 24h', N'Tân Bình, TP.HCM'),
(N'Đại lý Miền Đông', N'Biên Hòa, Đồng Nai'),
(N'Hợp tác xã An Bình', N'Cần Thơ'),
(N'Đại lý Phú Quý', N'Quận 11, TP.HCM'),
(N'Cửa hàng số 10', N'Tân Phú, TP.HCM'),
(N'Đại lý Sao Mai', N'Phú Nhuận, TP.HCM'),
(N'Đại lý Toàn Thắng', N'Gò Vấp, TP.HCM'),
(N'Đại lý Hữu Nghị', N'Quận 5, TP.HCM'),
(N'Đại lý Hoàng Gia', N'Quận 7, TP.HCM'),
(N'Showroom TechPro', N'Quận 10, TP.HCM'),
(N'Đại lý Phát Đạt', N'Bình Thạnh, TP.HCM'),
(N'Cửa hàng Minh Khôi', N'Thủ Đức, TP.HCM'),
(N'Đại lý An Khang', N'Quận 8, TP.HCM');

-- Bảng Item (20 Sản phẩm)
INSERT INTO Item (ItemName, Size, Price) VALUES
(N'Laptop Dell Latitude 5411', N'14 inch', 15000000),
(N'RAM DDR4 3200MHz', N'32GB', 1500000),
(N'RAM DDR4 3200MHz', N'64GB', 3000000),
(N'SSD Samsung 870 EVO', N'1TB', 2000000),
(N'Sò lạnh tản nhiệt SL33', N'Tiêu chuẩn', 250000),
(N'Sò lạnh tản nhiệt J86 Pro', N'Bản Pro', 350000),
(N'Pin Dell Latitude 5411', N'68Wh', 1200000),
(N'Ổ cứng HDD Toshiba', N'1TB', 1000000),
(N'Bàn phím cơ Custom', N'75% Layout', 1800000),
(N'Chuột Gaming không dây', N'Form to', 800000),
(N'Sữa tươi Vinamilk', N'180ml', 8500),
(N'Nước khoáng La Vie', N'500ml', 5000),
(N'Bánh mì sandwich', N'Gói lớn', 25000),
(N'Gạo ST25', N'Túi 5kg', 180000),
(N'Tài khoản Minecraft', N'Bản quyền', 600000),
(N'Sách Luyện thi IELTS', N'Bản in', 250000),
(N'Tài liệu ôn thi Aptis', N'Bản PDF', 150000),
(N'Đế tản nhiệt Laptop', N'15.6 inch', 300000),
(N'Balo chống sốc Laptop', N'Vừa', 450000),
(N'Hub chuyển đổi Type-C', N'7 trong 1', 550000);

-- Bảng Order (15 Đơn hàng)
INSERT INTO [Order] (OrderDate, AgentID) VALUES
('2026-04-01', 1), ('2026-04-02', 2), ('2026-04-03', 3),
('2026-04-04', 1), ('2026-04-05', 5), ('2026-04-06', 4),
('2026-04-07', 7), ('2026-04-08', 8), ('2026-04-09', 9),
('2026-04-10', 10),('2026-04-11', 14),('2026-04-12', 12),
('2026-04-13', 13),('2026-04-14', 14),('2026-04-15', 15);

-- Bảng OrderDetail (20 Chi tiết đơn hàng)
INSERT INTO OrderDetail (OrderID, ItemID, Quantity, UnitAmount) VALUES
(1, 1, 2, 15000000), (1, 2, 4, 1500000),
(2, 4, 5, 2000000),  (2, 5, 10, 250000),
(3, 11, 50, 8500),   (3, 12, 100, 5000),
(4, 14, 20, 180000), (4, 13, 30, 25000),
(5, 9, 3, 1800000),  (5, 10, 5, 800000),
(6, 15, 10, 600000), (7, 16, 20, 250000),
(8, 17, 15, 150000), (9, 18, 5, 300000),
(10, 19, 8, 450000), (11, 20, 12, 550000),
(12, 3, 4, 3000000), (13, 6, 10, 350000),
(14, 7, 2, 1200000), (15, 8, 5, 1000000);
GO