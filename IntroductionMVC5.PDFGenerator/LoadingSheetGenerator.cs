using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroductionMVC5.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace RustiviaSolutions.PDFGenerator
{
    public class LoadingSheetGenerator
    {
        private readonly string _currpath = Path.GetTempPath() + "loadingSheet.pdf";
        public void printDocument()
        {

            var doc = new Document(PageSize.A4);
            var output = new FileStream(_currpath + "\\table.pdf", FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, output);

            doc.AddTitle("Some Name");

            doc.Open();

            PdfPTable table1 = new PdfPTable(2);
            table1.DefaultCell.Border = 2;
            table1.WidthPercentage = 80;
            table1.HorizontalAlignment = Element.ALIGN_MIDDLE;

            //Header cell
            PdfPCell hdrCell = new PdfPCell(new Phrase("Container Instruction", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));
            hdrCell.Colspan = 2;
            hdrCell.HorizontalAlignment = 1;
            hdrCell.PaddingBottom = 22f;

            table1.AddCell(hdrCell);
            //Client
            PdfPCell cell11 = new PdfPCell();
            cell11.Colspan = 1;
            cell11.AddElement(new Phrase("CUSTOMER", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));
            cell11.VerticalAlignment = Element.ALIGN_LEFT;

            PdfPCell cell12 = new PdfPCell();
            cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell12.AddElement(new Paragraph("GRETA STEEL"));
            table1.AddCell(cell11);
            table1.AddCell(cell12);

            //Booking Reference
            PdfPCell cell21 = new PdfPCell();
            cell21.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell21.AddElement(new Paragraph("BOOKING REFERENCE", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell22 = new PdfPCell();
            cell22.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell22.AddElement(new Paragraph("9532145547"));
            table1.AddCell(cell21);
            table1.AddCell(cell22);

            //Container number
            PdfPCell cell31 = new PdfPCell();
            cell31.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell31.AddElement(new Paragraph("CONTAINER NUMBER", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell32 = new PdfPCell();
            cell32.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell32.AddElement(new Paragraph("MZ4233645"));
            table1.AddCell(cell31);
            table1.AddCell(cell32);

            //Seal number
            PdfPCell cell41 = new PdfPCell();
            cell41.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell41.AddElement(new Paragraph("SEAL NUMBER", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell42 = new PdfPCell();
            cell42.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell42.AddElement(new Paragraph("MLK85214"));
            table1.AddCell(cell41);
            table1.AddCell(cell42);

            //Gross Weight
            PdfPCell cell51 = new PdfPCell();
            cell51.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell51.AddElement(new Paragraph("GROSS WEIGHT", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell52 = new PdfPCell();
            cell52.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell52.AddElement(new Paragraph("29780KG"));
            table1.AddCell(cell51);
            table1.AddCell(cell52);

            //Tare Weight
            PdfPCell cell61 = new PdfPCell();
            cell61.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell61.AddElement(new Paragraph("TARE WEIGHT", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell62 = new PdfPCell();
            cell62.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell62.AddElement(new Paragraph("2000KG"));
            table1.AddCell(cell61);
            table1.AddCell(cell62);

            //Nett Weight
            PdfPCell cell71 = new PdfPCell();
            cell71.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell71.AddElement(new Paragraph("NETT WEIGHT", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell72 = new PdfPCell();
            cell72.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell72.AddElement(new Paragraph("25780KG"));
            table1.AddCell(cell71);
            table1.AddCell(cell72);

            //Product
            PdfPCell cell81 = new PdfPCell();
            cell81.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell81.AddElement(new Paragraph("PRODUCT", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell82 = new PdfPCell();
            cell82.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell82.AddElement(new Paragraph("TESTING"));
            table1.AddCell(cell81);
            table1.AddCell(cell82);

            //Depot name
            PdfPCell cell91 = new PdfPCell();
            cell91.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell91.AddElement(new Paragraph("DEPOT NAME", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell92 = new PdfPCell();
            cell92.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell92.AddElement(new Paragraph("HCT"));
            table1.AddCell(cell91);
            table1.AddCell(cell92);

            //Delivery note
            PdfPCell cell101 = new PdfPCell();
            cell101.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell101.AddElement(new Paragraph("DELIVERY NOTE", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell102 = new PdfPCell();
            cell102.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell102.AddElement(new Paragraph("039DK"));
            table1.AddCell(cell101);
            table1.AddCell(cell102);

            //Transport
            PdfPCell cell111 = new PdfPCell();
            cell111.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell111.AddElement(new Paragraph("TRANSPORT", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell112 = new PdfPCell();
            cell112.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell112.AddElement(new Paragraph("AVIS"));
            table1.AddCell(cell111);
            table1.AddCell(cell112);

            //Date In
            PdfPCell cell121 = new PdfPCell();
            cell121.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell121.AddElement(new Paragraph("DATE IN", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell122 = new PdfPCell();
            cell122.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell122.AddElement(new Paragraph("2014/06/01"));
            table1.AddCell(cell121);
            table1.AddCell(cell122);

            //Date Out
            PdfPCell cell131 = new PdfPCell();
            cell131.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell131.AddElement(new Paragraph("DATE OUT", FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

            PdfPCell cell132 = new PdfPCell();
            cell132.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell132.AddElement(new Paragraph("2014/06/20"));
            table1.AddCell(cell131);
            table1.AddCell(cell132);


            PdfContentByte content = writer.DirectContent;
            Barcode128 bar128 = new Barcode128();
            bar128.Code = DateTime.Now.ToString();


            Image imgs = bar128.CreateImageWithBarcode(content, null, null);

            imgs.Alignment = Element.ALIGN_MIDDLE;
            //imgs.Border = 1;
            // imgs.WidthPercentage = 5;

            Paragraph par = new Paragraph();
            doc.Add(table1);
            doc.Add(par);
            doc.Add(imgs);

            doc.Close();
        }


        public void PrintTicketSlip()
        {
            var pageRectangle = new Rectangle(226f, 567f);
            //            var document = new Document(PageSize.A6, 88f, 88f, 10f, 10f);
            var document = new Document(PageSize.A4);
            using (var memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;
                BaseColor color = null;

                document.Open();

                //Header Table
                table = new PdfPTable(1) { HorizontalAlignment = Element.ALIGN_LEFT };
                table.DefaultCell.Border = 2;
                table.WidthPercentage = 80;
                table.HorizontalAlignment = Element.ALIGN_MIDDLE;

                //Company Name and Address
                phrase = new Phrase
                {
                    new Chunk("CONATINER LOADING SHEET",
                        FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD))
                };

                table.AddCell(PhraseCell(phrase, Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Colspan = 5;
                table.AddCell(cell);

                //Separater Line
                //                color = BaseColor.BLACK;
                //                DrawLine(writer, 85f, document.Top - 25f, document.PageSize.Width - 240f, document.Top - 25f, color);
                document.Add(table);

                table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_LEFT };
                table.DefaultCell.Border = 2;
                table.WidthPercentage = 80;
                table.HorizontalAlignment = Element.ALIGN_LEFT;

                //Transaction number
                table.AddCell(
                    PhraseCell(
                        new Phrase("Customer name:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(string.Format("{0}", "GREATER TRADING"),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.ITALIC, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Registration number
                table.AddCell(
                    PhraseCell(
                        new Phrase("Container number:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                phrase =
                    new Phrase(new Chunk("MNASDJY62329" + "\n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Date 
                table.AddCell(
                    PhraseCell(
                        new Phrase("PRODUCT:", FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                phrase =
                    new Phrase(new Chunk("HMS" + "\n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Supplier
                table.AddCell(
                    PhraseCell(
                        new Phrase("SEAL Number:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase("MADK23400923",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Driver
                table.AddCell(
                    PhraseCell(
                        new Phrase("Delivery note:", FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(
                            string.Format("{0}", "23489"),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Product
                table.AddCell(
                    PhraseCell(
                        new Phrase("Order number:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase("2392DHG",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Comments
                table.AddCell(
                    PhraseCell(
                        new Phrase("Date in:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(DateTime.Now.ToString("d"),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Add line
                //                color = BaseColor.BLACK;
                //                DrawLine(writer, 177f, document.Top - 105f, document.PageSize.Width - 300f, document.Top - 105f, color);
                //Driver Signature
                table.AddCell(
                    PhraseCell(
                        new Phrase("Driver Signature:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);

                //Add line
                //                color = BaseColor.BLACK;
                //                DrawLine(writer, 177f, document.Top - 116f, document.PageSize.Width - 300f, document.Top - 116f, color);

                document.Add(table);
                document.Close();
                byte[] bytes = memoryStream.ToArray();


                using (FileStream fs = File.Create("C:\\LoadingSheet.pdf"))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
                //                var fs2 = new FileStream("C:\\Test.pdf", FileMode.Open);
                //                var reader = new StreamReader(fs2);
                //                string result = reader.ReadToEnd();
                //                var printDialog = new PrintDialog();
                //                if (printDialog.ShowDialog() == true)
                //                {
                //                    var doc = new FlowDocument(new Paragraph(new Run()))
                //                    {
                //                        Name = "FlowDoc"
                //                    };
                //                    IDocumentPaginatorSource idpSource = doc;
                //                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");
                //                }
                memoryStream.Close();
            }
        }


        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            var cell = new PdfPCell(phrase)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = align,
                PaddingBottom = 1f,
                PaddingTop = 0f
            };
            return cell;
        }

        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            Image image = Image.GetInstance("");
            image.ScalePercent(scale);
            var cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }
    }
}
