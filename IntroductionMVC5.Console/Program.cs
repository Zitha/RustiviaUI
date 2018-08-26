using System;
using System.Collections.Generic;
using System.Linq;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Models.PettyCash;
using IntroductionMVC5.Models.ArsloTrading;
using RustiviaSolutions.PDFGenerator;
using System.Net.Mail;
using System.Net;

namespace IntroductionMVC5.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing Database...");
            try
            {
                //var context = new DataContext();
                //context.Database.Initialize(true);

                //AddWeighBridgeInfos(context);
                //AddSupplier(context);
                GenerateProfoma();
                //GenerateRandomNumbers();
                GenerateInvoice();
                //var cn = new ContainerInfomationGenerator();
                //cn.PrintContainerInfomation(new Container
                //{
                //    Booking = new Booking
                //    {
                //        Customer = new Customer
                //        {
                //            CustomerName = "Testing"
                //        },
                //        Transporter = new Transporter
                //        {
                //            Name = "Twinkle star"
                //        },
                //    },
                //    ContainerNumber = "MMDH823420",
                //    DateIn = DateTime.Now,
                //    DeliveryNote = "Testing",
                //    GrossWeight = 5000,
                //    NettWeight = 3000,
                //    TareWeight = 2000,
                //    Product = "Testing",
                //    Sealnumber = "Testing",
                //    Status = "Processed",
                //    TruckRegNumber = "daasd",
                //    WeighBridgeInfo = new WeighBridgeInfo
                //    {
                //        DateIn = DateTime.Now.Date,
                //        FirstMass = 10000,
                //        NettMass = 4000,
                //        Product = "Scrap metal steel",
                //        SecondMass = 6000,
                //    }
                //});
                //var lodGenerator = new LoadingSheetGenerator();
                //lodGenerator.PrintTicketSlip();


                Console.WriteLine("Done...");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine("NDAVHE");
                Console.WriteLine(exception.Message);
                Console.ReadLine();
            }
        }

        private static void GenerateRandomNumbers()
        {
            try
            {
                Random rand = new Random();
                Dictionary<int, List<int>> boards = new Dictionary<int, List<int>>();
                List<int> result = new List<int>();
                for (int i = 0; i < 20; i++)
                {

                    while (result.Count < 6)
                    {
                        int curValue = rand.Next(1, 52);
                        if (!result.Exists(value => value == curValue))
                        {
                            result.Add(curValue);
                        }
                        else
                        {
                            curValue = rand.Next(1, 52);
                        }
                    }
                    boards.Add(i, result);
                    result = new List<int>();
                }
                string emailUserName = "info@tussotechnologies.co.za";
                string emailPassword = "Pa$$w0rd";
                string emailHost = "mail.tussotechnologies.co.za";
                int emailPot = 25;

                SmtpClient client = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = emailHost,
                    Timeout = 100000,
                    Port = emailPot,
                    Credentials = new NetworkCredential(emailUserName, emailPassword),
                    EnableSsl = false
                };
                try
                {
                    string textBody = " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + ">" +
                        "<tr bgcolor='#4da6ff'><td><b>Board</b></td> <td> <b> Number1</b> </td> <td> <b> Number2</b> </td><td> <b> Number3</b> </td>" +
                        "<td> <b> Number4</b> </td><td> <b> Number5</b> </td><td> <b> Number6</b> </td></tr>";
                    MailMessage mail = new MailMessage(emailUserName, "ndavhe@tussotechnologies.co.za");
                    mail.Subject = "Random numbers.";

                    foreach (var board in boards)
                    {
                        textBody += "<tr><td>" + board.Key + "</td><td>" + board.Value[0] + "</td><td> " + board.Value[1] + "</td>" +
                            "<td>" + board.Value[2] + "</td><td>" + board.Value[3] + "</td><td>" + board.Value[4] + "</td><td>" + board.Value[5] + "</td> </tr>";
                    }

                    textBody += "</table>";
                    mail.Body = textBody;
                    mail.IsBodyHtml = true;
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static void GenerateProfoma()
        {
            ArsloInvoiceGenerator invoiceGenerator = new ArsloInvoiceGenerator();
            ArsloProfoma profoma = new ArsloProfoma
            {
                Amount = 300,
                Date = DateTime.Now,
                ProfomaNumber = "PI-2018-01",
                UCRNumber = "UHADAA",
                ProfomaItems = new List<ArsloProfomaItem> {
                        new ArsloProfomaItem {
                        Description="HKJASD",
                        Price=30,
                        Quantity=10,
                        TotalPrice=300
                    } }
            };
            ArsloCustomer customer = new ArsloCustomer
            {
                CustomerName = "James Smith",
                TellNumber = "012 365 2514",
                Address = "AL SHAMSI BUILDING , FLAT #110	AL KUWAIT STREET , MYSALOON, SHARJAH 25714 UAE"
            };
            string invoicePath = invoiceGenerator.GenerateProfoma(profoma, customer);
        }

        private static void GenerateInvoice()
        {
            ArsloInvoiceGenerator invoiceGenerator = new ArsloInvoiceGenerator();
            ArsloInvoice invoice = new ArsloInvoice
            {
                Profoma = GetProfoma(),
                TotalPrice = 300,
                Date = DateTime.Now,
                Reference = "INV-ARSLO - 20350",
                PointOfLoading = "Durban",
                PointOfDelivery = "Mumbai",
                VesselNumber = "IRENES RHYTHM 006E",
                Customer =
                new ArsloCustomer
                {
                    CustomerName = "James Smith",
                    TellNumber = "012 365 2514",
                    Address = "AL SHAMSI BUILDING , FLAT #110	AL KUWAIT STREET , MYSALOON, SHARJAH 25714 UAE"
                },
                InvoiceItems = GetInvoiceItems()
            };
            var location = invoiceGenerator.GenerateInvoice(invoice);
        }

        private static List<ArsloInvoiceItem> GetInvoiceItems()
        {
            return new List<ArsloInvoiceItem> {
                     new ArsloInvoiceItem {
                         Description="PONU 0490475 SEAL 4866625",
                         Price=3.6M,
                         Quantity=32620,
                         TotalPrice= 117432.00M
                      },
                      new ArsloInvoiceItem {
                         Description="TEMU 5466173 S 4866623",
                         Price=3.6M,
                         Quantity=26940.00M,
                         TotalPrice=  96984.00M
                      },
                       new ArsloInvoiceItem {
                         Description="MSKU 2250447 SEAL 4866624",
                         Price=3.6M,
                         Quantity=26830,
                         TotalPrice=  96588.00M
                      },
                       new ArsloInvoiceItem {
                         Description="MSKU 2250447 SEAL 4866624",
                         Price=3.6M,
                         Quantity=26830,
                         TotalPrice=  96588.00M
                      },
                       new ArsloInvoiceItem {
                         Description="MSKU 2250447 SEAL 4866624",
                         Price=3.6M,
                         Quantity=26830,
                         TotalPrice=  96588.00M
                      },
                       new ArsloInvoiceItem {
                         Description="MSKU 2250447 SEAL 4866624",
                         Price=3.6M,
                         Quantity=26830,
                         TotalPrice=  96588.00M
                      },
                       new ArsloInvoiceItem {
                         Description="MSKU 2250447 SEAL 4866624",
                         Price=3.6M,
                         Quantity=26830,
                         TotalPrice=  96588.00M
                      },
                       new ArsloInvoiceItem {
                         Description="MSKU 2250447 SEAL 4866624",
                         Price=3.6M,
                         Quantity=26830,
                         TotalPrice=  96588.00M
                      },
                       new ArsloInvoiceItem {
                         Description="MSKU 2250447 SEAL 4866624",
                         Price=3.6M,
                         Quantity=26830,
                         TotalPrice=  96588.00M
                      }
            };
        }

        private static ArsloProfoma GetProfoma()
        {
            return new ArsloProfoma
            {
                Amount = 300,
                Date = DateTime.Now,
                ProfomaNumber = "PI-23432-3232",
                UCRNumber = "UHADAA",
                ProfomaItems = new List<ArsloProfomaItem> {
                        new ArsloProfomaItem {
                            Description="HKJASD",
                            Price=30,
                            Quantity=10,
                            TotalPrice=300,
                        }
                 }
            };
        }

        private static void AddSupplier(DataContext context)
        {
            List<Driver> drivers = context.Drivers.Where(x => x.Id >= 1 && x.Id <= 5).ToList();
            List<Driver> drivers2 = context.Drivers.Where(x => x.Id > 5 && x.Id <= 10).ToList();
            List<Driver> drivers3 = context.Drivers.Where(x => x.Id > 10 && x.Id <= 15).ToList();
            List<Driver> drivers4 = context.Drivers.Where(x => x.Id > 15 && x.Id <= 20).ToList();
            List<Driver> drivers5 = context.Drivers.Where(x => x.Id > 20).ToList();

            var client = new SupplierInfo
            {
                Address = " 120 Main Street",
                SupplierName = "Kempton park Metals",
                Drivers = drivers,
                Logo = "suplier.png",
                TellNumber = "0115083406",
                Suppliercode = "KEMP105",
                CompanyRegNumber = "823475"
            };
            var client2 = new SupplierInfo
            {
                Address = "215 Malibongwe Avenue",
                SupplierName = "Randburg Suppliers",
                Drivers = drivers2,
                Logo = "suplier.png",
                TellNumber = "0117983056",
                Suppliercode = "RAND175",
                CompanyRegNumber = "8533475"
            };
            var client3 = new SupplierInfo
            {
                Address = "101 Jean Avenue",
                SupplierName = "Centurion Suppliers",
                Drivers = drivers3,
                Logo = "suplier.png",
                TellNumber = "0126783456",
                Suppliercode = "CENT926",
                CompanyRegNumber = "675941"
            };
            var client4 = new SupplierInfo
            {
                Address = "132 Barry hetsoff",
                SupplierName = "Braamfontein steel",
                Drivers = drivers4,
                Logo = "suplier.png",
                TellNumber = "0116783446",
                Suppliercode = "BRAAM372",
                CompanyRegNumber = "745133"
            };
            var client5 = new SupplierInfo
            {
                Address = "7 Vaal Avenue",
                SupplierName = "Vaal Suppliers",
                Drivers = drivers5,
                Logo = "suplier.png",
                TellNumber = "0616783436",
                Suppliercode = "VAAL782",
                CompanyRegNumber = "384597"
            };
            context.SupplierInfos.Add(client);
            context.SupplierInfos.Add(client2);
            context.SupplierInfos.Add(client3);
            context.SupplierInfos.Add(client4);
            context.SupplierInfos.Add(client5);

            context.SaveChanges();
        }

        private static void AddWeighBridgeInfos(DataContext context)
        {
            List<Driver> drivers = context.Drivers.Where(x => x.Id != 0).ToList();
            int a = 1;
            foreach (Driver driver in drivers)
            {
                var weighBridgeInfo = new WeighBridgeInfo
                {
                    DateIn = DateTime.Now.Date,
                    FirstMass = 10000,
                    // Supplier = driver,
                    NettMass = 4000,
                    Product = "Scrap metal steel",
                    SecondMass = 6000,
                };
                context.WeighBridgeInfos.Add(weighBridgeInfo);
                a++;
            }
            context.SaveChanges();
            //            AddPastel(context);
        }

        private static void AddPastel(DataContext context)
        {
            var pastelInfo = new PastelInfo
            {
                Date = DateTime.Now,
                FileLocation = @"C:\Rustivia\PastelFiles\PN12083.pdf",
                PastelNumber = "PN12083"
            };
            context.PastelInfos.Add(pastelInfo);
            context.SaveChanges();
        }
    }
}