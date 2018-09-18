using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TussoTechWebsite.Model;
using TussoTechWebsite.PDFGenerator;

namespace TussoTechWebsite.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generate Invoice");

            InvoiceGenerator invG = new InvoiceGenerator();
            Customer cst = new Customer();

            cst.Address = "Azola Human Capitals\n" +
                "14 Corner Littler Loop and True North \n" +
                "Mulberton";

            List<Item> items = new List<Item>()
            {
                new Item
                {
                    Description="IT Support",
                    Quantity=2,
                    UnitPrice=1000
                },
                new Item
                {
                    Description="Telepohne",
                    Quantity=1,
                    UnitPrice=200
                }
            };


            Invoice invoice = new Invoice
            {
                InvoiceNumber = "1001",
                Customer = cst,
                Items = items
            };

            Expense exp = new Expense
            {
                PurchaseNumber = "EXP_100",
                DateSent = DateTime.Now,
                Type = "Air Time",
                Description = "The PageSize class contains a number of Rectangle objects representing the most common paper sizes from A0 to A10, B0 to B10, LEGAL, LEDGER, LETTER, POSTCARD, TABLOID ",
                Employee = "Ndavhe",
                Total = 60
            };

            invG.CreateExpense(exp);
            // invG.CreateInvoice(invoice);

            Console.WriteLine("Done............");
            Console.Read();

        }
    }
}
