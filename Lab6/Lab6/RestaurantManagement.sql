USE [RestaurantManagement]
GO
/****** Object:  UserDefinedFunction [dbo].[GetTotalSalesByDate]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetTotalSalesByDate]
    (@Date DATE)
RETURNS FLOAT
AS
BEGIN
    DECLARE @TotalSales FLOAT;

    SELECT @TotalSales = SUM(bd.Quantity * f.Price)
    FROM [BillDetails] bd
    JOIN [Bills] b ON bd.InvoiceID = b.ID
    JOIN [Food] f ON bd.FoodID = f.ID
    WHERE CAST(b.CheckoutDate AS DATE) = @Date;

    RETURN ISNULL(@TotalSales, 0);
END;
GO
/****** Object:  Table [dbo].[Bills]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bills](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	[TableID] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Discount] [float] NULL,
	[Tax] [float] NULL,
	[CheckoutDate] [smalldatetime] NULL,
	[Account] [nvarchar](100) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE Bills
ADD [Total] decimal(18,2) NOT NULL DEFAULT 0;
/****** Object:  Table [dbo].[Food]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[Name] [nvarchar](1000) NOT NULL,
	[Unit] [nvarchar](100) NOT NULL,
	[FoodCategoryID] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[Notes] [nvarchar](3000) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Food] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillDetails]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillDetails](
	[InvoiceID] [int] NOT NULL,
	[FoodID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[GetFoodQuantityByDate]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetFoodQuantityByDate]
    (@Date DATE)
RETURNS TABLE
AS
RETURN
(
    SELECT f.Name AS FoodName, SUM(bd.Quantity) AS TotalQuantity
    FROM [BillDetails] bd
    JOIN [Bills] b ON bd.InvoiceID = b.ID
    JOIN [Food] f ON bd.FoodID = f.ID
    WHERE CAST(b.CheckoutDate AS DATE) = @Date
    GROUP BY f.Name
);
GO
/****** Object:  Table [dbo].[Account]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[FullName] [nvarchar](1000) NOT NULL,
	[Email] [nvarchar](1000) NULL,
	[Tell] [nvarchar](200) NULL,
	[DateCreated] [smalldatetime] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Name] [nvarchar](1000) NOT NULL,
	[Type] [int] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleName] [nvarchar](1000) NOT NULL,
	[Path] [nvarchar](3000) NULL,
	[Notes] [nvarchar](3000) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleAccount]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleAccount](
	[RoleID] [int] NOT NULL,
	[AccountName] [nvarchar](100) NOT NULL,
	[Actived] [bit] NOT NULL,
	[Notes] [nvarchar](3000) NULL,
 CONSTRAINT [PK_RoleAccount] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC,
	[AccountName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Table]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table](
	[Name] [nvarchar](1000) NULL,
	[Status] [int] NOT NULL,
	[Capacity] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([AccountName], [Password], [FullName], [Email], [Tell], [DateCreated]) VALUES (N'admin', N'123456', N'Admin', N'admin@example.com', N'0123456789', CAST(N'2025-11-18T09:00:00' AS SmallDateTime))
INSERT [dbo].[Account] ([AccountName], [Password], [FullName], [Email], [Tell], [DateCreated]) VALUES (N'staff1', N'password1', N'Staff One', N'staff1@example.com', N'0987654321', CAST(N'2025-09-18T10:00:00' AS SmallDateTime))
INSERT [dbo].[Account] ([AccountName], [Password], [FullName], [Email], [Tell], [DateCreated]) VALUES (N'staff2', N'2', N'Staff Two', N'staff2@example.com', N'0883248232', CAST(N'2025-11-24T17:32:00' AS SmallDateTime))
INSERT [dbo].[Account] ([AccountName], [Password], [FullName], [Email], [Tell], [DateCreated]) VALUES (N'user1', N'newpass123', N'Nguyễn Văn A', N'a.nguyen@email.com', N'0246813579', CAST(N'2025-08-14T09:00:00' AS SmallDateTime))
INSERT [dbo].[Account] ([AccountName], [Password], [FullName], [Email], [Tell], [DateCreated]) VALUES (N'user3', N'123456', N'Phạm Minh C', N'c.pham@email.com', N'0534843841', CAST(N'2025-10-17T17:42:00' AS SmallDateTime))
INSERT [dbo].[Account] ([AccountName], [Password], [FullName], [Email], [Tell], [DateCreated]) VALUES (N'user4', N'23456', N'Trần Thùy D', N'd.tran@email.com', N'0219348623', CAST(N'2025-10-24T16:30:00' AS SmallDateTime))
GO
SET IDENTITY_INSERT [dbo].[BillDetails] ON 

INSERT [dbo].[BillDetails] ([InvoiceID], [FoodID], [Quantity], [ID]) VALUES (1, 1, 2, 1)
INSERT [dbo].[BillDetails] ([InvoiceID], [FoodID], [Quantity], [ID]) VALUES (2, 2, 3, 2)
INSERT [dbo].[BillDetails] ([InvoiceID], [FoodID], [Quantity], [ID]) VALUES (7, 1, 1, 6)
INSERT [dbo].[BillDetails] ([InvoiceID], [FoodID], [Quantity], [ID]) VALUES (8, 1, 1, 7)
INSERT [dbo].[BillDetails] ([InvoiceID], [FoodID], [Quantity], [ID]) VALUES (8, 3, 1, 8)
SET IDENTITY_INSERT [dbo].[BillDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Bills] ON 

INSERT [dbo].[Bills] ([Name], [TableID], [Amount], [Discount], [Tax], [CheckoutDate], [Account], [Status], [Total]) VALUES ( N'Hóa đơn 1', 1, 500000, 0, 0.05000000074505806, CAST(N'2025-11-18T12:00:00' AS SmallDateTime), N'user1', 1, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Bills] ([Name], [TableID], [Amount], [Discount], [Tax], [CheckoutDate], [Account], [Status], [Total]) VALUES (N'Hóa đơn 2', 2, 800000, 0.5, 0.079999998211860657, CAST(N'2025-11-18T14:00:00' AS SmallDateTime), N'user2', 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Bills] ([Name], [TableID], [Amount], [Discount], [Tax], [CheckoutDate], [Account], [Status], [Total]) VALUES (N'Hóa đơn 3', 3, 1000000, 0.9, 0.8, CAST(N'2025-10-28T14:00:00' AS SmallDateTime), N'admin', 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Bills] ([Name], [TableID], [Amount], [Discount], [Tax], [CheckoutDate], [Account], [Status], [Total]) VALUES (N'Hóa đơn 4', 6, 650000, 1.4, 0.67, CAST(N'2025-10-14T14:00:00' AS SmallDateTime), N'user2', 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Bills] ([Name], [TableID], [Amount], [Discount], [Tax], [CheckoutDate], [Account], [Status], [Total]) VALUES ( N'Hóa đơn của Admin', 0, 0, 0, 0, NULL, N'admin', 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Bills] ( [Name], [TableID], [Amount], [Discount], [Tax], [CheckoutDate], [Account], [Status], [Total]) VALUES ( N'Hóa đơn của Nguyễn Văn A', 0, 0, 0, 0, CAST(N'2025-10-27T10:50:00' AS SmallDateTime), N'user1', 1, CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Bills] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Hải sản', 1, 2, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Gà', 1, 3, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Cơm', 1, 4, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Thịt', 1, 5, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Rau', 1, 6, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Canh', 1, 7, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Lẩu', 1, 8, 1)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Bia', 0, 9, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Nước ngọt', 0, 10, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Cà phê', 0, 11, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Trà sữa', 0, 12, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Bánh mì', 1, 16, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Rượu vang', 0, 17, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Tráng miệng', 1, 18, NULL)
INSERT [dbo].[Category] ([Name], [Type], [ID], [IsDeleted]) VALUES (N'Phở', 1, 21, 0)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Food] ON 

INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Gà nướng', N'Đĩa', 3, 100000, N'', 1)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Cơm chiên hải sản', N'Phần', 4, 150000, N'', 2)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Lẩu thái', N'Nồi', 8, 250000, N'Cay', 3)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Nước ngọt lon', N'Lon', 10, 20000, N'Lạnh', 4)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Bia tươi', N'Ly', 9, 50000, N'Chilled', 5)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Cà phê sữa', N'Ly', 11, 25000, N'Ðá', 6)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Bò nướng lá lốt', N'Đĩa', 5, 100000, N'', 8)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Lẩu bò chua cay', N'Nồi', 8, 350000, N'4 người ăn', 9)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Gà hấp bia', N'Đĩa', 3, 75000, N'', 11)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Cà phê trứng', N'Ly', 11, 20000, N'', 13)
INSERT [dbo].[Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes], [ID]) VALUES (N'Trà sâm dứa', N'Ly', 12, 25000, N'', 14)
SET IDENTITY_INSERT [dbo].[Food] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([RoleName], [Path], [Notes], [ID]) VALUES (N'Admin One', N'/admin1', N'Người quản trị', 1)
INSERT [dbo].[Role] ([RoleName], [Path], [Notes], [ID]) VALUES (N'Staff One', N'/staff1', N'Nhân viên bồi bàn', 2)
INSERT [dbo].[Role] ([RoleName], [Path], [Notes], [ID]) VALUES (N'Staff Two', N'/staff2', N'Nhân viên phục vụ', 3)
INSERT [dbo].[Role] ([RoleName], [Path], [Notes], [ID]) VALUES (N'Staff Three', N'/staff3', N'Nhân viên kế toán', 6)
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (1, N'admin', 1, N'Quản trị viên')
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (2, N'staff1', 1, N'Nhân viên phục vụ')
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (2, N'staff2', 1, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (3, N'staff1', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (3, N'staff2', 0, N'Nhân viên bồi bàn')
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (3, N'user1', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (3, N'user3', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (3, N'user4', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (6, N'staff1', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (6, N'staff2', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (6, N'user1', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (6, N'user3', 0, NULL)
INSERT [dbo].[RoleAccount] ([RoleID], [AccountName], [Actived], [Notes]) VALUES (6, N'user4', 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[Table] ON 

INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 1', 1, 4, 1)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 2', 1, 4, 2)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 3', 1, 6, 3)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 4', 1, 6, 6)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 5', 0, 0, 8)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn VIP', 0, 0, 9)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 5 - 1', 0, 0, 10)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 5 - 2', 0, 0, 11)
INSERT [dbo].[Table] ([Name], [Status], [Capacity], [ID]) VALUES (N'Bàn 6', 0, 0, 12)
SET IDENTITY_INSERT [dbo].[Table] OFF
GO
ALTER TABLE [dbo].[Bills] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Bills] ADD  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD  CONSTRAINT [FK_BillDetails_Bills] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Bills] ([ID])
GO
ALTER TABLE [dbo].[BillDetails] CHECK CONSTRAINT [FK_BillDetails_Bills]
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD  CONSTRAINT [FK_BillDetails_Food] FOREIGN KEY([FoodID])
REFERENCES [dbo].[Food] ([ID])
GO
ALTER TABLE [dbo].[BillDetails] CHECK CONSTRAINT [FK_BillDetails_Food]
GO
ALTER TABLE [dbo].[Food]  WITH CHECK ADD  CONSTRAINT [FK_Food_Category] FOREIGN KEY([FoodCategoryID])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[Food] CHECK CONSTRAINT [FK_Food_Category]
GO
ALTER TABLE [dbo].[RoleAccount]  WITH CHECK ADD  CONSTRAINT [FK_RoleAccount_Account] FOREIGN KEY([AccountName])
REFERENCES [dbo].[Account] ([AccountName])
GO
ALTER TABLE [dbo].[RoleAccount] CHECK CONSTRAINT [FK_RoleAccount_Account]
GO
ALTER TABLE [dbo].[RoleAccount]  WITH CHECK ADD  CONSTRAINT [FK_RoleAccount_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleAccount] CHECK CONSTRAINT [FK_RoleAccount_Role]
GO
/****** Object:  StoredProcedure [dbo].[_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[_Delete]
    @TableName NVARCHAR(100),
    @ID INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'DELETE FROM ' + QUOTENAME(@TableName) + ' WHERE ID = @ID';
    EXEC sp_executesql @SQL, N'@ID INT', @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[_GetAll]
    @TableName NVARCHAR(100)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT * FROM ' + QUOTENAME(@TableName);
    EXEC sp_executesql @SQL;
END;
GO
/****** Object:  StoredProcedure [dbo].[_GetByID]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[_GetByID]
    @TableName NVARCHAR(100),
    @ID INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'SELECT * FROM ' + QUOTENAME(@TableName) + ' WHERE ID = @ID';
    EXEC sp_executesql @SQL, N'@ID INT', @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Account_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Account_Delete]
    @AccountName NVARCHAR(100)
AS
BEGIN
    DELETE FROM [Account]
    WHERE AccountName = @AccountName;
END;
GO
/****** Object:  StoredProcedure [dbo].[Account_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Account_GetAll]
AS
SELECT * FROM [Account];
GO
/****** Object:  StoredProcedure [dbo].[Account_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Account_Insert]
    @AccountName NVARCHAR(100),
    @Password NVARCHAR(100),
    @FullName NVARCHAR(100),
    @Email NVARCHAR(200),
    @Tell NVARCHAR(50),
    @DateCreated DATETIME
AS
BEGIN
    INSERT INTO [Account] (AccountName, Password, FullName, Email, Tell, DateCreated)
    VALUES (@AccountName, @Password, @FullName, @Email, @Tell, @DateCreated);
END;
GO
/****** Object:  StoredProcedure [dbo].[Account_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Account_InsertUpdateDelete]
    @AccountName NVARCHAR(100),
    @Password NVARCHAR(200),
    @FullName NVARCHAR(1000),
    @Email NVARCHAR(1000),
    @Tell NVARCHAR(200),
    @DateCreated SMALLDATETIME,
    @Action INT
AS
BEGIN
    IF @Action = 0 -- Thêm mới
    BEGIN
        INSERT INTO [Account] ([AccountName], [Password], [FullName], [Email], [Tell], [DateCreated])
        VALUES (@AccountName, @Password, @FullName, @Email, @Tell, @DateCreated)
    END
    ELSE IF @Action = 1 -- Cập nhật
    BEGIN
        UPDATE [Account]
        SET [Password] = @Password, [FullName] = @FullName, [Email] = @Email,
            [Tell] = @Tell, [DateCreated] = @DateCreated
        WHERE [AccountName] = @AccountName
    END
    ELSE IF @Action = 2 -- Xóa
    BEGIN
        DELETE FROM [Account]
        WHERE [AccountName] = @AccountName
    END
END
GO
/****** Object:  StoredProcedure [dbo].[Account_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Account_Update]
    @AccountName NVARCHAR(100),
    @Password NVARCHAR(100),
    @FullName NVARCHAR(100),
    @Email NVARCHAR(200),
    @Tell NVARCHAR(50),
    @DateCreated DATETIME
AS
BEGIN
    UPDATE [Account]
    SET Password = @Password, FullName = @FullName, Email = @Email,
        Tell = @Tell, DateCreated = @DateCreated
    WHERE AccountName = @AccountName;
END;
GO
/****** Object:  StoredProcedure [dbo].[AddCategory]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddCategory]
    @Name NVARCHAR(1000),
    @Type INT
AS
BEGIN
    INSERT INTO Category (Name, Type)
    VALUES (@Name, @Type);

    SELECT SCOPE_IDENTITY() AS NewCategoryID; -- Lấy ID vừa thêm
END;
GO
/****** Object:  StoredProcedure [dbo].[BillDetails_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BillDetails_Delete]
    @ID INT
AS
BEGIN
    DELETE FROM [BillDetails]
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[BillDetails_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BillDetails_GetAll]
AS
SELECT * FROM [BillDetails];
GO
/****** Object:  StoredProcedure [dbo].[BillDetails_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BillDetails_Insert]
    @InvoiceID INT,
    @FoodID INT,
    @Quantity INT
AS
BEGIN
    INSERT INTO BillDetails (InvoiceID, FoodID, Quantity)
    VALUES (@InvoiceID, @FoodID, @Quantity);
END;
GO
/****** Object:  StoredProcedure [dbo].[BillDetails_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BillDetails_InsertUpdateDelete]
    @ID INT OUTPUT,
    @InvoiceID INT,
    @FoodID INT,
    @Quantity INT,
    @Action INT
AS
BEGIN
    IF @Action = 0 -- Thêm mới
    BEGIN
        INSERT INTO [BillDetails] ([InvoiceID], [FoodID], [Quantity])
        VALUES (@InvoiceID, @FoodID, @Quantity)
        SET @ID = SCOPE_IDENTITY()
    END
    ELSE IF @Action = 1 -- Cập nhật
    BEGIN
        UPDATE [BillDetails]
        SET [InvoiceID] = @InvoiceID, [FoodID] = @FoodID, [Quantity] = @Quantity
        WHERE [ID] = @ID
    END
    ELSE IF @Action = 2 -- Xóa
    BEGIN
        DELETE FROM [BillDetails]
        WHERE [ID] = @ID
    END
END
GO
/****** Object:  StoredProcedure [dbo].[BillDetails_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BillDetails_Update]
    @ID INT,
    @InvoiceID INT,
    @FoodID INT,
    @Quantity INT
AS
BEGIN
    UPDATE [BillDetails]
    SET InvoiceID = @InvoiceID, FoodID = @FoodID, Quantity = @Quantity
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Bills_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Bills_Delete]
    @ID INT
AS
BEGIN
    DELETE FROM [Bills]
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Bills_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Bills_GetAll]
AS
SELECT * FROM [Bills];
GO
/****** Object:  StoredProcedure [dbo].[Bills_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Bills_Insert]
@Name NVARCHAR(1000),
@TableID INT,
@Amount INT,
@Discount FLOAT,
@Tax FLOAT,
@CheckoutDate DATETIME,
@Account NVARCHAR(100),
@Status BIT
AS
BEGIN
    INSERT INTO Bills (Name, TableID, Amount, Discount, Tax, CheckoutDate, Account, Status)
    VALUES (@Name, @TableID, @Amount, @Discount, @Tax, @CheckoutDate, @Account, @Status);
END;
GO
/****** Object:  StoredProcedure [dbo].[Bills_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE or alter PROCEDURE [dbo].[Bills_InsertUpdateDelete]
    @ID INT OUTPUT,
    @Name NVARCHAR(1000),
    @TableID INT,
    @Amount INT,
    @Discount FLOAT,
    @Tax FLOAT,
    @Status BIT,
    @CheckoutDate SMALLDATETIME,
    @Account NVARCHAR(100),
    @Total DECIMAL(18, 2),
    @Action INT
AS
BEGIN
    IF @Action = 0 -- Insert
    BEGIN
        INSERT INTO Bills ([Name], TableID, Amount, Discount, Tax, [Status], CheckoutDate, Account, Total)
        VALUES (@Name, @TableID, @Amount, @Discount, @Tax, @Status, @CheckoutDate, @Account, @Total);

        SET @ID = SCOPE_IDENTITY();
    END
    ELSE IF @Action = 1 -- Update
    BEGIN
        UPDATE Bills
        SET [Name] = @Name,
            TableID = @TableID,
            Amount = @Amount,
            Discount = @Discount,
            Tax = @Tax,
            [Status] = @Status,
            CheckoutDate = @CheckoutDate,
            Account = @Account,
            Total = @Total
        WHERE ID = @ID;
    END
    ELSE IF @Action = 2 -- Delete
    BEGIN
        DELETE FROM Bills WHERE ID = @ID;
    END
END

GO
/****** Object:  StoredProcedure [dbo].[Bills_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Bills_Update]
    @ID INT,
    @Name NVARCHAR(100),
    @TableID INT,
    @Amount FLOAT,
    @Discount FLOAT,
    @Tax FLOAT,
    @Status INT,
    @CheckoutDate DATETIME,
    @Account NVARCHAR(100)
AS
BEGIN
    UPDATE [Bills]
    SET Name = @Name, TableID = @TableID, Amount = @Amount, Discount = @Discount,
        Tax = @Tax, Status = @Status, CheckoutDate = @CheckoutDate, Account = @Account
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Category_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Category_Delete]
    @ID INT
AS
BEGIN
    DELETE FROM [Category]
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Category_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Category_GetAll]
AS
SELECT * FROM Category
GO
/****** Object:  StoredProcedure [dbo].[Category_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Category_Insert]
    @Name NVARCHAR(1000),
    @Type INT
AS
BEGIN
    INSERT INTO [Category] (Name, Type)
    VALUES (@Name, @Type);
END;
GO
/****** Object:  StoredProcedure [dbo].[Category_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Category_InsertUpdateDelete]
    @ID INT OUTPUT,
    @Name NVARCHAR(1000),
    @Type INT,
    @Action INT
AS
BEGIN
    IF @Action = 1
    BEGIN
        INSERT INTO Category (Name, Type)
        VALUES (@Name, @Type);

        SET @ID = SCOPE_IDENTITY();
    END
    ELSE IF @Action = 2
    BEGIN
        UPDATE Category
        SET Name = @Name, Type = @Type
        WHERE ID = @ID;
    END
    ELSE IF @Action = 3
    BEGIN
        DELETE FROM Category
        WHERE ID = @ID;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[Category_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Category_Update]
    @ID INT,
    @Name NVARCHAR(1000),
    @Type INT
AS
BEGIN
    UPDATE [Category]
    SET Name = @Name, Type = @Type
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Food_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Food_Delete]
    @ID INT
AS
BEGIN
    DELETE FROM [Food]
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Food_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Food_GetAll]
AS
SELECT * FROM Food

GO
/****** Object:  StoredProcedure [dbo].[Food_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Food_Insert]
    @ID INT OUTPUT,
    @Name NVARCHAR(1000),
    @Unit NVARCHAR(100),
    @FoodCategoryID INT,
    @Price INT,
    @Notes NVARCHAR(2000)
AS
BEGIN
    INSERT INTO [Food] ([Name], [Unit], [FoodCategoryID], [Price], [Notes])
    VALUES (@Name, @Unit, @FoodCategoryID, @Price, @Notes);

	SELECT @ID = SCOPE_IDENTITY();
END;
GO
/****** Object:  StoredProcedure [dbo].[Food_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Food_InsertUpdateDelete]
    @ID INT,
    @Name NVARCHAR(1000),
    @Unit NVARCHAR(100),
    @FoodCategoryID INT,
    @Price INT,
    @Notes NVARCHAR(3000),
    @Action INT
AS
BEGIN
    IF @Action = 1 -- Thêm mới
    BEGIN
        INSERT INTO Food (Name, Unit, FoodCategoryID, Price, Notes)
        VALUES (@Name, @Unit, @FoodCategoryID, @Price, @Notes)
    END
    ELSE IF @Action = 2 -- Cập nhật
    BEGIN
        UPDATE Food
        SET Name = @Name, Unit = @Unit, FoodCategoryID = @FoodCategoryID, Price = @Price, Notes = @Notes
        WHERE ID = @ID
    END
    ELSE IF @Action = 3 -- Xóa
    BEGIN
        DELETE FROM Food WHERE ID = @ID
    END
END
GO
/****** Object:  StoredProcedure [dbo].[Food_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Food_Update]
    @ID INT,
    @Name NVARCHAR(1000),
    @Unit NVARCHAR(100),
    @FoodCategoryID INT,
    @Price FLOAT,
    @Notes NVARCHAR(2000)
AS
BEGIN
    UPDATE [Food]
    SET Name = @Name, Unit = @Unit, FoodCategoryID = @FoodCategoryID, 
        Price = @Price, Notes = @Notes
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetBillDetails]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetBillDetails]
    @BillID INT
AS
BEGIN
    SELECT 
        bd.ID AS DetailID, 
        f.Name AS FoodName, 
        bd.Quantity, 
        f.Price, 
        (bd.Quantity * f.Price) AS TotalPrice 
    FROM BillDetails bd
    JOIN Food f ON bd.FoodID = f.ID
    WHERE bd.InvoiceID = @BillID;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetBillsByDateRange]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetBillsByDateRange]
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SELECT 
        ID, 
        Name, 
        Amount, 
        Discount, 
        (Amount - Discount) AS NetAmount, 
        CheckoutDate 
    FROM Bills
    WHERE CheckoutDate BETWEEN @StartDate AND @EndDate;
END;
GO
/****** Object:  StoredProcedure [dbo].[MergeTables]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MergeTables]
    @TableID1 INT,
    @TableID2 INT
AS
BEGIN
    UPDATE [Bills]
    SET TableID = @TableID1
    WHERE TableID = @TableID2;

    DELETE FROM [Table] WHERE ID = @TableID2;
END;
GO
/****** Object:  StoredProcedure [dbo].[Role_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Role_Delete]
    @ID INT
AS
BEGIN
    DELETE FROM [Role]
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Role_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Role_GetAll]
AS
SELECT * FROM [Role];
GO
/****** Object:  StoredProcedure [dbo].[Role_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Role_Insert]
    @RoleName NVARCHAR(1000),
    @Path NVARCHAR(3000),
    @Notes NVARCHAR(3000)
AS
BEGIN
    INSERT INTO [Role] (RoleName, [Path], Notes)
    VALUES (@RoleName, @Path, @Notes);
END
GO
/****** Object:  StoredProcedure [dbo].[Role_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Role_InsertUpdateDelete]
    @ID INT OUTPUT,
    @RoleName NVARCHAR(1000),
    @Path NVARCHAR(3000),
    @Notes NVARCHAR(3000),
    @Action INT
AS
BEGIN
    IF @Action = 0 -- Thêm mới
    BEGIN
        INSERT INTO [Role] ([RoleName], [Path], [Notes])
        VALUES (@RoleName, @Path, @Notes)
        SET @ID = SCOPE_IDENTITY()
    END
    ELSE IF @Action = 1 -- Cập nhật
    BEGIN
        UPDATE [Role]
        SET [RoleName] = @RoleName, [Path] = @Path, [Notes] = @Notes
        WHERE [ID] = @ID
    END
    ELSE IF @Action = 2 -- Xóa
    BEGIN
        DELETE FROM [Role]
        WHERE [ID] = @ID
    END
END
GO
/****** Object:  StoredProcedure [dbo].[Role_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Role_Update]
    @ID INT,
    @RoleName NVARCHAR(100),
    @Path NVARCHAR(200),
    @Notes NVARCHAR(200)
AS
BEGIN
    UPDATE [Role]
    SET RoleName = @RoleName, Path = @Path, Notes = @Notes
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[RoleAccount_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleAccount_Delete]
    @ID INT
AS
BEGIN
    DELETE FROM [RoleAccount]
    WHERE RoleID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[RoleAccount_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleAccount_GetAll]
AS
SELECT * FROM [RoleAccount];
GO
/****** Object:  StoredProcedure [dbo].[RoleAccount_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleAccount_Insert]
    @RoleID INT,
    @AccountName NVARCHAR(100),
    @Actived BIT,
    @Notes NVARCHAR(200)
AS
BEGIN
    INSERT INTO RoleAccount (RoleID, AccountName, Actived, Notes)
    VALUES (@RoleID, @AccountName, @Actived, @Notes);
END;
GO
/****** Object:  StoredProcedure [dbo].[RoleAccount_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleAccount_InsertUpdateDelete]
    @RoleID INT,
    @AccountName NVARCHAR(100),
    @Actived BIT,
    @Notes NVARCHAR(3000),
    @Action INT
AS
BEGIN
    IF @Action = 0 -- Thêm mới
    BEGIN
        INSERT INTO [RoleAccount] ([RoleID], [AccountName], [Actived], [Notes])
        VALUES (@RoleID, @AccountName, @Actived, @Notes)
    END
    ELSE IF @Action = 1 -- Cập nhật
    BEGIN
        UPDATE [RoleAccount]
        SET [Actived] = @Actived, [Notes] = @Notes
        WHERE [RoleID] = @RoleID AND [AccountName] = @AccountName
    END
    ELSE IF @Action = 2 -- Xóa
    BEGIN
        DELETE FROM [RoleAccount]
        WHERE [RoleID] = @RoleID AND [AccountName] = @AccountName
    END
END
GO
/****** Object:  StoredProcedure [dbo].[RoleAccount_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RoleAccount_Update]
    @ID INT,
    @RoleID INT,
    @AccountName NVARCHAR(100),
    @Actived BIT,
    @Notes NVARCHAR(200)
AS
BEGIN
    UPDATE [RoleAccount]
    SET RoleID = @RoleID, AccountName = @AccountName,
        Actived = @Actived, Notes = @Notes
    WHERE RoleID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[SalesReport_ByCategory]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SalesReport_ByCategory]
    @Date DATE
AS
BEGIN
    SELECT c.Name AS CategoryName, SUM(bd.Quantity * f.Price) AS TotalSales
    FROM [BillDetails] bd
    JOIN [Food] f ON bd.FoodID = f.ID
    JOIN [Category] c ON f.FoodCategoryID = c.ID
    JOIN [Bills] b ON bd.InvoiceID = b.ID
    WHERE CAST(b.CheckoutDate AS DATE) = @Date
    GROUP BY c.Name;
END;
GO
/****** Object:  StoredProcedure [dbo].[SplitTable]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SplitTable]
    @OldTableID INT,
    @NewTableName NVARCHAR(100),
    @Capacity INT
AS
BEGIN
    INSERT INTO [Table] (Name, Status, Capacity)
    VALUES (@NewTableName, 0, @Capacity);

    DECLARE @NewTableID INT = SCOPE_IDENTITY();

    UPDATE [Bills]
    SET TableID = @NewTableID
    WHERE TableID = @OldTableID AND Status = 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[Table_Delete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Table_Delete]
    @ID INT
AS
BEGIN
    DELETE FROM [Table]
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[Table_GetAll]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Table_GetAll]
AS
SELECT * FROM [Table];
GO
/****** Object:  StoredProcedure [dbo].[Table_Insert]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Table_Insert]
    @Name NVARCHAR(100),
    @Status INT,
    @Capacity INT
AS
BEGIN
    INSERT INTO [Table] (Name, Status, Capacity)
    VALUES (@Name, @Status, @Capacity);
END;
GO
/****** Object:  StoredProcedure [dbo].[Table_InsertUpdateDelete]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Table_InsertUpdateDelete]
    @ID INT OUTPUT,
    @Name NVARCHAR(1000) = NULL,
    @Status INT = NULL,
    @Capacity INT = NULL,
    @Action INT
AS
BEGIN
    IF @Action = 0 -- Insert
    BEGIN
        INSERT INTO [Table] ([Name], [Status], Capacity)
        VALUES (@Name, @Status, @Capacity);
        SET @ID = SCOPE_IDENTITY();
    END
    ELSE IF @Action = 1 -- Update
    BEGIN
        UPDATE [Table]
        SET [Name] = @Name,
            [Status] = @Status,
            Capacity = @Capacity
        WHERE ID = @ID;
    END
    ELSE IF @Action = 2 -- Delete
    BEGIN
        DELETE FROM [Table] WHERE ID = @ID;
    END
END;

GO
/****** Object:  StoredProcedure [dbo].[Table_Update]    Script Date: 27-Dec-24 15:01:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Table_Update]
    @ID INT,
    @Name NVARCHAR(100),
    @Status INT,
    @Capacity INT
AS
BEGIN
    UPDATE [Table]
    SET Name = @Name, Status = @Status, Capacity = @Capacity
    WHERE ID = @ID;
END;
GO
