# E-commercebackend
It is backned product
Mini E-Commerce CRM

ASP.NET Core Web API + EF Core

📌 Introduction

Mini E-Comme<img width="1904" height="883" alt="Screenshot 2025-12-10 084123" src="https://github.com/user-attachments/assets/bb70f822-fcd3-4268-b920-61c9b56ca65f" />

<img width="1887" height="917" alt="Screenshot 2025-12-10 120124" src="https://github.com/user-attachments/assets/91292d0e-75c2-45e0-aab5-6111565b260b" />

rce CRM is a backend project built using ASP.NET Core Web API and Entity Framework Core.
It includes modules for managing Products, Customers, Orders, and Order Items.
Database creation and seed data are handled via SQL migration scripts.

📁 Project Architecture
1. Controllers (Presentation Layer)

These APIs expose endpoints for CRUD operations:

Controller	Description
CustomerController	Manage customers (Add, Edit, Delete, List)
ProductController	Manage product catalog
OrderController	Create orders, list orders, and handle order items
2. Data Layer (Data Folder)

This folder contains:

✔ ApplicationDbContext.cs

EF Core database context

Configures DbSets for Customer, Product, Order, OrderItem

Responsible for communicating with SQL Server

3. Database Folder (database)

Contains SQL scripts used for clean database creation.

(a) migration_script_clean.sql

CREATE TABLE [Products] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [Stock] INT NOT NULL,
    [Category] NVARCHAR(100) NOT NULL
);

-- ======== Customers Table ========
CREATE TABLE [Customers] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    [Email] NVARCHAR(200) NOT NULL,
    [Phone] NVARCHAR(50) NOT NULL
);

-- ======== Orders Table ========
CREATE TABLE [Orders] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CustomerId] INT NOT NULL,
    [OrderDate] DATETIME2 NOT NULL,
    [TotalAmount] DECIMAL(18,2) NOT NULL,
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([Id])
);

-- ======== OrderItems Table ========
CREATE TABLE [OrderItems] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [OrderId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    [Quantity] INT NOT NULL,
    [UnitPrice] DECIMAL(18,2) NOT NULL,
    CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]),
    CONSTRAINT [FK_OrderItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id])
);


-- Drop existing FK
--ALTER TABLE Orders DROP CONSTRAINT FK_Orders_Customers;
--ALTER TABLE OrderItems DROP CONSTRAINT FK_OrderItems_Orders;
--ALTER TABLE OrderItems DROP CONSTRAINT FK_OrderItems_Products;

---- Recreate with CASCADE
--ALTER TABLE Orders
--ADD CONSTRAINT FK_Orders_Customers
--FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE;

--ALTER TABLE OrderItems
--ADD CONSTRAINT FK_OrderItems_Orders
--FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE;

--ALTER TABLE OrderItems
--ADD CONSTRAINT FK_OrderItems_Products
--FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE;


(b) seed_data.sql

-- Seed Data: Mini E-Commerce CRM
-- ==========================

-- ======== Products ========
INSERT INTO Products (Name, Price, Stock, Category) VALUES
('iPhone 14', 70000, 15, 'Mobile'),
('Samsung Galaxy S23', 55000, 20, 'Mobile'),
('Dell Inspiron 15', 50000, 10, 'Laptop'),
('HP Pavilion', 48000, 12, 'Laptop'),
('Sony Headphones', 3000, 50, 'Accessories'),
('Logitech Mouse', 1200, 60, 'Accessories');

-- ======== Customers ========
INSERT INTO Customers (Name, Email, Phone) VALUES
('Murari Jha', 'murari@example.com', '9876543210'),
('Rahul Singh', 'rahul@example.com', '9876501234'),
('Priya Sharma', 'priya@example.com', '9876512340'),
('Ankit Kumar', 'ankit@example.com', '9876523450');

-- ======== Orders ========
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) VALUES
(1, GETDATE(), 73000),
(2, GETDATE(), 51200),
(3, GETDATE(), 6200);

-- ======== Order Items ========
-- Murari's Order
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
(1, 1, 1, 70000),  -- iPhone 14
(1, 5, 1, 3000);   -- Sony Headphones

-- Rahul's Order
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
(2, 3, 1, 50000),  -- Dell Inspiron 15
(2, 6, 1, 1200);   -- Logitech Mouse

-- Priya's Order
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
(3, 5, 2, 3000);   -- Sony Headphones x2

Running Migration and Seed Scripts
Step 1: Create Database Schema
sqlcmd -S MUNMUN\JHA -d MiniECommerceCRM -i "C:\Users\bhave\source\repos\E-Commerce\E-Commerce\database\migration_script_clean.sql"

Step 2: Run Seed Data
sqlcmd -S MUNMUN\JHA -d MiniECommerceCRM -i "C:\Users\bhave\source\repos\E-Commerce\E-Commerce\database\seed_data.sql"

📦 Models (Models Folder)
Model	Description
Customer	Stores customer info (Id, Name, Email, Phone)
Product	Product catalog (Id, Name, Price, Stock, Category)
Order	Order details (Id, CustomerId, OrderDate, Total)
OrderItem	Items inside an order (ProductId, Quantity, Price)
🚀 Technologies Used



C# .NET 8/7

SQLCMD CLI Tool
