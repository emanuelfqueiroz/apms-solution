
GO
Create Database AffiliatePMS;
Go
Use AffiliatePMS
    

-- AFFILIATE AGGREGATION
CREATE TABLE Affiliate (
    Id INT IDENTITY(1,1),
    PublicName NVARCHAR(255),
    CONSTRAINT PK_Affiliate_ID PRIMARY KEY (Id)
);

CREATE TABLE [AppUser]
(
    Id INT IDENTITY(1,1),
    FullName VARCHAR(50) Not NULL,
    Email VARCHAR(50) NOT NULL,
    EncodedPassword VARCHAR(128) NOT NULL,
    [Status] SMALLINT NOT NULL CONSTRAINT DF_AppuserStatus DEFAULT 1,
    [Role] VARCHAR(20) NOT NULL,
    [AffiliateId] INT NULL,
    CONSTRAINT PK_AppUser_Id PRIMARY KEY (Id),
    CONSTRAINT FK_AppUser_AffiliateId FOREIGN KEY (AffiliateId) REFERENCES Affiliate(Id)
);

CREATE UNIQUE INDEX UQ_User_Email ON [AppUser] (Email);



CREATE INDEX IDX_AffiliateDetail_PublicName ON Affiliate(PublicName);

CREATE TABLE AffiliateDetail (
    AffiliateId INT,
    FullName NVARCHAR(255),
    Email NVARCHAR(255),
    Phone1 NVARCHAR(50),
    Phone2 NVARCHAR(50),
    CONSTRAINT PK_AffiliateDetail_AffiliateId  PRIMARY KEY (AffiliateId),
    CONSTRAINT FK_AffiliateDetail_AffiliateId FOREIGN KEY (AffiliateId) REFERENCES Affiliate(Id)
);

CREATE UNIQUE INDEX IDX_AffiliateDetail_Email ON [AffiliateDetail] (Email)


CREATE TABLE AffiliateSocialMedia (
	Id INT IDENTITY(1,1),
    AffiliateId INT,
    [Url] NVARCHAR(255),
    [Type] NVARCHAR(30),
    Followers INT,
    CONSTRAINT PK_AffiliateSocialMedia_Id  PRIMARY KEY (Id),
    CONSTRAINT FK_AffiliateSocialMedia_AffiliteId FOREIGN KEY (AffiliateId) REFERENCES Affiliate(Id),
);

-- For Payments
CREATE TABLE AffiliateBankAccount (
    AffiliateId INT,
    BankName NVARCHAR(255),
    BankBranch NVARCHAR(255),
    BankAccountNumber NVARCHAR(50),
    BankAccountType NVARCHAR(50),
    BankAccountHolder NVARCHAR(255),
    CONSTRAINT PK_AffiliateBankAccount_AffiliateId  PRIMARY KEY (AffiliateId),
    CONSTRAINT FK_AffiliateBankAccount_AffiliateId FOREIGN KEY (AffiliateId) REFERENCES Affiliate(Id),
);

Create TABLE AffiliateAddress (
    AffiliateId INT,
    [Address] NVARCHAR(255),
    City NVARCHAR(255),
    [State] NVARCHAR(255),
    Country NVARCHAR(255),
    ZipCode NVARCHAR(50),
    CONSTRAINT PK_AffiliateAddress_AffiliateId  PRIMARY KEY (AffiliateId),
    CONSTRAINT FK_AffiliateAddress_AffiliateId FOREIGN KEY (AffiliateId) REFERENCES Affiliate(Id),
);

--- CUSTOMERS AGGREGATION
CREATE TABLE AffiliateCustomer (
    Id INT IDENTITY(1,1),
    AffiliateId INT,
    FullName NVARCHAR(255),
    Email NVARCHAR(255),
    Gender NVARCHAR(50),
    BirthDate Date,
    TotalPurchase DECIMAL(18, 2),
    AvgTicket DECIMAL(18, 2),
    CONSTRAINT PK_AffiliateCustomer_Id PRIMARY KEY (Id),
    CONSTRAINT FK_AffiliateCustomer_AffiliateId FOREIGN KEY (AffiliateId) REFERENCES Affiliate(Id),
);

CREATE TABLE AffiliateCustomerTag (
    AffiliateId INT,
    [Tag] NVARCHAR(255),
    Weigth SMALLINT DEFAULT 1,
    CONSTRAINT FK_AffiliateCustomerTag_AffiliateId FOREIGN KEY (AffiliateId) REFERENCES Affiliate(Id),
);

Go



CREATE SCHEMA Sales;
GO
CREATE TABLE Sales.OrderHeader (
    Id uniqueidentifier,
    AffiliateCustomerId INT FOREIGN KEY REFERENCES dbo.AffiliateCustomer(Id),
    TotalAmountItems Money not NULL, -- Total amount of all items in USD currency
    CONSTRAINT PK_Sales_OrderHeader_Id PRIMARY KEY (Id),
    CONSTRAINT FK_Sales_OrderHeader_AffiliateCustomerId FOREIGN KEY (AffiliateCustomerId) REFERENCES AffiliateCustomer(Id),
);

Create Table Sales.Product(
    Id INT ,
    [Name] NVARCHAR(255),
    CONSTRAINT PK_Sales_Product_Id PRIMARY KEY (Id)

);
CREATE TABLE Sales.OrderItem (
	Id uniqueidentifier,
    OrderId uniqueidentifier,
    ProductId INT,
    Quantity INT,
    Price Money,
    TotalAmount Money,
    CONSTRAINT PK_Sales_OrderItem_Id PRIMARY KEY (Id),
    CONSTRAINT FK_Sales_OrderItem_ProductId FOREIGN KEY (ProductId) REFERENCES Sales.Product(Id),
    CONSTRAINT FK_Sales_OrderItem_OrderId FOREIGN KEY (OrderId) REFERENCES Sales.OrderHeader(Id)
);



Go
Print 'Database structure created successfully'

Print 'Inserting Admin'
--Password: "password-test"
Insert into AppUser (FullName, Email, EncodedPassword, [Status], [Role]) Values('Admin', 'admin@example','$argon2id$v=19$m=65536,t=3,p=1$UBhv66QEYyhTetZjS/+FKw$qvJe71XoJ1EIOhW1B/zBo55gJwNbpmt/AlIzAZLCp6g', 1, 'admin')

Print 'Inserting Affiliate'
Insert into Affiliate(PublicName) Values('Jeff') ;

declare @affiliateId int = @@IDENTITY;
print @affiliateId
print CONCAT('AffiliateID: ', @affiliateId)

Insert into AppUser (FullName, Email, EncodedPassword, [Status], [Role], AffiliateId) Values('Jeff Bez_s', 'affiliate@example','$argon2id$v=19$m=65536,t=3,p=1$UBhv66QEYyhTetZjS/+FKw$qvJe71XoJ1EIOhW1B/zBo55gJwNbpmt/AlIzAZLCp6g',1, 'affiliate', @affiliateId)


Insert into AffiliateDetail(AffiliateId, FullName, Email, Phone1) 
Select Id, FullName, Email, '123-456-7890' From AppUser Where Id = @affiliateId;


Insert into AffiliateAddress(AffiliateId, [Address], City, [State], Country, ZipCode)
Select Id, '123 Main St', 'Anytown', 'NY', 'USA', '12345' From AppUser Where Id = @affiliateId;

INsert AffiliateSocialMedia (AffiliateId, [Url], [Type], Followers) 
Values	(@affiliateId, 'https://facebook.com/jeff', 'Facebook', 1000000),
		(@affiliateId, '@jeff', 'Instagram', 2000000);

INsert AffiliateBankAccount (AffiliateId, BankName, BankBranch, BankAccountNumber, BankAccountType, BankAccountHolder)
Values(@affiliateId, 'Bank of America', 'Main Branch', '1234567890', 'Checking', 'Jeff Bez_s');




Print 'Inserting Customers Data'
INSERT INTO AffiliateCustomer (AffiliateId, FullName, Email, Gender, BirthDate, TotalPurchase, AvgTicket)
VALUES  (@affiliateId, 'John Doe', 'john@example.com', 1, '1990-01-01', 10000.00, 600.00),
        (@affiliateId, 'Jane Doe', 'jane@example.com', 2, '1992-02-02', 200000.00, 1000.00),
        (@affiliateId, 'Bob Smith', 'bob@example.com', 1, '1988-03-03', 4000000.00, 12000.00);



		--

print 'Adding More Affiliates'
Insert into Affiliate(PublicName) Values('Affiliate 997')
Insert into AffiliateDetail(AffiliateId, FullName, Email, Phone1) 
Select @@IDENTITY, PublicName, 'email997@example.com', '123-456-7997'
From Affiliate Where Id = 997

Insert into Affiliate(PublicName) Values('Affiliate 998')
Insert into AffiliateDetail(AffiliateId, FullName, Email, Phone1) 
Select Id, PublicName, 'email998@example.com', '123-456-7998' 
From Affiliate Where Id = @@IDENTITY

Insert into Affiliate(PublicName) Values('Affiliate 999')
Insert into AffiliateDetail(AffiliateId, FullName, Email, Phone1) 
Select Id, PublicName, 'email999@example.com', '123-456-7999'
From Affiliate Where Id = @@IDENTITY

