IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(300) NULL,
    [Price] decimal(10,2) NOT NULL,
    [ImageUrl] nvarchar(200) NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'ImageUrl', N'Name', N'Price') AND [object_id] = OBJECT_ID(N'[Products]'))
    SET IDENTITY_INSERT [Products] ON;
INSERT INTO [Products] ([Id], [Description], [ImageUrl], [Name], [Price])
VALUES (1, N'12 Fresh Red Roses with Custom Arrangement', N'/flower.jpg', N'Red Rose Bouquet', 120.0),
(2, N'Luxury Flower Mix with Premium Chocolate Box', N'/flower2.png', N'Roses with Chocolate', 200.0),
(3, N'Elegant White Lilies Wrapped in Premium Paper', NULL, N'White Lily Bouquet', 150.0),
(4, N'Colorful Tulips in a Luxury Gift Box', NULL, N'Tulip Gift Box', 180.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'ImageUrl', N'Name', N'Price') AND [object_id] = OBJECT_ID(N'[Products]'))
    SET IDENTITY_INSERT [Products] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261118135936_AddProducts', N'10.0.15');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261123172645_AddUsers', N'10.0.15');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Users] ADD [Role] nvarchar(max) NOT NULL DEFAULT N'Customer';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261123191431_AddUserRole', N'10.0.15');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [UserEmail] nvarchar(150) NOT NULL,
    [CustomerName] nvarchar(100) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [Address] nvarchar(300) NOT NULL,
    [DeliveryDate] datetime2 NOT NULL,
    [Notes] nvarchar(200) NULL,
    [OrderDate] datetime2 NOT NULL,
    [Total] decimal(10,2) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
);

CREATE TABLE [OrderItems] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [ProductName] nvarchar(max) NOT NULL,
    [Price] decimal(10,2) NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

UPDATE [Products] SET [ImageUrl] = N'/images/flower.jpg'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


UPDATE [Products] SET [ImageUrl] = N'/images/flower2.png'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;


CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261123224723_AddOrders', N'10.0.15');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Products] ADD [Category] nvarchar(50) NOT NULL DEFAULT N'Bouquets';

UPDATE [Products] SET [Category] = N'Roses'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


UPDATE [Products] SET [Category] = N'Gift Boxes'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;


UPDATE [Products] SET [Category] = N'Bouquets'
WHERE [Id] = 3;
SELECT @@ROWCOUNT;


UPDATE [Products] SET [Category] = N'Gift Boxes'
WHERE [Id] = 4;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261124011106_AddCategories', N'10.0.15');

COMMIT;
GO

BEGIN TRANSACTION;
UPDATE [Products] SET [ImageUrl] = N'/images/red-roses.jpg'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


UPDATE [Products] SET [ImageUrl] = N'/images/roses-chocolate.jpg'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;


UPDATE [Products] SET [ImageUrl] = N'/images/white-lily.jpg'
WHERE [Id] = 3;
SELECT @@ROWCOUNT;


UPDATE [Products] SET [ImageUrl] = N'/images/tulip-box.jpg'
WHERE [Id] = 4;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261124013937_UpdateProductImages', N'10.0.15');

COMMIT;
GO

