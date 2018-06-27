using IntroductionMVC5.Models.ArsloTrading;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RustiviaSolutions.PDFGenerator
{
    public class ArsloInvoiceGenerator
    {
        private string _invoicepath;
        public ArsloInvoiceGenerator()
        {
            _invoicepath = HttpContext.Current.Server.MapPath("~/ArsloInvoice");
            //_invoicepath = @"C:\Projects\Rustivia\IntroductionMVC5\IntroductionMVC5\ArsloInvoice";
            if (!Directory.Exists(_invoicepath))
            {
                Directory.CreateDirectory(_invoicepath);
            }
        }

        public string GenerateInvoice(ArsloInvoice invoice)
        {
            var doc = new Document(PageSize.A4);
            _invoicepath = string.Format("{0}\\{1}.pdf", _invoicepath, invoice.Reference);
            var output = new FileStream(_invoicepath, FileMode.Create);
            //var filecontent = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, output);

            doc.AddTitle("Invoice");

            doc.Open();

            var mainTable = new PdfPTable(2);
            mainTable.DefaultCell.Border = 2;
            mainTable.WidthPercentage = 80;
            mainTable.HorizontalAlignment = Element.ALIGN_LEFT;

            //Header Table
            var headerTable = new PdfPTable(2)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                WidthPercentage = 80
            };

            var infoPhrase = new Phrase
            {
                new Chunk("Arslo Trading PTY LTD\n\n",
                    FontFactory.GetFont("Microsoft Sans Serif", 14, Font.BOLD, BaseColor.BLACK)),
                new Chunk(string.Format("{0} {1}", "CK NO:", "2012/218854/07 \n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                 new Chunk(string.Format("IE Code:{0}", "21366338\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Regn:{0}", "9246511183\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Vat Regn:{0}", "4950267544\n\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("{0}", "54, NORTH REEF ROAD ACTIVIA PARK GERMISTON 1420\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Tel: +{0}", "27 11 828 9961\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Fax: +{0}", "27 11 828 5134\n\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                 new Chunk(string.Format("www.Arslo.co.za\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                  new Chunk(string.Format("Email:{0}", "Nikhil@Rustiviametals.co.za\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
            };
            headerTable.AddCell(new PdfPCell(infoPhrase)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 1f,
                PaddingTop = 0f
            });

            var path = Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogoPath"]));
            //var path = $"C:\\Projects\\Rustivia\\IntroductionMVC5\\IntroductionMVC5\\Content\\img\\ArsoloLogo.png";
            Image image = Image.GetInstance(path);

            image.ScaleToFit(60f, 40f);
            image.Alignment = Element.ALIGN_RIGHT;
            headerTable.AddCell(new PdfPCell(image)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                PaddingBottom = 1f,
                PaddingTop = 0f
            });
            //Header cell
            var invoiceLabelCell =
                new PdfPCell(new Phrase("INVOICE",
                    FontFactory.GetFont(FontFactory.HELVETICA, 22, Font.BOLD, BaseColor.GRAY)))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    //PaddingTop = 2f,
                    BorderColor = BaseColor.WHITE
                };

            mainTable.AddCell(invoiceLabelCell);
            mainTable.SpacingBefore = 10f;
            //-------------------------//--------------------------------------------------//
            //-------------------------//--------------------------------------------------//
            //Invoice Number Table
            var invoiceNumberTable = new PdfPTable(2)
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                WidthPercentage = 50
            };

            //Labels For PI number and Data
            var invNumberCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY
            };
            invNumberCellLbl.AddElement(new Phrase("Invoice Number",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));
            invNumberCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            var invNumberCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY
            };
            invNumberCellData.AddElement(new Phrase(invoice.Reference,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            invNumberCellData.VerticalAlignment = Element.ALIGN_LEFT;

            invoiceNumberTable.AddCell(invNumberCellLbl);
            invoiceNumberTable.AddCell(invNumberCellData);


            //Labels For UCR number and Data
            var UCRCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY
            };
            UCRCellLbl.AddElement(new Phrase("UCR NO",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));
            UCRCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            var UCRCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY
            };
            UCRCellData.AddElement(new Phrase(invoice.Profoma.UCRNumber,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            UCRCellData.VerticalAlignment = Element.ALIGN_LEFT;

            invoiceNumberTable.AddCell(UCRCellLbl);
            invoiceNumberTable.AddCell(UCRCellData);

            //PI Data
            var pICellLbl = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY
            };
            pICellLbl.AddElement(new Phrase("PI",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));

            var pICellData = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderColor = BaseColor.GRAY
            };
            pICellData.AddElement(new Phrase(invoice.Profoma.ProfomaNumber,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));

            invoiceNumberTable.AddCell(pICellLbl);
            invoiceNumberTable.AddCell(pICellData);
            invoiceNumberTable.SpacingBefore = 10f;

            //Data For Date and Date
            var dateCellLbl = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY
            };
            dateCellLbl.AddElement(new Phrase("Date",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));

            var dateCellData = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderColor = BaseColor.GRAY
            };
            dateCellData.AddElement(new Phrase(DateTime.Now.ToString("yyyy-MM-dd"),
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            invoiceNumberTable.AddCell(dateCellLbl);
            invoiceNumberTable.AddCell(dateCellData);
            invoiceNumberTable.SpacingBefore = 10f;
            //-------------------------//--------------------------------------------------//

            invoiceNumberTable.SpacingBefore = 10f;
            invoiceNumberTable.SpacingAfter = 50f;

            //-------------------------//--------------------------------------------------//

            //-------------------------//--------------------------------------------------//

            //Labels For Invoice Billing Information
            var billignInfoTable = new PdfPTable(1)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                WidthPercentage = 30
            };
            //Billing Information Label
            var billignCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY,
                BorderColorBottom = BaseColor.BLACK,
                PaddingTop = 10f,
                PaddingLeft = 50f
            };
            billignCellLbl.AddElement(new Phrase("Customer",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));
            billignCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            billignInfoTable.AddCell(billignCellLbl);

            //Billing Information Label
            var billignCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.WHITE
            };
            billignCellData.AddElement(new Phrase(invoice.Customer.CustomerName + "\n" + invoice.Customer.Address + "\n" + invoice.Customer.TellNumber,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            billignCellData.VerticalAlignment = Element.ALIGN_LEFT;
            billignInfoTable.AddCell(billignCellData);
            billignInfoTable.SpacingAfter = 50f;
            billignInfoTable.SpacingBefore = 10f;
            //-------------------------//--------------------------------------------------//
            var itemsTable = new PdfPTable(4)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                WidthPercentage = 100
            };
            //Description Label
            var descriptionCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            descriptionCellLbl.AddElement(new Phrase("PRODUCT",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            descriptionCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(descriptionCellLbl);
            //QTY Label
            var qtyCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            qtyCellLbl.AddElement(new Phrase("QTY",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            qtyCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(qtyCellLbl);
            //Unit Price Label
            var unitPriceCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            unitPriceCellLbl.AddElement(new Phrase("Unit Price per kg",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            unitPriceCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(unitPriceCellLbl);
            //total Price Label
            var totalPriceCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            totalPriceCellLbl.AddElement(new Phrase("Total Price",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            totalPriceCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(totalPriceCellLbl);
            //-------------------------//--------------------------------------------------//
            //Calculating the total
            decimal total = 0;
            foreach (var orderItem in invoice.InvoiceItems)
            {
                total = total + orderItem.Price * orderItem.Quantity;
                //Description Label
                var item1 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item1.AddElement(new Phrase(orderItem.Description,
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item1.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item1);
                //QTY Label
                var item2 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item2.AddElement(new Phrase(string.Format("{0}", orderItem.Quantity),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item2.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item2);


                //Unit Price Label
                var item3 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item3.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", orderItem.Price)),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item3.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item3);
                //totalunit Price Label
                var item4 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item4.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", orderItem.Price * orderItem.Quantity)),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item4.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item4);
            }


            float[] widths = new float[] { 50f, 10f, 20f, 20f };
            itemsTable.SetWidths(widths);
            var useless1 = new PdfPCell
            {
                Colspan = 2,
                BorderColor = BaseColor.GRAY
            };
            useless1.AddElement(new Phrase("",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
            useless1.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(useless1);


            var TotalLabel = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            TotalLabel.AddElement(new Phrase("TOTAL",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            TotalLabel.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(TotalLabel);

            //total Price Label
            var totalPrice = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            totalPrice.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", total)),
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            totalPrice.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(totalPrice);

            var conactHeaderPr = new Paragraph { SpacingBefore = 20f };
            Phrase p2 = new Phrase
            {
                new Phrase("Shipping Details",
                    FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLDITALIC, BaseColor.BLACK))
            };
            var conact1Pr = new Paragraph { SpacingBefore = 10f };

            var pol = new Phrase("POL :" + invoice.PointOfLoading + "\n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            var pod = new Phrase("POD :" + invoice.PointOfDelivery + "\n",
             FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            var vessel = new Phrase("Vessel :" + invoice.VesselNumber + "\n",
             FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            var bookingNumber = new Phrase("Booking Number :" + invoice.BookingNumber + "\n",
             FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            conactHeaderPr.Add(p2);
            conact1Pr.Add(pol);
            conact1Pr.Add(pod);
            conact1Pr.Add(vessel);
            conact1Pr.Add(bookingNumber);

            doc.Add(headerTable);
            doc.Add(mainTable);
            doc.Add(invoiceNumberTable);
            doc.Add(billignInfoTable);
            doc.Add(itemsTable);
            doc.Add(conactHeaderPr);
            doc.Add(conact1Pr);

            doc.Close();
            output.Close();

            return _invoicepath;
        }

        public string GenerateProfoma(ArsloProfoma profoma, ArsloCustomer arsloCustomer)
        {
            var doc = new Document(PageSize.A4);
            _invoicepath = string.Format("{0}\\{1}.pdf", _invoicepath, profoma.ProfomaNumber);
            var output = new FileStream(_invoicepath, FileMode.Create);
            //var filecontent = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, output);

            doc.AddTitle("Profoma Invoice");

            doc.Open();

            var mainTable = new PdfPTable(2);
            mainTable.DefaultCell.Border = 2;
            mainTable.WidthPercentage = 90;
            mainTable.HorizontalAlignment = Element.ALIGN_MIDDLE;
            //Header Table
            var headerTable = new PdfPTable(2)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                WidthPercentage = 90
            };

            var infoPhrase = new Phrase
            {
                new Chunk("Arslo Trading PTY LTD\n\n",
                    FontFactory.GetFont("Microsoft Sans Serif", 16, Font.BOLD, BaseColor.BLACK)),
                new Chunk(string.Format("{0} {1}", "CK NO:", "2012/218854/07 \n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                 new Chunk(string.Format("IE Code:{0}", "21366338\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Regn:{0}", "9246511183\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Vat Regn:{0}", "4950267544\n\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("{0}", "54, NORTH REEF ROAD ACTIVIA PARK GERMISTON 1420\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Tel: +{0}", "27 11 828 9961\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("Fax: +{0}", "27 11 828 5134\n\n"),
                   FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Empty,
                   FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                 new Chunk(string.Format("www.Arslo.co.za\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                  new Chunk(string.Format("Email:{0}", "Nikhil@Rustiviametals.co.za\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
            };
            headerTable.AddCell(new PdfPCell(infoPhrase)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 1f,
                PaddingTop = 0f
            });
            //Add Logo
            var path = Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogoPath"]));
            //var path = ConfigurationManager.AppSettings["LogoPath"];
            //var path = $"C:\\Projects\\Rustivia\\IntroductionMVC5\\IntroductionMVC5\\Content\\img\\ArsoloLogo.png";
            Image image =
                Image.GetInstance(path);

            image.ScaleToFit(60f, 40f);
            image.Alignment = Element.ALIGN_RIGHT;
            headerTable.AddCell(new PdfPCell(image)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                PaddingBottom = 1f,
                PaddingTop = 0f
            });
            //Header cell
            var invoiceLabelCell =
                new PdfPCell(new Phrase("Profoma Invoice",
                    FontFactory.GetFont(FontFactory.HELVETICA, 26, Font.BOLD, BaseColor.DARK_GRAY)))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingTop = 10f,
                    BorderColor = BaseColor.WHITE
                };

            mainTable.AddCell(invoiceLabelCell);
            mainTable.SpacingBefore = 10f;
            //-------------------------//--------------------------------------------------//
            //Invoice Number Table
            var invoiceNumberTable = new PdfPTable(2)
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                WidthPercentage = 50
            };

            //Labels For PI number and Data
            var invNumberCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY
            };
            invNumberCellLbl.AddElement(new Phrase("PI",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));
            invNumberCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            var invNumberCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY
            };
            invNumberCellData.AddElement(new Phrase(profoma.ProfomaNumber,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            invNumberCellData.VerticalAlignment = Element.ALIGN_LEFT;

            invoiceNumberTable.AddCell(invNumberCellLbl);
            invoiceNumberTable.AddCell(invNumberCellData);


            //Labels For UCR number and Data
            var UCRCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY
            };
            UCRCellLbl.AddElement(new Phrase("UCR NO",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));
            UCRCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            var UCRCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY
            };
            UCRCellData.AddElement(new Phrase(profoma.UCRNumber,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            UCRCellData.VerticalAlignment = Element.ALIGN_LEFT;

            invoiceNumberTable.AddCell(UCRCellLbl);
            invoiceNumberTable.AddCell(UCRCellData);

            //Data For Date and Date
            var dateCellLbl = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY
            };
            dateCellLbl.AddElement(new Phrase("Date",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));

            var dateCellData = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderColor = BaseColor.GRAY
            };
            dateCellData.AddElement(new Phrase(DateTime.Now.ToString("yyyy-MM-dd"),
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            invoiceNumberTable.AddCell(dateCellLbl);
            invoiceNumberTable.AddCell(dateCellData);
            invoiceNumberTable.SpacingBefore = 10f;
            //-------------------------//--------------------------------------------------//

            //Labels For Invoice Billing Information
            var billignInfoTable = new PdfPTable(1)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                WidthPercentage = 30
            };
            //Billing Information Label
            var billignCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.DARK_GRAY,
                PaddingTop = 10f,
                PaddingLeft = 50f
            };
            billignCellLbl.AddElement(new Phrase("Customer",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.DARK_GRAY)));
            billignCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            billignInfoTable.AddCell(billignCellLbl);

            //Billing Information Label
            var billignCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.WHITE
            };
            billignCellData.AddElement(new Phrase(arsloCustomer.CustomerName + "\n" + arsloCustomer.Address + "\n" + arsloCustomer.TellNumber,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            billignCellData.VerticalAlignment = Element.ALIGN_LEFT;
            billignInfoTable.AddCell(billignCellData);
            billignInfoTable.SpacingAfter = 50f;
            billignInfoTable.SpacingBefore = 10f;
            //-------------------------//--------------------------------------------------//
            var itemsTable = new PdfPTable(4)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                WidthPercentage = 100
            };
            //Description Label
            var descriptionCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            descriptionCellLbl.AddElement(new Phrase("Product",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            descriptionCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(descriptionCellLbl);
            //QTY Label
            var qtyCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            qtyCellLbl.AddElement(new Phrase("Qty",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            qtyCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(qtyCellLbl);
            //Unit Price Label
            var unitPriceCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            unitPriceCellLbl.AddElement(new Phrase("Unit Price",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            unitPriceCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(unitPriceCellLbl);
            //total Price Label
            var totalPriceCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            totalPriceCellLbl.AddElement(new Phrase("Total Price",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            totalPriceCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(totalPriceCellLbl);
            //-------------------------//--------------------------------------------------//
            //Calculating the total
            decimal total = 0;
            foreach (var item in profoma.ProfomaItems)
            {
                total = total + item.Quantity * item.Price;
                //Description Label
                var item1 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item1.AddElement(new Phrase(item.Description,
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item1.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item1);
                //QTY Label
                var item2 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item2.AddElement(new Phrase(string.Format("{0}", item.Quantity),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item2.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item2);


                //Unit Price Label
                var item3 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item3.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", item.Price)),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item3.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item3);
                //totalunit Price Label
                var item4 = new PdfPCell
                {
                    Colspan = 1,
                    BorderColor = BaseColor.GRAY,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                item4.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", item.Price * item.Quantity)),
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
                item4.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item4);
            }
            float[] widths = new float[] { 50f, 10f, 20f, 20f };
            itemsTable.SetWidths(widths);
            var useless1 = new PdfPCell
            {
                Colspan = 2,
                BorderColor = BaseColor.GRAY
            };
            useless1.AddElement(new Phrase("",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)));
            useless1.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(useless1);


            var TotalLabel = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            TotalLabel.AddElement(new Phrase("Total",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            TotalLabel.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(TotalLabel);


            //total Price Label
            var totalPrice = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY,
                BackgroundColor = BaseColor.LIGHT_GRAY,
            };
            totalPrice.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", total)),
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            totalPrice.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(totalPrice);


            //Banking Details
            var bankingHeaderPr = new Paragraph { SpacingBefore = 20f };
            Phrase p2 = new Phrase
            {
                new Phrase("Beneficiary Details",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK))
            };

            var bankingHeaderSpacing = new Paragraph { SpacingBefore = 4f };

            var beneficiaryName = new Phrase
            {
                   new Chunk("Beneficiary Name: Arslo Trading (PTY)LTD \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK))
            };

            var bankNameSpacing = new Paragraph { SpacingBefore = 1f };
            var bankName = new Phrase("Beneficiary Bank Name: ABSA Bank Limited \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var accountNumberSpacing = new Paragraph { SpacingBefore = 1f };
            var accountNumber = new Phrase("Beneficiary Account Number: 4085043679 \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var bankSwiftSpacing = new Paragraph { SpacingBefore = 1f };
            var bankSwift = new Phrase("Beneficiary Bank SWITFBIC: ABSAZAJJ \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var branchNameSpacing = new Paragraph { SpacingBefore = 1f };
            var branchName = new Phrase("Branch Name: ABSA East Rand \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var branchCodeSpacing = new Paragraph { SpacingBefore = 1f };
            var branchCode = new Phrase("Branch code: 632005 \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var authorisedSpacing = new Paragraph { SpacingBefore = 30f };
            var authorised = new Phrase("AUTHORISED SIGNATORY:\n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            var forArsloSpacing = new Paragraph { SpacingBefore = 20f };
            var forArslo = new Phrase("For Arslo Trading (Pty) Ltd\n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));


            bankingHeaderPr.Add(p2);
            bankingHeaderSpacing.Add(beneficiaryName);
            bankNameSpacing.Add(bankName);
            accountNumberSpacing.Add(accountNumber);
            bankSwiftSpacing.Add(bankSwift);
            branchNameSpacing.Add(branchName);
            branchCodeSpacing.Add(branchCode);
            authorisedSpacing.Add(authorised);
            forArsloSpacing.Add(forArslo);

            doc.Add(headerTable);
            doc.Add(mainTable);
            doc.Add(invoiceNumberTable);
            doc.Add(billignInfoTable);
            doc.Add(itemsTable);

            doc.Add(bankingHeaderPr);
            doc.Add(bankingHeaderSpacing);
            doc.Add(bankNameSpacing);
            doc.Add(accountNumberSpacing);
            doc.Add(bankSwiftSpacing);
            doc.Add(branchNameSpacing);
            doc.Add(branchCodeSpacing);
            doc.Add(authorisedSpacing);
            doc.Add(forArsloSpacing);

            doc.Close();
            output.Close();

            return _invoicepath;
        }
    }
}
