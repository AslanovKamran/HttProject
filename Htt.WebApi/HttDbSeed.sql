GO
USE master

GO
CREATE DATABASE HttStoreDb

GO 
USE HttStoreDb



--Tables Creation

GO
CREATE TABLE Categories(
[Id] INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(100) UNIQUE NOT NULL 
)

GO
CREATE TABLE Products(
[Id] INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(100) UNIQUE NOT NULL ,
[Price] DECIMAL (10,2) NOT NULL CHECK([Price] > 0),
)

--Linking Table (Many-To-Many Relationship)


GO
CREATE TABLE ProductCategories
(
[Id] INT PRIMARY KEY IDENTITY,
[ProductId] INT FOREIGN KEY REFERENCES Products(Id) ON DELETE CASCADE,
[CategoryId] INT FOREIGN KEY REFERENCES Categories(Id) ON DELETE CASCADE,
)



--Stored Procedures for Categories



GO
CREATE PROC AddNewCategory @Name NVARCHAR(100)
AS
BEGIN
INSERT INTO Categories
OUTPUT inserted.Id, inserted.Name
VALUES (@Name)
END


GO
CREATE PROC UpdateCategory @Id INT, @Name NVARCHAR(100)
AS
BEGIN
UPDATE Categories SET
Categories.Name = @Name 
OUTPUT inserted.Id, inserted.Name
WHERE Categories.Id = @Id
END

GO
CREATE PROC DeleteCategory @Id INT
AS
BEGIN
DELETE FROM Categories WHERE Categories.Id =  @Id
END


GO
CREATE PROC GetAllCategories 
AS
BEGIN
SELECT * FROM Categories
END

GO
CREATE PROC GetCategoryById @Id INT
AS
BEGIN
SELECT * FROM Categories
WHERE Categories.Id = @Id
END



--Stored Procedures for Products



GO
CREATE PROC AddNewProduct @Name NVARCHAR(100), @Price DECIMAL (10,2)
AS
BEGIN
INSERT INTO Products
OUTPUT inserted.Id, inserted.Name, inserted.Price
VALUES (@Name, @Price)
END

GO
CREATE PROC UpdateProduct @Id INT, @Name NVARCHAR(100), @Price DECIMAL (10,2)
AS
BEGIN
UPDATE Products SET
Products.Name = @Name, Products.Price = @Price
OUTPUT inserted.Id, inserted.Name, inserted.Price
WHERE Products.Id = @Id
END

GO
CREATE PROC DeleteProduct @Id INT
AS
BEGIN
DELETE FROM Products WHERE Products.Id =  @Id
END

GO
CREATE PROC GetAllProducts
AS
BEGIN
SELECT * FROM Products
END

GO
CREATE PROC GetProductById @Id INT
AS
BEGIN
SELECT * FROM Products
WHERE Products.Id = @Id
END

--Stored Procedures ProductCategories

GO
CREATE PROC AddCategoryToProduct @ProductId INT, @CategoryId INT
AS
BEGIN
INSERT INTO ProductCategories (ProductId, CategoryId) VALUES (@ProductId, @CategoryId)
END

GO
CREATE PROC DeleteCategoryFromProduct @ProductId INT, @CategoryId INT
AS
BEGIN
DELETE FROM ProductCategories 
WHERE ProductCategories.ProductId = @ProductId AND ProductCategories.CategoryId = @CategoryId
END

GO
CREATE PROC GetAllCategoriesByProductId @ProductId INT
AS
BEGIN
SELECT Categories.Id, Categories.Name FROM ProductCategories 
JOIN Products ON ProductCategories.ProductId = Products.Id
JOIN Categories ON ProductCategories.CategoryId = Categories.Id
WHERE Products.Id = @ProductId
END

SELECT * FROM Products









