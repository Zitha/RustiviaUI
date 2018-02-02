using System;
using System.Configuration;
using System.Globalization;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using IntroductionMVC5.Models.PettyCash;

namespace RustiviaSolutions.PDFGenerator
{
    public class PettyCashPrint
    {
        public void PrintPaymentReceipt(Payment payment)
        {
            var printDialog = new PrintDialog();

            var flowDoc = new FlowDocument { PageWidth = 320f };

            //Header
            var header = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 18,
                Padding = new Thickness(1, 0, 1, 0)
            };
            header.Inlines.Add("Petty Cash Receipt");
            flowDoc.Blocks.Add(header);

            var para = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                Padding = new Thickness(1, 3, 1, 0)
            };
            //Address
            //var address = new Paragraph
            //{
            //    Margin = new Thickness(0),
            //    FontFamily = new FontFamily("Arial"),
            //    FontSize = 12,
            //    Padding = new Thickness(1, 0, 1, 0)
            //};
            //address.Inlines.Add("54 Northreef");
            //address.Inlines.Add(new LineBreak());
            //address.Inlines.Add("Activia perk");
            //address.Inlines.Add(new LineBreak());
            //address.Inlines.Add("Germiston, South Africa");
            //address.Inlines.Add(new LineBreak());
            //address.Inlines.Add("(T) 011 828 9961");
            //address.Inlines.Add(new LineBreak());
            //address.Inlines.Add("Reg no. 1997/0025504/23");
            //address.Inlines.Add(new LineBreak());
            //address.Inlines.Add("Vat no. 4610216634");
            //address.Inlines.Add(new LineBreak());
            //flowDoc.Blocks.Add(address);

            var typeParagraph = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 14,
                Padding = new Thickness(1, 0, 1, 0)
            };
            typeParagraph.Inlines.Add(new LineBreak());
            typeParagraph.Inlines.Add(new LineBreak());
            typeParagraph.Inlines.Add("Payment");
            flowDoc.Blocks.Add(typeParagraph);
            //Ref no
            string refNoDescription = "Ref no:".PadRight(16);
            string refNo = payment.AutoGenNumber.ToString(CultureInfo.InvariantCulture);
            string refNoLine = refNoDescription + refNo;

            //Type
            string typeDescription = "Info: ".PadRight(15);
            string type = payment.PaymentType != null ? payment.PaymentType.Type : string.Empty;
            string typeLine = (typeDescription + type);

            //Pastel no
            string pastelNoDescription = "Pastel no:".PadRight(14);
            string pastelNo = payment.PastelNo;
            string pastelLine = pastelNoDescription + pastelNo;

            //Description
            string description = "Description:".PadRight(13);
            string dscrption = payment.Description;
            string descriptionLine = description + dscrption;

            //Date
            string dateDescription = "Date".PadRight(17);
            string date = payment.Date.ToString(CultureInfo.InvariantCulture);
            string dateLine = dateDescription + date;

            //Amount
            string amountDescription = "Amount: ".PadRight(15);
            string amount = string.Format("R {0}", payment.Amount.ToString(CultureInfo.InvariantCulture));
            string amountLine = (amountDescription + amount);

            //User
            string userDescription = "User: ".PadRight(17);
            string user = payment.User;
            string userLine = (userDescription + user);

            //PettCash
            string pettyCashDescription = "Petty Cash Balance: ".PadRight(17);
            string pettyCash = string.Format("R {0}", payment.PettyAccount);
            string pettyCashLine = (pettyCashDescription + pettyCash);

            para.Inlines.Add(new Run(typeLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(refNoLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(pastelLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(descriptionLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(dateLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(amountLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(userLine));
            var pettyCashPara = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 10,
                Padding = new Thickness(1, 0, 1, 0),
                FontWeight = FontWeights.Bold
            };
            pettyCashPara.Inlines.Add(new LineBreak());
            pettyCashPara.Inlines.Add(new LineBreak());
            pettyCashPara.Inlines.Add(new Run(pettyCashLine));

            flowDoc.Blocks.Add(para);
            flowDoc.Blocks.Add(pettyCashPara);

            var localPrintServer = new LocalPrintServer();
            var p = localPrintServer.GetPrintQueues();

            foreach (PrintQueue pq in p)
            {
                if (pq.FullName.Contains(ConfigurationManager.AppSettings["ThermalPrinter"]))
                {
                    PrintQueue defaultPrintQueue = pq;
                    printDialog.PrintQueue = defaultPrintQueue;
                }
            }

            DocumentPaginator paginator =
                (flowDoc as IDocumentPaginatorSource).DocumentPaginator;

            printDialog.PrintDocument(paginator, "Reciept");
        }

        public void PrintReceipt(Receipt receipt)
        {
            var printDialog = new PrintDialog();

            var flowDoc = new FlowDocument { PageWidth = 320f };

            //Header
            var header = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 18,
                Padding = new Thickness(1, 0, 1, 0)
            };
            header.Inlines.Add("Petty Cash Receipt");
            flowDoc.Blocks.Add(header);

            var para = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                Padding = new Thickness(1, 3, 1, 0)
            };

            var typeParagraph = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 14,
                Padding = new Thickness(1, 0, 1, 0)
            };
            typeParagraph.Inlines.Add(new LineBreak());
            typeParagraph.Inlines.Add(new LineBreak());
            typeParagraph.Inlines.Add("Receipt");
            flowDoc.Blocks.Add(typeParagraph);
            //Ref no
            string refNoDescription = "Ref no:".PadRight(16);
            string refNo = receipt.AutoGenNumber;
            string refNoLine = refNoDescription + refNo;

            //Type
            string typeDescription = "Type: ".PadRight(15);
            string type = receipt.Type;
            string typeLine = (typeDescription + type);

            //Info
            string pastelNoDescription = "Info:".PadRight(14);
            string pastelNo = receipt.Reference;
            string pastelLine = pastelNoDescription + pastelNo;

            //Date
            string dateDescription = "Date".PadRight(17);
            string date = receipt.Date.ToString(CultureInfo.InvariantCulture);
            string dateLine = dateDescription + date;

            //Amount
            string amountDescription = "Amount: ".PadRight(15);
            string amount = string.Format("R {0}", receipt.Amount.ToString(CultureInfo.InvariantCulture));
            string amountLine = (amountDescription + amount);

            //User
            string userDescription = "User: ".PadRight(17);
            string user = receipt.User;
            string userLine = (userDescription + user);

            //PettyCash
            string pettyCashDescription = "Petty Cash Balance: ".PadRight(17);
            string pettyCash = string.Format("R {0}", receipt.PettyAccount);
            string pettyCashLine = (pettyCashDescription + pettyCash);

            para.Inlines.Add(new Run(typeLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(refNoLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(pastelLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(dateLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(amountLine));
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new LineBreak());
            para.Inlines.Add(new Run(userLine));

            var pettyCashPara = new Paragraph
            {
                Margin = new Thickness(0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 10,
                Padding = new Thickness(1, 0, 1, 0),
                FontWeight = FontWeights.Bold
            };
            pettyCashPara.Inlines.Add(new LineBreak());
            pettyCashPara.Inlines.Add(new LineBreak());
            pettyCashPara.Inlines.Add(new Run(pettyCashLine));

            flowDoc.Blocks.Add(para);
            flowDoc.Blocks.Add(pettyCashPara);
            var localPrintServer = new LocalPrintServer();
            var p = localPrintServer.GetPrintQueues();

            foreach (PrintQueue pq in p)
            {
                if (pq.FullName.Contains(ConfigurationManager.AppSettings["ThermalPrinter"]))
                {
                    PrintQueue defaultPrintQueue = pq;
                    printDialog.PrintQueue = defaultPrintQueue;
                }
            }

            DocumentPaginator paginator =
                (flowDoc as IDocumentPaginatorSource).DocumentPaginator;

            printDialog.PrintDocument(paginator, "Reciept");
        }
    }
}