CREATE DATABASE  IF NOT EXISTS TussoTech;

CREATE TABLE `aspnetusers` (
  `Id` varchar(128) NOT NULL,
  `IsApproved` tinyint(1) NOT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEndDateUtc` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(128) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`UserName`) USING HASH
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `aspnetroles` (
  `Id` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`Name`) USING HASH
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(128) NOT NULL,
  `RoleId` varchar(128) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_UserId` (`UserId`) USING HASH,
  KEY `IX_RoleId` (`RoleId`) USING HASH,
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `UserId` varchar(128) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
  KEY `IX_UserId` (`UserId`) USING HASH,
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(128) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_UserId` (`UserId`) USING HASH,
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `__migrationhistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ContextKey` varchar(300) NOT NULL,
  `Model` longblob NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE BankStatements  (
	  Id  int NOT NULL auto_increment,
	  DateSent     datetime   NOT NULL,
	  AccountAmount     float   NOT NULL,
	  Location     nvarchar  (100) NULL,
       PRIMARY KEY ( Id));

CREATE TABLE  Companies  (
	  Id  int NOT NULL auto_increment,
	  Name     nvarchar  (100) NULL,
	  Contacts     nvarchar  (100) NULL,
      PRIMARY KEY ( Id));
	  
	  
CREATE TABLE CompanyDocuments  (
	  Id  int NOT NULL auto_increment,
	  Name     nvarchar  (100) NOT NULL,
	  Location     nvarchar  (1000) NULL,
	  Type     nvarchar  (100) NULL,
	  Company_Id     int   NULL,
       PRIMARY KEY ( Id),
       FOREIGN KEY (Company_Id) REFERENCES companies(ID));

CREATE TABLE Customers  (
	  Id  int NOT NULL auto_increment,
	  Name     nvarchar  (100) NULL,
	  Address     nvarchar  (1000) NULL,
	  Contact     nvarchar  (100) NULL,
	  EmailAddress     nvarchar  (100) NULL,
	  VatNumber     nvarchar  (100) NULL,
      PRIMARY KEY ( Id));

CREATE TABLE  Expenses  (
	   Id  int NOT NULL auto_increment,
	  DateSent     datetime   NOT NULL,
	  Type     nvarchar  (100) NULL,
	  PurchaseNumber     nvarchar  (100) NULL,
	  Total     float   NOT NULL,
	  Description     nvarchar  (100) NULL,
	  Location     nvarchar  (1000) NULL,
	  Employee     nvarchar  (100) NULL,
	  Company_Id     int   NULL,
       PRIMARY KEY ( Id),
       FOREIGN KEY (Company_Id) REFERENCES companies(ID));
	   
      
CREATE TABLE  Invoices  (
	   Id  int NOT NULL auto_increment,
	  DateSent     datetime   NOT NULL,
	  InvoiceNumber     nvarchar  (100) NULL,
	  Total     float   NOT NULL,
	  Status     nvarchar  (100) NULL,
	  Description     nvarchar  (1000) NULL,
	  Location     nvarchar  (1000) NULL,
	  Customer_Id     int   NULL,
      PRIMARY KEY ( Id),
       FOREIGN KEY (Customer_Id) REFERENCES customers(ID));
	   
	   
CREATE TABLE Items  (
	  Id  int NOT NULL auto_increment,
	  Description     nvarchar  (1000) NULL,
	  Quantity     int   NOT NULL,
	  UnitPrice     float   NOT NULL,
	  Invoice_Id     int   NULL,
       PRIMARY KEY ( Id),
       FOREIGN KEY (Invoice_Id) REFERENCES invoices(ID));
	   
	   
CREATE TABLE  Resources  (
	  Id  int NOT NULL auto_increment,
	  Description     nvarchar  (1000) NULL,
	  Type     nvarchar  (100) NULL,
	  OutCome     nvarchar  (100) NULL,
	  Location     nvarchar  (1000) NULL,
	  Customer_Id     int   NULL,
	  Company_Id     int   NULL,
	PRIMARY KEY ( Id),
	FOREIGN KEY (Customer_Id) REFERENCES customers(ID),
	FOREIGN KEY (Company_Id) REFERENCES companies(ID));
	
INSERT Companies (  Id  ,   Name  ,   Contacts  ) VALUES (1, N'Tusso Technologies', N'072 631 5461/ 083 473 1660');

INSERT INTO `tussotech`.`aspnetusers`
(`Id`,
`IsApproved`,
`Email`,
`EmailConfirmed`,
`PasswordHash`,
`SecurityStamp`,
`PhoneNumber`,
`PhoneNumberConfirmed`,
`TwoFactorEnabled`,
`LockoutEndDateUtc`,
`LockoutEnabled`,
`AccessFailedCount`,
`UserName`)
VALUES
('129d732d-5d0b-4ad9-9d92-a644e520e821',
0,
'ndavhe@tussotechnologies.co.za',
0,
'AEhxGz39raTjFOejGWqqy71OSDZuuDxwkSVjeHUzvTZC87by3u0AApMRo9JhWRHPog==',
'81b8ad2a-7a9e-48ae-9614-204ff8faa2b9',
null,
0,
0,
null,
1,
0,
'ndavhe@tussotechnologies.co.za');

INSERT INTO `tussotech`.`aspnetusers`
(`Id`,
`IsApproved`,
`Email`,
`EmailConfirmed`,
`PasswordHash`,
`SecurityStamp`,
`PhoneNumber`,
`PhoneNumberConfirmed`,
`TwoFactorEnabled`,
`LockoutEndDateUtc`,
`LockoutEnabled`,
`AccessFailedCount`,
`UserName`)
VALUES
('e8e93d71-294a-4c0c-af0f-a8681e0ae99b',
0,
'riwanise@tussotechnologies.co.za',
0,
'AGyUhZiWZ6KtNKSjj83h8w9RkCZIdz9BLKhSQwjIB+KoVjOMojGJNAVBbEIKdvI+3g==',
'44a4af4d-8472-4df9-94e5-a9871c8e71f9',
null,
0,
0,
null,
1,
0,
'riwanise@tussotechnologies.co.za');

INSERT INTO `tussotech`.`aspnetroles`(`Id`,`Name`)VALUES(1,'Director');

INSERT INTO `tussotech`.`aspnetuserroles`
(`UserId`,
`RoleId`)
VALUES
('129d732d-5d0b-4ad9-9d92-a644e520e821',
1);

INSERT INTO `tussotech`.`aspnetuserroles`
(`UserId`,
`RoleId`)
VALUES
('e8e93d71-294a-4c0c-af0f-a8681e0ae99b',
1);