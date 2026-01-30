-- Copy-paste this code in SSMS:
USE QuotationAppDB;

CREATE TABLE Quotations (
    Id int IDENTITY(1,1) PRIMARY KEY,
    QuotationNo int NOT NULL,
    Date datetime NOT NULL,
    Enquiry nvarchar(255),
    CustomerName nvarchar(255),
    Address nvarchar(500),
    KindAttn nvarchar(255),
    GSTNo nvarchar(50),
    Terms nvarchar(500),
    Subtotal decimal(18,2),
    RGPNo nvarchar(50),
    RGPDate nvarchar(50),
    ChallanNo nvarchar(50),
    ChallanDate nvarchar(50)
);

CREATE TABLE QuotationItems (
    Id int IDENTITY(1,1) PRIMARY KEY,
    QuotationId int REFERENCES Quotations(Id),
    SrNo int,
    Particulars nvarchar(255),
    Unit nvarchar(50),
    Qty decimal(18,2),
    Rate decimal(18,2),
    Amount decimal(18,2)
);

CREATE TABLE Users (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(100),
    Username nvarchar(50) UNIQUE,
    Password nvarchar(255),
    PhotoPath nvarchar(500)
);

CREATE TABLE Clients (
    Id int IDENTITY(1,1) PRIMARY KEY,
    VendorCode nvarchar(50),
    CustomerName nvarchar(255),
    Address nvarchar(500),
    GSTNo nvarchar(50)
);
