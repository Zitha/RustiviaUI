namespace IntroductionMVC5.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 20),
                        IdNumber = c.String(nullable: false, maxLength: 15),
                        Gender = c.String(nullable: false),
                        Surname = c.String(maxLength: 20),
                        IdLocation = c.String(nullable: false),
                        ImageName = c.String(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                        SupplierInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SupplierInfoes", t => t.SupplierInfo_Id)
                .Index(t => t.SupplierInfo_Id);
            
            CreateTable(
                "dbo.SupplierInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(nullable: false, maxLength: 50),
                        Logo = c.String(),
                        Address = c.String(),
                        TellNumber = c.String(),
                        Suppliercode = c.String(),
                        CompanyRegNumber = c.String(nullable: false),
                        VatNumber = c.String(),
                        Legalnote = c.String(),
                        UserId = c.String(),
                        IsBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankName = c.String(nullable: false),
                        AccountNumber = c.String(nullable: false),
                        BranchNumber = c.String(),
                        SupplierInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SupplierInfoes", t => t.SupplierInfo_Id)
                .Index(t => t.SupplierInfo_Id);
            
            CreateTable(
                "dbo.SupplierProducts",
                c => new
                    {
                        SupplierId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        SupplierPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.SupplierId, t.ProductId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.SupplierInfoes", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        RustiviaPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trucks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TruckRegNumber = c.String(nullable: false, maxLength: 15),
                        Image = c.String(),
                        Own = c.Boolean(nullable: false),
                        SupplierInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SupplierInfoes", t => t.SupplierInfo_Id)
                .Index(t => t.SupplierInfo_Id);
            
            CreateTable(
                "dbo.WeighBridgeInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstMass = c.Long(),
                        SecondMass = c.Long(),
                        NettMass = c.Long(nullable: false),
                        DateIn = c.DateTime(nullable: false),
                        DateOut = c.DateTime(),
                        Comments = c.String(),
                        Product = c.String(),
                        Truck_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trucks", t => t.Truck_Id)
                .Index(t => t.Truck_Id);
            
            CreateTable(
                "dbo.PastelInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PastelNumber = c.String(nullable: false),
                        Date = c.DateTime(),
                        FileLocation = c.String(nullable: false),
                        VatFile = c.String(),
                        WeighBridgeInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WeighBridgeInfoes", t => t.WeighBridgeInfo_Id, cascadeDelete: true)
                .Index(t => t.WeighBridgeInfo_Id);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        PaymentType = c.String(),
                        PaymentReference = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Driver_Id = c.Int(nullable: false),
                        Truck_Id = c.Int(),
                        WeighBridgeInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.Driver_Id, cascadeDelete: true)
                .ForeignKey("dbo.Trucks", t => t.Truck_Id)
                .ForeignKey("dbo.WeighBridgeInfoes", t => t.WeighBridgeInfo_Id, cascadeDelete: true)
                .Index(t => t.Driver_Id)
                .Index(t => t.Truck_Id)
                .Index(t => t.WeighBridgeInfo_Id);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 20),
                        Password = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 50),
                        Logo = c.String(),
                        Address = c.String(),
                        TellNumber = c.String(),
                        Suppliercode = c.String(),
                        CompanyRegNumber = c.String(nullable: false),
                        VatNumber = c.String(),
                        Legalnote = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Reference = c.String(nullable: false, maxLength: 128),
                        DateIn = c.DateTime(nullable: false),
                        DateOut = c.DateTime(),
                        Status = c.String(),
                        Customer_Id = c.Int(),
                        Transporter_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Reference)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Transporters", t => t.Transporter_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Transporter_Id);
            
            CreateTable(
                "dbo.Containers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContainerNumber = c.String(nullable: false),
                        Sealnumber = c.String(),
                        GrossWeight = c.Long(nullable: false),
                        TareWeight = c.Long(nullable: false),
                        NettWeight = c.Long(nullable: false),
                        Product = c.String(),
                        Status = c.String(),
                        DeliveryNote = c.String(),
                        DepotName = c.String(),
                        Paid = c.Boolean(nullable: false),
                        TruckRegNumber = c.String(),
                        Invoice1 = c.String(),
                        Invoice2 = c.String(),
                        DateIn = c.DateTime(nullable: false),
                        Booking_Reference = c.String(maxLength: 128),
                        WeighBridgeInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.Booking_Reference)
                .ForeignKey("dbo.WeighBridgeInfoes", t => t.WeighBridgeInfo_Id)
                .Index(t => t.Booking_Reference)
                .Index(t => t.WeighBridgeInfo_Id);
            
            CreateTable(
                "dbo.Transporters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        PastelNo = c.String(),
                        AutoGenNumber = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PettyAccount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCleared = c.Boolean(nullable: false),
                        User = c.String(),
                        Account_Id = c.Int(),
                        PaymentType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentType_Id)
                .Index(t => t.Account_Id)
                .Index(t => t.PaymentType_Id);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EndDayBalances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        SystemUser = c.String(),
                        OpeningBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClosingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Type = c.String(nullable: false),
                        Reference = c.String(nullable: false),
                        ExtraInfo = c.String(),
                        User = c.String(),
                        AutoGenNumber = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PettyAccount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCleared = c.Boolean(nullable: false),
                        PastelNo = c.String(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        TruckRegNumber = c.String(),
                        ExtraInfo = c.String(),
                        Customer_Id = c.Int(nullable: false),
                        WeighBridgeInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .ForeignKey("dbo.WeighBridgeInfoes", t => t.WeighBridgeInfo_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.WeighBridgeInfo_Id);
            
            CreateTable(
                "dbo.ArsloCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        Address = c.String(),
                        TellNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArsloProfomas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfomaNumber = c.String(nullable: false),
                        UCRNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        Status = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Location = c.String(),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloCustomers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.ArsloInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        Reference = c.String(),
                        InvoiceLocation = c.String(),
                        PointOfLoading = c.String(),
                        PointOfDelivery = c.String(),
                        BookingNumber = c.String(),
                        VesselNumber = c.String(),
                        Customer_Id = c.Int(),
                        Profoma_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloCustomers", t => t.Customer_Id)
                .ForeignKey("dbo.ArsloProfomas", t => t.Profoma_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Profoma_Id);
            
            CreateTable(
                "dbo.ArsloInvoiceItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ArsloInvoice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloInvoices", t => t.ArsloInvoice_Id)
                .Index(t => t.ArsloInvoice_Id);
            
            CreateTable(
                "dbo.ArsloProfomaDrawDowns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Reference = c.String(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        ArsloProfoma_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloProfomas", t => t.ArsloProfoma_Id)
                .Index(t => t.ArsloProfoma_Id);
            
            CreateTable(
                "dbo.ArsloProfomaItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ArsloProfoma_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloProfomas", t => t.ArsloProfoma_Id)
                .Index(t => t.ArsloProfoma_Id);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArsloProfomaItems", "ArsloProfoma_Id", "dbo.ArsloProfomas");
            DropForeignKey("dbo.ArsloProfomaDrawDowns", "ArsloProfoma_Id", "dbo.ArsloProfomas");
            DropForeignKey("dbo.ArsloInvoices", "Profoma_Id", "dbo.ArsloProfomas");
            DropForeignKey("dbo.ArsloInvoiceItems", "ArsloInvoice_Id", "dbo.ArsloInvoices");
            DropForeignKey("dbo.ArsloInvoices", "Customer_Id", "dbo.ArsloCustomers");
            DropForeignKey("dbo.ArsloProfomas", "Customer_Id", "dbo.ArsloCustomers");
            DropForeignKey("dbo.Sales", "WeighBridgeInfo_Id", "dbo.WeighBridgeInfoes");
            DropForeignKey("dbo.Sales", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Receipts", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Payments", "PaymentType_Id", "dbo.PaymentTypes");
            DropForeignKey("dbo.Payments", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Bookings", "Transporter_Id", "dbo.Transporters");
            DropForeignKey("dbo.Bookings", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Containers", "WeighBridgeInfo_Id", "dbo.WeighBridgeInfoes");
            DropForeignKey("dbo.Containers", "Booking_Reference", "dbo.Bookings");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Purchases", "WeighBridgeInfo_Id", "dbo.WeighBridgeInfoes");
            DropForeignKey("dbo.Purchases", "Truck_Id", "dbo.Trucks");
            DropForeignKey("dbo.Purchases", "Driver_Id", "dbo.Drivers");
            DropForeignKey("dbo.PastelInfoes", "WeighBridgeInfo_Id", "dbo.WeighBridgeInfoes");
            DropForeignKey("dbo.WeighBridgeInfoes", "Truck_Id", "dbo.Trucks");
            DropForeignKey("dbo.Trucks", "SupplierInfo_Id", "dbo.SupplierInfoes");
            DropForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.SupplierInfoes");
            DropForeignKey("dbo.SupplierProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Drivers", "SupplierInfo_Id", "dbo.SupplierInfoes");
            DropForeignKey("dbo.BankAccounts", "SupplierInfo_Id", "dbo.SupplierInfoes");
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.ArsloProfomaItems", new[] { "ArsloProfoma_Id" });
            DropIndex("dbo.ArsloProfomaDrawDowns", new[] { "ArsloProfoma_Id" });
            DropIndex("dbo.ArsloInvoiceItems", new[] { "ArsloInvoice_Id" });
            DropIndex("dbo.ArsloInvoices", new[] { "Profoma_Id" });
            DropIndex("dbo.ArsloInvoices", new[] { "Customer_Id" });
            DropIndex("dbo.ArsloProfomas", new[] { "Customer_Id" });
            DropIndex("dbo.Sales", new[] { "WeighBridgeInfo_Id" });
            DropIndex("dbo.Sales", new[] { "Customer_Id" });
            DropIndex("dbo.Receipts", new[] { "Account_Id" });
            DropIndex("dbo.Payments", new[] { "PaymentType_Id" });
            DropIndex("dbo.Payments", new[] { "Account_Id" });
            DropIndex("dbo.Containers", new[] { "WeighBridgeInfo_Id" });
            DropIndex("dbo.Containers", new[] { "Booking_Reference" });
            DropIndex("dbo.Bookings", new[] { "Transporter_Id" });
            DropIndex("dbo.Bookings", new[] { "Customer_Id" });
            DropIndex("dbo.Purchases", new[] { "WeighBridgeInfo_Id" });
            DropIndex("dbo.Purchases", new[] { "Truck_Id" });
            DropIndex("dbo.Purchases", new[] { "Driver_Id" });
            DropIndex("dbo.PastelInfoes", new[] { "WeighBridgeInfo_Id" });
            DropIndex("dbo.WeighBridgeInfoes", new[] { "Truck_Id" });
            DropIndex("dbo.Trucks", new[] { "SupplierInfo_Id" });
            DropIndex("dbo.SupplierProducts", new[] { "ProductId" });
            DropIndex("dbo.SupplierProducts", new[] { "SupplierId" });
            DropIndex("dbo.BankAccounts", new[] { "SupplierInfo_Id" });
            DropIndex("dbo.Drivers", new[] { "SupplierInfo_Id" });
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.ArsloProfomaItems");
            DropTable("dbo.ArsloProfomaDrawDowns");
            DropTable("dbo.ArsloInvoiceItems");
            DropTable("dbo.ArsloInvoices");
            DropTable("dbo.ArsloProfomas");
            DropTable("dbo.ArsloCustomers");
            DropTable("dbo.Sales");
            DropTable("dbo.Receipts");
            DropTable("dbo.EndDayBalances");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Payments");
            DropTable("dbo.Accounts");
            DropTable("dbo.Transporters");
            DropTable("dbo.Containers");
            DropTable("dbo.Bookings");
            DropTable("dbo.Customers");
            DropTable("dbo.Users");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.Purchases");
            DropTable("dbo.PastelInfoes");
            DropTable("dbo.WeighBridgeInfoes");
            DropTable("dbo.Trucks");
            DropTable("dbo.Products");
            DropTable("dbo.SupplierProducts");
            DropTable("dbo.BankAccounts");
            DropTable("dbo.SupplierInfoes");
            DropTable("dbo.Drivers");
        }
    }
}
