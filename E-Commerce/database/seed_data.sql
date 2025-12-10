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
