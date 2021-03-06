﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TussoTechWebsite.Model;
using System.Web;

namespace TussoTechWebsite.PDFGenerator
{
    public class QouteGenerator : IDisposable
    {
        private string _qoutationPath;

        public QouteGenerator()
        {
            //_qoutationPath = HttpContext.Current.Server.MapPath("~/Qoutation");
            _qoutationPath = @"C:\Dev\Tusso\TussoTechWebsite\TussoTechWebsite\Qoutation";
            if (!Directory.Exists(_qoutationPath))
            {
                Directory.CreateDirectory(_qoutationPath);
            }
        }

        public string CreateQoute(Qoutation qoutation)
        {
            var doc = new Document(PageSize.A4);
            _qoutationPath = string.Format("{0}\\{1}.pdf", _qoutationPath, qoutation.QoutationNumber);
            var output = new FileStream(_qoutationPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(doc, output);

            doc.AddTitle("Quotation");

            doc.Open();

            var mainTable = new PdfPTable(2);
            mainTable.DefaultCell.Border = 2;
            mainTable.WidthPercentage = 80;
            mainTable.HorizontalAlignment = Element.ALIGN_MIDDLE;
            //Header Table
            var headerTable = new PdfPTable(2)
            {
                HorizontalAlignment = Element.ALIGN_MIDDLE,
                WidthPercentage = 80
            };

            var infoPhrase = new Phrase
            {
                new Chunk("Tusso Technologies\n\n",
                    FontFactory.GetFont("Microsoft Sans Serif", 16, Font.BOLD, BaseColor.BLACK)),
                new Chunk(string.Format("{0}, {1}", "2 Keswic", "Nanyuki \n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                new Chunk(string.Format("{0}", "Sunninghill\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK)),
                    new Chunk(string.Format("Phone: {0}", "(083) 473-1660 / (072) 631-5461\n"),
                    FontFactory.GetFont("Microsoft Sans Serif", 10, Font.NORMAL, BaseColor.BLACK))
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
                new PdfPCell(new Phrase("QUOTE",
                    FontFactory.GetFont(FontFactory.HELVETICA, 26, Font.BOLD, BaseColor.GRAY)))
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
            var qoutationNumberTable = new PdfPTable(2)
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                WidthPercentage = 50
            };

            //Labels For Invoice number and Date
            var qouteNumberCellLbl = new PdfPCell
            {
                Colspan = 1,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            qouteNumberCellLbl.AddElement(new Phrase("QOUTE NUMBER",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.GRAY)));
            qouteNumberCellLbl.VerticalAlignment = Element.ALIGN_LEFT;

            var dateCellLbl = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GRAY
            };
            dateCellLbl.AddElement(new Phrase("DATE",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.GRAY)));

            qoutationNumberTable.AddCell(qouteNumberCellLbl);
            qoutationNumberTable.AddCell(dateCellLbl);

            //Data For Invoice number and Date
            var qouteNumberCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.GRAY
            };
            qouteNumberCellData.AddElement(new Phrase(qoutation.QoutationNumber,
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
            qouteNumberCellData.VerticalAlignment = Element.ALIGN_LEFT;

            var dateCellData = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderColor = BaseColor.GRAY
            };
            dateCellData.AddElement(new Phrase(DateTime.Now.ToString("yyyy-MM-dd"),
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));

            qoutationNumberTable.AddCell(qouteNumberCellData);
            qoutationNumberTable.AddCell(dateCellData);

            qoutationNumberTable.SpacingBefore = 10f;
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
                BorderColor = BaseColor.GRAY,
                PaddingTop = 10f,
                PaddingLeft = 50f
            };
            billignCellLbl.AddElement(new Phrase("QUOTE TO",
                FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.GRAY)));
            billignCellLbl.VerticalAlignment = Element.ALIGN_LEFT;
            billignInfoTable.AddCell(billignCellLbl);

            //Billing Information Label
            var billignCellData = new PdfPCell
            {
                Colspan = 1,
                BorderColor = BaseColor.WHITE
            };
            billignCellData.AddElement(new Phrase(qoutation.CustomerName + "\n" + qoutation.Address,
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
            descriptionCellLbl.AddElement(new Phrase("DESRIPTION",
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
            double total = 0;
            foreach (var item in qoutation.Items)
            {
                total = total + item.Quantity * item.UnitPrice;
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
                item3.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", item.UnitPrice)),
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
                item4.AddElement(new Phrase(string.Format("R {0}", String.Format("{0:n}", item.UnitPrice * item.Quantity)),
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

            var conactHeaderPr = new Paragraph
            {
                SpacingBefore = 20f
            };
            Phrase p2 = new Phrase
            {
                new Phrase("Contact Person",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK))
            };

            var conact1Pr = new Paragraph
            {
                SpacingBefore = 10f
            };

            var contact1 = new Phrase("Ndavhe Tshivhase \n Ndavhe@Tussotechnologies.co.za \n +2772 631 5461\n",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));


            var conact2Pr = new Paragraph
            {
                SpacingBefore = 10f
            };
            var contact2 = new Phrase("Dickson Tshikovhi \n Riwanise@Tussotechnologies.co.za \n +2783 473 1660",
                FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));

            conactHeaderPr.Add(p2);
            conact1Pr.Add(contact1);
            conact2Pr.Add(contact2);

            doc.Add(headerTable);
            doc.Add(mainTable);
            doc.Add(qoutationNumberTable);
            doc.Add(billignInfoTable);
            doc.Add(itemsTable);
            doc.Add(conactHeaderPr);
            doc.Add(conact1Pr);
            doc.Add(conact2Pr);

            doc.Close();
            writer.Close();
            output.Close();

            if (output != null)
            {
                output.Dispose();
            }
            if (doc != null)
            {
                doc.Dispose();
            }
            if (writer != null)
            {
                writer.Dispose();
            }

            return _qoutationPath;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
