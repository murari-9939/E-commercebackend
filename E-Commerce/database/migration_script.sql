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
