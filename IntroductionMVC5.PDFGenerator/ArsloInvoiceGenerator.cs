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
            //_invoicepath = HttpContext.Current.Server.MapPath("~/ArsloInvoice");
            _invoicepath = @"C:\Projects\Rustivia\IntroductionMVC5\IntroductionMVC5\ArsloInvoice";
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
            PdfWriter writer = PdfWriter.GetInstance(doc, output);

            doc.AddTitle("Invoice");

            doc.Open();

            //Header Table
            var logoTable = new PdfPTable(1)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                WidthPercentage = 90
            };

            // var path = Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogoPath"]));
            var path = @"C:\Projects\Rustivia\IntroductionMVC5\IntroductionMVC5\Content\img\ArsoloLogo2.png";
            Image image = Image.GetInstance(path);
            PdfPCell imghead = new PdfPCell(image);
            imghead.PaddingRight = 100f;
            logoTable.AddCell(image);

            //Header cell
            List<CellValue> registration = new List<CellValue> {
                new CellValue{ Value     ="CK NO:2012/218554",FontSize=Font.BOLD ,BorderColor=BaseColor.WHITE},
                new CellValue{ Value     ="Vat No : 4950267544",FontSize=Font.BOLD,BorderColor=BaseColor.WHITE},
                new CellValue{ Value     ="IE Code : 21366338",FontSize=Font.BOLD,BorderColor=BaseColor.WHITE}
            };
            PdfPTable registrationTable = GetTable(registration);
            registrationTable.SpacingBefore = 5f;

            List<CellValue> address = new List<CellValue> {
                new CellValue{ Value="54, NORTH REEF ROAD PARK GERMISTON 1420",
                    FontSize =Font.NORMAL,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding =100f,
                    BorderColor=BaseColor.WHITE
                }
            };
            PdfPTable addressTable = GetTable(address);
            addressTable.SpacingBefore = 2f;

            List<CellValue> contactDetails = new List<CellValue> {
                new CellValue{ Value       ="Tel: +27 11 828 9961",
                    FontSize               =Font.NORMAL,
                    BorderColor            =BaseColor.WHITE},
                new CellValue{ Value       ="Fax: +27 11 828 5134",
                    FontSize               =Font.NORMAL,
                    HorizontalAlignment    = Element.ALIGN_RIGHT,
                    Padding                =100f,
                    BorderColor            =BaseColor.WHITE
                }
            };
            PdfPTable contactDetail = GetTable(contactDetails);
            contactDetail.SpacingBefore = 2f;

            //------------------------------------------------------------


            List<CellValue> profomaNumber = new List<CellValue> {
                new CellValue{ Value=string.Format("INVOICE ARSLO {0}",invoice.Reference),
                    FontSize =Font.BOLD,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding =120f,
                    BorderColor =BaseColor.GRAY,
                    BackGroundColor=BaseColor.GRAY
                }
            };
            PdfPTable profomaNumberTable = GetTable(profomaNumber);
            profomaNumberTable.SpacingBefore = 2f;


            List<CellValue> dateValues = new List<CellValue> {
                new CellValue{ Value=string.Format("UCR NO - {0}",invoice.Profoma.UCRNumber),
                    FontSize =Font.BOLD,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderColor =BaseColor.WHITE,
                    BackGroundColor=BaseColor.WHITE
                },
                 new CellValue{ Value=string.Format("DATE - {0}",invoice.Date.ToString("yyyy/mm/dd")),
                    FontSize =Font.BOLD,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding =100f,
                    BorderColor =BaseColor.WHITE,
                    BackGroundColor=BaseColor.WHITE
                }
            };
            PdfPTable dateTable = GetTable(dateValues);
            dateTable.SpacingBefore = 2f;

            string[] custAddress = invoice.Customer.Address.Split(',');
            Phrase custAddressPhrase = new Phrase();

            for (int i = 0; i < custAddress.Length; i++)
            {
                custAddressPhrase.Add(new Phrase { new Chunk(string.Format("{0} \n", custAddress[i]), FontFactory.GetFont("Microsoft Sans Serif", 8, Font.BOLD, BaseColor.BLACK)) });
            }
            var headerTable = new PdfPTable(1)
            {
                WidthPercentage = 90
            };
            headerTable.AddCell(new PdfPCell(custAddressPhrase)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            headerTable.SpacingBefore = 15f;
            //-------------------------//--------------------------------------------------//
            //-------------------------//--------------------------------------------------//
            var itemsTable = new PdfPTable(5)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                WidthPercentage = 90
            };
            //Item no
            var itenNoCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            itenNoCellLbl.AddElement(new Phrase("Item No",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            itenNoCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(itenNoCellLbl);

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
            int count = 1;
            foreach (var item in invoice.InvoiceItems)
            {
                total = total + item.Quantity * item.Price;
                //Label
                var item0 = GetCell(string.Format("{0}", count), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item0.PaddingTop = 5f;
                item0.PaddingBottom = 5f;
                item0.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item0);
                //Description Label
                var item1 = GetCell(item.Description, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item1.PaddingTop = 5f;
                item1.PaddingBottom = 5f;
                item1.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item1);
                //QTY Label
                var item2 = GetCell(string.Format("{0}", item.Quantity), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item2.PaddingTop = 5f;
                item2.PaddingBottom = 5f;
                item2.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item2);
                //Unit Price Label
                var item3 = GetCell(string.Format("R {0}", String.Format("{0:n}", item.Price)), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item3.PaddingTop = 5f;
                item3.PaddingBottom = 5f;
                item3.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item3);
                //totalunit Price Label
                var item4 = GetCell(string.Format("R {0}", String.Format("{0:n}", item.Price * item.Quantity)), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item4.PaddingTop = 5f;
                item4.PaddingBottom = 5f;
                item4.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item4);
                count++;
            }

            for (int em = 1; em < 10; em++)
            {
                var item1 = GetCell(string.Empty, BaseColor.BLUE, BaseColor.WHITE, Font.NORMAL);
                item1.PaddingTop = 5f;
                item1.PaddingBottom = 5f;
                item1.VerticalAlignment = Element.ALIGN_LEFT;
                item1.VerticalAlignment = Element.ALIGN_CENTER;
                item1.PaddingLeft = 40f;
                itemsTable.AddCell(item1);

                //Description Label
                if (em == 2)
                {
                    var item2 = GetCell("QUALITY", BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
                    item2.PaddingTop = 5f;
                    item2.PaddingBottom = 5f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else if (em == 3)
                {
                    var item2 = GetCell("STEEL SCRAP", BaseColor.BLUE, BaseColor.WHITE, Font.NORMAL);
                    item2.PaddingTop = 5f;
                    item2.PaddingBottom = 5f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.HorizontalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else if (em == 4)
                {
                    var item2 = GetCell("POL", BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
                    item2.PaddingTop = 5f;
                    item2.PaddingBottom = 5f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.HorizontalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else if (em == 5)
                {
                    var item2 = GetCell(invoice.PointOfLoading, BaseColor.BLUE, BaseColor.WHITE, Font.NORMAL);
                    item2.PaddingTop = 5f;
                    item2.PaddingBottom = 5f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.HorizontalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else if (em == 6)
                {
                    var item2 = GetCell("POD", BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
                    item2.PaddingTop = 10f;
                    item2.PaddingBottom = 10f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.HorizontalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else if (em == 7)
                {
                    var item2 = GetCell(invoice.PointOfDelivery, BaseColor.BLUE, BaseColor.WHITE, Font.NORMAL);
                    item2.PaddingTop = 10f;
                    item2.PaddingBottom = 10f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.HorizontalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else if (em == 8)
                {
                    var item2 = GetCell("VESSEL", BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
                    item2.PaddingTop = 10f;
                    item2.PaddingBottom = 10f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.HorizontalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else if (em == 9)
                {
                    PdfPCell item2 = GetCell(invoice.VesselNumber, BaseColor.BLUE, BaseColor.WHITE, Font.NORMAL);
                    item2.PaddingTop = 10f;
                    item2.PaddingBottom = 10f;
                    item2.VerticalAlignment = Element.ALIGN_CENTER;
                    item2.HorizontalAlignment = Element.ALIGN_CENTER;
                    item2.PaddingLeft = 40f;
                    itemsTable.AddCell(item2);
                }
                else
                {
                    //QTY Label
                    var item2 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                    item2.PaddingTop = 10f;
                    item2.PaddingBottom = 10f;
                    item2.VerticalAlignment = Element.ALIGN_LEFT;
                    itemsTable.AddCell(item2);
                }

                //Unit Price Label
                var item3 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item3.PaddingTop = 10f;
                item3.PaddingBottom = 10f;
                item3.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item3);
                //totalunit Price Label
                var item4 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item4.PaddingTop = 10f;
                item4.PaddingBottom = 10f;
                item4.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item4);

                //totalunit Price Label
                var item5 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item5.PaddingTop = 10f;
                item5.PaddingBottom = 10f;
                item5.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item5);
            }

            float[] widths = new float[] { 8f, 45f, 18f, 18f, 20f };
            itemsTable.SetWidths(widths);
            var emptyCell = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
            emptyCell.Colspan = 1;
            emptyCell.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(emptyCell);

            var TotalLabel = GetCell("Total", BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            TotalLabel.BorderColor = BaseColor.GRAY;
            TotalLabel.BackgroundColor = BaseColor.LIGHT_GRAY;
            TotalLabel.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(TotalLabel);

            var TotalQty = GetCell(string.Format("{0} kg", invoice.InvoiceItems.Sum(iv => iv.Quantity)), BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            TotalQty.BorderColor = BaseColor.GRAY;
            TotalQty.BackgroundColor = BaseColor.LIGHT_GRAY;
            TotalQty.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(TotalQty);

            var anotherEmptyCell = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            anotherEmptyCell.BorderColor = BaseColor.GRAY;
            anotherEmptyCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            anotherEmptyCell.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(anotherEmptyCell);

            //total Price Label
            var totalPrice = GetCell(string.Format("R {0}", String.Format("{0:n}", invoice.InvoiceItems.Sum(iv => iv.TotalPrice))), BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            totalPrice.BorderColor = BaseColor.GRAY;
            totalPrice.BackgroundColor = BaseColor.LIGHT_GRAY;
            totalPrice.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(totalPrice);
            itemsTable.SpacingBefore = 30f;

            
            var authorisedSpacing = new Paragraph
            {
                SpacingBefore = 30f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var authorised = new Phrase("AUTHORISED SIGNATORY:\n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            var forArsloSpacing = new Paragraph
            {
                SpacingBefore = 20f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var forArslo = new Phrase("For Arslo Trading (Pty) Ltd\n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            authorisedSpacing.Add(authorised);
            forArsloSpacing.Add(forArslo);


            doc.Add(logoTable);
            doc.Add(registrationTable);
            doc.Add(addressTable);
            doc.Add(contactDetail);
            doc.Add(profomaNumberTable);
            doc.Add(dateTable);
            doc.Add(headerTable);
            doc.Add(itemsTable);
            doc.Add(authorisedSpacing);
            doc.Add(forArsloSpacing);

            doc.Close();
            output.Close();

            return _invoicepath;
        }

        public string GenerateProfoma(ArsloProfoma profoma, ArsloCustomer arsloCustomer)
        {
            var doc = new Document(PageSize.A4);
            _invoicepath = string.Format("{0}\\{1}.pdf", _invoicepath, profoma.ProfomaNumber);
            var output = new FileStream(_invoicepath, FileMode.Create);

            PdfWriter writer = PdfWriter.GetInstance(doc, output);

            doc.AddTitle("Profoma Invoice");

            doc.Open();

            //Header Table
            var logoTable = new PdfPTable(1)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                WidthPercentage = 90
            };

            //Add Logo
            //var path = Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogoPath"]));
            var path = @"C:\Projects\Rustivia\IntroductionMVC5\IntroductionMVC5\Content\img\ArsoloLogo2.png";
            Image image = Image.GetInstance(path);
            PdfPCell imghead = new PdfPCell(image);
            imghead.PaddingRight = 100f;
            logoTable.AddCell(image);

            //Header cell
            List<CellValue> registration = new List<CellValue> {
                new CellValue{ Value="CK NO:2012/218554",FontSize=Font.BOLD ,BorderColor=BaseColor.WHITE},
                new CellValue{ Value="Vat No : 4950267544",FontSize=Font.BOLD,BorderColor=BaseColor.WHITE},
                new CellValue{ Value="IE Code : 21366338",FontSize=Font.BOLD,BorderColor=BaseColor.WHITE}
            };
            PdfPTable registrationTable = GetTable(registration);
            registrationTable.SpacingBefore = 5f;

            List<CellValue> address     = new List<CellValue> {
                new CellValue{ Value    ="54, NORTH REEF ROAD PARK GERMISTON 1420",
                    FontSize            =Font.NORMAL,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding             =100f,
                    BorderColor         =BaseColor.WHITE
                }
            };
            PdfPTable addressTable = GetTable(address);
            addressTable.SpacingBefore = 2f;

            List<CellValue> contactDetails = new List<CellValue> {
                new CellValue{ Value="Tel: +27 11 828 9961",
                    FontSize =Font.NORMAL,
                    BorderColor =BaseColor.WHITE},
                new CellValue{ Value="Fax: +27 11 828 5134",
                    FontSize =Font.NORMAL,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding =100f,
                    BorderColor =BaseColor.WHITE
                }
            };
            PdfPTable contactDetail = GetTable(contactDetails);
            contactDetail.SpacingBefore = 2f;
            //------------------------------------------------------------

            List<CellValue> profomaNumber = new List<CellValue> {
                new CellValue{ Value=string.Format("PRO FORMA INVOICE {0}",profoma.ProfomaNumber),
                    FontSize =Font.BOLD,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding =100f,
                    BorderColor =BaseColor.GRAY,
                    BackGroundColor=BaseColor.GRAY
                }
            };
            PdfPTable profomaNumberTable = GetTable(profomaNumber);
            profomaNumberTable.SpacingBefore = 2f;


            List<CellValue> dateValues = new List<CellValue> {
                new CellValue{ Value=string.Format("UCR NO - {0}",profoma.UCRNumber),
                    FontSize =Font.BOLD,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    BorderColor =BaseColor.WHITE,
                    BackGroundColor=BaseColor.WHITE
                },
                 new CellValue{ Value=string.Format("DATE - {0}",profoma.Date.ToString("yyyy/mm/dd")),
                    FontSize =Font.BOLD,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding =100f,
                    BorderColor =BaseColor.WHITE,
                    BackGroundColor=BaseColor.WHITE
                }
            };
            PdfPTable dateTable = GetTable(dateValues);
            dateTable.SpacingBefore = 2f;

            string[] custAddress = arsloCustomer.Address.Split(',');
            Phrase custAddressPhrase = new Phrase();

            for (int i = 0; i < custAddress.Length; i++)
            {
                custAddressPhrase.Add(new Phrase { new Chunk(string.Format("{0} \n", custAddress[i]), FontFactory.GetFont("Microsoft Sans Serif", 8, Font.BOLD, BaseColor.BLACK)) });
            }
            var headerTable = new PdfPTable(1)
            {
                WidthPercentage = 90
            };
            headerTable.AddCell(new PdfPCell(custAddressPhrase)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            headerTable.SpacingBefore = 15f;
            //-------------------------//--------------------------------------------------//

            //-------------------------//--------------------------------------------------//
            var itemsTable = new PdfPTable(5)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                WidthPercentage = 90
            };
            //Item no
            var itenNoCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            itenNoCellLbl.AddElement(new Phrase("Item No",
                FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)));
            itenNoCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(itenNoCellLbl);

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
            int count = 1;
            foreach (var item in profoma.ProfomaItems)
            {
                total = total + item.Quantity * item.Price;
                //Label
                var item0 = GetCell(string.Format("{0}", count), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item0.PaddingTop = 10f;
                item0.PaddingBottom = 10f;
                item0.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item0);
                //Description Label
                var item1 = GetCell(item.Description, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item1.PaddingTop = 10f;
                item1.PaddingBottom = 10f;
                item1.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item1);
                //QTY Label
                var item2 = GetCell(string.Format("{0}", item.Quantity), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item2.PaddingTop = 10f;
                item2.PaddingBottom = 10f;
                item2.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item2);
                //Unit Price Label
                var item3 = GetCell(string.Format("R {0}", String.Format("{0:n}", item.Price)), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item3.PaddingTop = 10f;
                item3.PaddingBottom = 10f;
                item3.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item3);
                //totalunit Price Label
                var item4 = GetCell(string.Format("R {0}", String.Format("{0:n}", item.Price * item.Quantity)), BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item4.PaddingTop = 10f;
                item4.PaddingBottom = 10f;
                item4.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item4);
            }

            for (int em = 1; em < 10; em++)
            {
                var item1 = GetCell(string.Empty, BaseColor.BLUE, BaseColor.WHITE, Font.NORMAL);
                item1.PaddingTop = 10f;
                item1.PaddingBottom = 10f;
                item1.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item1);

                //QTY Label
                var item2 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item2.PaddingTop = 10f;
                item2.PaddingBottom = 10f;
                item2.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item2);
                //Unit Price Label
                var item3 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item3.PaddingTop = 10f;
                item3.PaddingBottom = 10f;
                item3.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item3);
                //totalunit Price Label
                var item4 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item4.PaddingTop = 10f;
                item4.PaddingBottom = 10f;
                item4.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item4);

                //totalunit Price Label
                var item5 = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
                item5.PaddingTop = 10f;
                item5.PaddingBottom = 10f;
                item5.VerticalAlignment = Element.ALIGN_LEFT;
                itemsTable.AddCell(item5);
            }

            float[] widths = new float[] { 8f, 45f, 18f, 18f, 20f };
            itemsTable.SetWidths(widths);
            var emptyCell = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.NORMAL);
            emptyCell.Colspan = 1;
            emptyCell.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(emptyCell);

            var TotalLabel = GetCell("Total", BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            TotalLabel.BorderColor = BaseColor.GRAY;
            TotalLabel.BackgroundColor = BaseColor.LIGHT_GRAY;
            TotalLabel.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(TotalLabel);

            var TotalQty = GetCell(string.Format("{0} kg", profoma.ProfomaItems.Sum(iv => iv.Quantity)), BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            TotalQty.BorderColor = BaseColor.GRAY;
            TotalQty.BackgroundColor = BaseColor.LIGHT_GRAY;
            TotalQty.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(TotalQty);

            var anotherEmptyCell = GetCell(string.Empty, BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            anotherEmptyCell.BorderColor = BaseColor.GRAY;
            anotherEmptyCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            anotherEmptyCell.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(anotherEmptyCell);

            //total Price Label
            var totalPrice = GetCell(string.Format("R {0}", String.Format("{0:n}", total)), BaseColor.BLACK, BaseColor.WHITE, Font.BOLD);
            totalPrice.BorderColor = BaseColor.GRAY;
            totalPrice.BackgroundColor = BaseColor.LIGHT_GRAY;
            totalPrice.VerticalAlignment = Element.ALIGN_LEFT;
            itemsTable.AddCell(totalPrice);
            itemsTable.SpacingBefore = 30f;

            //Banking Details
            var bankingHeaderPr = new Paragraph
            {
                SpacingBefore = 20f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            Phrase p2 = new Phrase
            {
                new Phrase("Beneficiary Details",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK))
            };

            var bankingHeaderSpacing = new Paragraph
            {
                SpacingBefore = 4f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };

            var beneficiaryName = new Phrase
            {
                   new Chunk("Beneficiary Name: Arslo Trading (PTY)LTD \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK))
            };

            var bankNameSpacing = new Paragraph
            {
                SpacingBefore = 1f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var bankName = new Phrase("Beneficiary Bank Name: ABSA Bank Limited \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var accountNumberSpacing = new Paragraph
            {
                SpacingBefore = 1f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var accountNumber = new Phrase("Beneficiary Account Number: 4085043679 \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var bankSwiftSpacing = new Paragraph
            {
                SpacingBefore = 1f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var bankSwift = new Phrase("Beneficiary Bank SWITFBIC: ABSAZAJJ \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var branchNameSpacing = new Paragraph
            {
                SpacingBefore = 1f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var branchName = new Phrase("Branch Name: ABSA East Rand \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var branchCodeSpacing = new Paragraph
            {
                SpacingBefore = 1f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var branchCode = new Phrase("Branch code: 632005 \n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            var authorisedSpacing = new Paragraph
            {
                SpacingBefore = 30f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
            var authorised = new Phrase("AUTHORISED SIGNATORY:\n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK));

            var forArsloSpacing = new Paragraph
            {
                SpacingBefore = 20f,
                Alignment = Element.ALIGN_LEFT,
                IndentationLeft = 25f
            };
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

            doc.Add(logoTable);
            doc.Add(registrationTable);
            doc.Add(addressTable);
            doc.Add(contactDetail);
            doc.Add(profomaNumberTable);
            doc.Add(dateTable);
            doc.Add(headerTable);
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

        private Image GetImage(string imagePath)
        {
            Image image = null;
            if (File.Exists(imagePath))
            {
                image = Image.GetInstance(imagePath);
            }
            return image;
        }

        private PdfPCell GetCell(string data, BaseColor baseColor, BaseColor backgroundColor, int style = Font.NORMAL)
        {
            PdfPCell cell = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderColor = BaseColor.GRAY,
                BackgroundColor = backgroundColor,
            };
            cell.AddElement(new Phrase(data, FontFactory.GetFont(FontFactory.HELVETICA, 8, style, baseColor)));

            return cell;
        }

        private PdfPTable GetTable(List<CellValue> values)
        {
            PdfPTable table = new PdfPTable(values.Count)
            {
                WidthPercentage = 90
            };

            foreach (CellValue value in values)
            {
                PdfPCell cell = GetCell(value.Value, BaseColor.BLACK, value.BackGroundColor, value.FontSize);
                cell.BorderColor = value.BorderColor;
                cell.HorizontalAlignment = value.HorizontalAlignment;
                cell.PaddingLeft = value.Padding;
                table.AddCell(cell);
            }
            return table;
        }
    }
}
