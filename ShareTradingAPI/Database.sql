--USE master
--GO

--DROP DATABASE ShareTrading
--GO

CREATE DATABASE ShareTrading
GO

USE ShareTrading
GO

CREATE TABLE dbo.Accounts
(
	AccountNumber		UNIQUEIDENTIFIER	NOT NULL,
	AccountName			NVARCHAR(100)		NOT NULL,
	OpeningCash			INT					NOT NULL,
CONSTRAINT pk_Accounts PRIMARY KEY (AccountNumber)
)
GO

CREATE TABLE dbo.Products
(
	ID					INT					NOT NULL,
	ProductCode			VARCHAR(100)		NOT NULL
CONSTRAINT pk_Products PRIMARY KEY (ID)
)
GO

INSERT INTO dbo.Products (ID, ProductCode) SELECT 1, 'ProductA'
GO
INSERT INTO dbo.Products (ID, ProductCode) SELECT 2, 'ProductB'
GO
INSERT INTO dbo.Products (ID, ProductCode) SELECT 3, 'ProductC'
GO
INSERT INTO dbo.Products (ID, ProductCode) SELECT 4, 'ProductD'
GO

CREATE TABLE dbo.ShareTransactions
(
	ID					UNIQUEIDENTIFIER	NOT NULL,
	AccountNumber		UNIQUEIDENTIFIER	NOT NULL,
	ProductID			INT					NOT NULL,
	Quantity			INT					NOT NULL,
	DateAndTime			DATETIME2(7)		NOT NULL,
	UnitPrice			INT					NOT NULL,
	TotalValue			INT					NOT NULL,
CONSTRAINT  pk_ShareTransactions  PRIMARY KEY NONCLUSTERED (ID),
CONSTRAINT fk_ShareTransactions_AccountNumber FOREIGN KEY (AccountNumber) REFERENCES dbo.Accounts(AccountNumber),
CONSTRAINT fk_ShareTransactions_ProductCode FOREIGN KEY (ProductID) REFERENCES dbo.Products(ID)
)
GO

CREATE CLUSTERED INDEX [ind_ShareTransactions+AccountNumber] ON dbo.ShareTransactions(AccountNumber)
GO
CREATE INDEX [ind_ShareTransactions+ProductID] ON dbo.ShareTransactions(ProductID)
GO

CREATE PROCEDURE dbo.usp_CreateAccount(@AccountNumber		UNIQUEIDENTIFIER,
									   @AccountName			NVARCHAR(100),
									   @OpeningCash			INT) AS
BEGIN

	SET NOCOUNT ON

	INSERT INTO dbo.Accounts (AccountNumber, AccountName, OpeningCash)
	SELECT @AccountNumber, @AccountName, @OpeningCash
END
GO

CREATE PROCEDURE dbo.usp_GetAccount(@AccountNumber		UNIQUEIDENTIFIER) AS
BEGIN
	
	SET NOCOUNT ON

   SELECT A.AccountName,
	      A.AccountNumber,
		  A.OpeningCash
	 FROM dbo.Accounts A
	WHERE A.AccountNumber = @AccountNumber

   SELECT SUM(ST.Quantity) AS CurrentQuantity,
		  P.ProductCode
	 FROM dbo.ShareTransactions ST
	 JOIN dbo.Products P ON P.ID = ST.ProductID
	WHERE ST.AccountNumber = @AccountNumber
	GROUP BY ProductCode
	  
   SELECT IsNull(SUM(ST.TotalValue),0) AS TotalFromTransactions
	 FROM dbo.ShareTransactions ST
	WHERE ST.AccountNumber = @AccountNumber

END
GO


CREATE PROCEDURE dbo.usp_GetTransactionsForAccount(@AccountNumber		UNIQUEIDENTIFIER) AS
BEGIN
	
	SET NOCOUNT ON

   SELECT ST.AccountNumber,
	      ST.DateAndTime,
		  ST.ID,
		  ST.Quantity,
		  ST.TotalValue,
		  ST.UnitPrice,
		  P.ProductCode
	 FROM dbo.ShareTransactions ST
	 JOIN dbo.Products P ON P.ID = ST.ProductID
	WHERE ST.AccountNumber = @AccountNumber
	ORDER BY ST.DateAndTime

END
GO


CREATE PROCEDURE dbo.usp_CreateTransaction(@AccountNumber		UNIQUEIDENTIFIER,
										   @DateAndTime			DATETIME2(7),
										   @ID					UNIQUEIDENTIFIER,
										   @ProductCode			VARCHAR(100),
										   @Quantity			INT,
										   @TotalValue			INT,
										   @UnitPrice			INT) AS
BEGIN
	
	SET NOCOUNT ON

	INSERT INTO dbo.ShareTransactions(AccountNumber, DateAndTime, ID, ProductID, Quantity, TotalValue, UnitPrice)
	SELECT @AccountNumber, @DateAndTime, @ID, P.ID, @Quantity, @TotalValue, @UnitPrice
	 FROM dbo.Products P 
	WHERE P.ProductCode = @ProductCode

END
GO