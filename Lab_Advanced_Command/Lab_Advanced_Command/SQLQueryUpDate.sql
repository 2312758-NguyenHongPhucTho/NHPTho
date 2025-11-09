CREATE OR ALTER PROCEDURE [UpdateFood]
    @ID int,
    @Name nvarchar(1000),
    @Unit nvarchar(100),
    @FoodCategoryID int,
    @Price int,
    @Notes nvarchar(3000)
AS
UPDATE [Food]
SET
    [Name] = @Name,
    [Unit] = @Unit,
    [FoodCategoryID] = @FoodCategoryID,
    [Price] = @Price,
    [Notes] = @Notes
WHERE ID = @ID

IF @@ERROR <> 0
    RETURN 0
ELSE
    RETURN 1
GO