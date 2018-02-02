using System;
using System.Configuration;
using System.IO;
using IntroductionMVC5.Models.Integrator;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace RustiviaSolutions.PDFGenerator
{
    public class ContainerInfomationGenerator
    {
        private string _currpath =
                    @"C:\inetpub\wwwroot\Rustivia2.0\Rustivia Container Instruction\";

        //private string _currpath =
        //    @"C:\Dev\IntroductionMVC5\IntroductionMVC5\Rustivia Container Instruction\";

        //http://simpledotnetsolutions.wordpress.com/2012/11/01/itextsharp-creating-form-fields/
        /// <summary>
        ///     http://www.codeproject.com/Tips/679606/Filling-PDF-Form-using-iText-PDF-Library
        /// </summary>
        /// <param name="container"></param>
        public void PrintContainerInfomation(Container container)
        {
            var doc = new Document(PageSize.A4);
            _currpath = string.Format("{0}{1}.pdf", _currpath, container.ContainerNumber);
            var output = new FileStream(_currpath, FileMode.Create);
            //var filecontent = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, output);

            doc.AddTitle("Container Instruction");

            doc.Open();

            var table1 = new PdfPTable(2);
            table1.DefaultCell.Border = 2;
            table1.WidthPercentage = 80;
            table1.HorizontalAlignment = Element.ALIGN_MIDDLE;

            //Header Table
            var table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_MIDDLE, WidthPercentage = 80 };

            var phrase = new Phrase
            {
                new Chunk("Rustivia Metals CC\n\n",
                    FontFactory.GetFont("Microsoft Sans Serif", 16, Font.BOLD, BaseColor.BLACK)),
                new Chunk("54, Northreef,\n",
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk("Activia park,\n",
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk("Germiston, South Africa \n",
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("{0} {1}", "(T)", "011 828 9961 \n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("{0} {1}", "Reg no.", "1997/0025504/23 \n\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 8, Font.NORMAL, BaseColor.BLACK))
            };
            table.AddCell(new PdfPCell(phrase)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_TOP,
                PaddingBottom = 1f,
                PaddingTop = 0f
            });
            //Add Logo
            Image image =
                Image.GetInstance(ConfigurationManager.AppSettings["LogoPath"]);

            image.ScaleToFit(140f, 120f);
            image.Alignment = Element.ALIGN_RIGHT;
            table.AddCell(new PdfPCell(image)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                PaddingBottom = 1f,
                PaddingTop = 0f
            });
            //Header cell
            var hdrCell =
                new PdfPCell(new Phrase("Container Instruction",
                    FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)))
                {
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    PaddingBottom = 22f
                };

            table1.AddCell(hdrCell);
            //Wb Number
            var cell161 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell161.AddElement(new Paragraph("WB NUMBER", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));
            var cell162 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            string wbNumber = container.WeighBridgeInfo.Id.ToString();
            cell162.AddElement(new Paragraph(wbNumber));
            table1.AddCell(cell161);
            table1.AddCell(cell162);

            //Client
            var cell11 = new PdfPCell { Colspan = 1 };
            cell11.AddElement(new Phrase("CUSTOMER", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));
            cell11.VerticalAlignment = Element.ALIGN_LEFT;

            var cell12 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell12.AddElement(new Paragraph(container.Booking.Customer.CustomerName));
            table1.AddCell(cell11);
            table1.AddCell(cell12);

            //Booking Reference
            var cell21 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell21.AddElement(new Paragraph("BOOKING REFERENCE",
                FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell22 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell22.AddElement(new Paragraph(container.Booking.Reference));
            table1.AddCell(cell21);
            table1.AddCell(cell22);

            //Container number
            var cell31 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell31.AddElement(new Paragraph("CONTAINER NUMBER",
                FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell32 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell32.AddElement(new Paragraph(container.ContainerNumber));
            table1.AddCell(cell31);
            table1.AddCell(cell32);

            //Seal number
            var cell41 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell41.AddElement(new Paragraph("SEAL NUMBER", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell42 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell42.AddElement(new Paragraph(container.Sealnumber));
            table1.AddCell(cell41);
            table1.AddCell(cell42);

            //Gross Weight
            var cell51 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell51.AddElement(new Paragraph("GROSS WEIGHT", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell52 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell52.AddElement(new Paragraph(string.Format("{0} KG", container.GrossWeight)));
            table1.AddCell(cell51);
            table1.AddCell(cell52);

            //Tare Weight
            var cell61 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell61.AddElement(new Paragraph("TARE WEIGHT", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell62 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell62.AddElement(new Paragraph(string.Format("{0} KG", container.TareWeight)));
            table1.AddCell(cell61);
            table1.AddCell(cell62);

            //Nett Weight
            var cell71 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell71.AddElement(new Paragraph("NETT WEIGHT", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell72 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell72.AddElement(new Paragraph(string.Format("{0} KG", container.NettWeight)));
            table1.AddCell(cell71);
            table1.AddCell(cell72);

            //Product
            var cell81 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell81.AddElement(new Paragraph("PRODUCT", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell82 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell82.AddElement(new Paragraph(container.Product));
            table1.AddCell(cell81);
            table1.AddCell(cell82);

            //Depot name
            var cell91 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell91.AddElement(new Paragraph("DEPOT NAME", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell92 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell92.AddElement(new Paragraph(container.DepotName));
            table1.AddCell(cell91);
            table1.AddCell(cell92);

            //Delivery note
            var cell101 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell101.AddElement(new Paragraph("DELIVERY NOTE", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell102 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell102.AddElement(new Paragraph(container.DeliveryNote));
            table1.AddCell(cell101);
            table1.AddCell(cell102);

            //Transport 1
            var cell111 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell111.AddElement(new Paragraph("TRANSPORT 1", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell112 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell112.AddElement(new Paragraph(container.Booking.Transporter.Name));
            table1.AddCell(cell111);
            table1.AddCell(cell112);

            //Transport 2
            var cell151 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell151.AddElement(new Paragraph("TRANSPORT 2", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell152 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell152.AddElement(new Paragraph(container.TruckRegNumber));
            table1.AddCell(cell151);
            table1.AddCell(cell152);

            //Date In
            var cell121 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell121.AddElement(new Paragraph("DATE IN", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell122 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell122.AddElement(new Paragraph(container.DateIn.Date.ToString("d")));
            table1.AddCell(cell121);
            table1.AddCell(cell122);

            //Date Out
            var cell131 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell131.AddElement(new Paragraph("DATE OUT", FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));

            var cell132 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            string dateout = container.WeighBridgeInfo.DateOut.HasValue
                ? container.WeighBridgeInfo.DateOut.Value.Date.ToString("d")
                : DateTime.Now.Date.ToString("d");
            cell132.AddElement(new Paragraph(dateout));
            table1.AddCell(cell131);
            table1.AddCell(cell132);

            //Driver signature
            var cell141 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            cell141.AddElement(new Paragraph("DRIVER SIGNATURE",
                FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD)));
            var cell142 = new PdfPCell { VerticalAlignment = Element.ALIGN_MIDDLE };
            table1.AddCell(cell141);
            table1.AddCell(cell142);

            PdfContentByte content = writer.DirectContent;
            var bar128 = new Barcode128 { Code = DateTime.Now.ToString() };

            Image imgs = bar128.CreateImageWithBarcode(content, null, null);

            imgs.Alignment = Element.ALIGN_MIDDLE;

            var par = new Paragraph();
            doc.Add(table);
            doc.Add(table1);
            doc.Add(par);
            doc.Add(imgs);

            doc.Close();
        }
    }
}