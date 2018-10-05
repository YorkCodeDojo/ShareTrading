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

CREATE TABLE dbo.ShareTransaction
(
	ID					UNIQUEIDENTIFIER	NOT NULL,
	AccountNumber		UNIQUEIDENTIFIER	NOT NULL,
	ProductCode			VARCHAR(100)		NOT NULL,
	Quantity			INT					NOT NULL,
	DateAndTime			DATETIME2(7)		NOT NULL,
	UnitPrice			INT					NOT NULL,
	TotalValue			INT					NOT NULL,
CONSTRAINT  pk_ShareTransaction  PRIMARY KEY NONCLUSTERED (ID),
CONSTRAINT fk_ShareTransaction_AccountNumbre FOREIGN KEY (AccountNumber) REFERENCES dbo.Accounts(AccountNumber)
)
GO

CREATE CLUSTERED INDEX [ind_ShareTransaction+AccountNumber] ON dbo.ShareTransaction(AccountNumber)
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
		  ST.ProductCode
	 FROM dbo.ShareTransaction ST
	WHERE ST.AccountNumber = @AccountNumber
	GROUP BY ProductCode
	  
   SELECT IsNull(SUM(ST.TotalValue),0) AS TotalFromTransactions
	 FROM dbo.ShareTransaction ST
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
		  ST.ProductCode
	 FROM dbo.ShareTransaction ST
	WHERE ST.AccountNumber = @AccountNumber
	ORDER BY ST.ID

END
GO