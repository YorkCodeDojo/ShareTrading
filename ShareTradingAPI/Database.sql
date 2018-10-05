CREATE DATABASE ShareTrading
GO

USE ShareTrading
GO

CREATE TABLE dbo.Accounts
(
	AccountNumber		UNIQUEIDENTIFIER	NOT NULL,
	AccountName			NVARCHAR(100)		NOT NULL,
	Cash				INT					NOT NULL,
	SharesHeld			INT					NOT NULL,
CONSTRAINT pk_Accounts PRIMARY KEY (AccountNumber)
)
GO

CREATE TABLE dbo.ShareTransaction
(
	ID					UNIQUEIDENTIFIER	NOT NULL,
	AccountNumber		UNIQUEIDENTIFIER	NOT NULL,
	Quantity			INT					NOT NULL,
	DateAndTime			DATETIME2(7)		NOT NULL,
	UnitCost			INT					NOT NULL,
	TotalCost			INT					NOT NULL,
CONSTRAINT pk_ShareTransaction PRIMARY KEY (ID),
CONSTRAINT fk_ShareTransaction_AccountNumbre FOREIGN KEY (AccountNumber) REFERENCES dbo.Accounts(AccountNumber)
)
GO

CREATE INDEX [ind_ShareTransaction+AccountNumber] ON dbo.ShareTransaction(AccountNumber)
GO

CREATE PROCEDURE dbo.usp_CreateOrUpdateAccount(@AccountNumber		UNIQUEIDENTIFIER,
											   @AccountName			NVARCHAR(100),
											   @Cash				INT,
											   @SharesHeld			INT) AS
BEGIN

	SET NOCOUNT ON

	UPDATE dbo.Accounts
	   SET AccountName = @AccountName,
	       Cash = @Cash,
		   SharesHeld = @SharesHeld
	 WHERE AccountNumber = @AccountNumber

	 IF @@ROWCOUNT = 0
	 BEGIN
		INSERT INTO dbo.Accounts (AccountNumber, AccountName, Cash, SharesHeld)
		SELECT @AccountNumber, @AccountName, @Cash, @SharesHeld
	 END


END
GO

CREATE PROCEDURE dbo.usp_GetAccount(@AccountNumber		UNIQUEIDENTIFIER) AS
BEGIN
	
	SET NOCOUNT ON

   SELECT A.AccountName,
	      A.AccountNumber,
		  A.Cash,
		  A.SharesHeld
	 FROM dbo.Accounts A
	WHERE A.AccountNumber = @AccountNumber

END
GO


CREATE PROCEDURE dbo.usp_GetTransactionsForAccount(@AccountNumber		UNIQUEIDENTIFIER) AS
BEGIN
	
	SET NOCOUNT ON

   SELECT ST.AccountNumber,
	      ST.DateAndTime,
		  ST.ID,
		  ST.Quantity,
		  ST.TotalCost,
		  ST.UnitCost
	 FROM dbo.ShareTransaction ST
	WHERE ST.AccountNumber = @AccountNumber
	ORDER BY ST.ID

END
GO