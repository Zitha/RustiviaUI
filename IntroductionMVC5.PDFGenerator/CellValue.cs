using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustiviaSolutions.PDFGenerator
{
    public class CellValue
    {
        public string Value { get; set; }

        public int FontSize { get; set; }

        public float Padding { get; set; }

        public int HorizontalAlignment { get; set; }

        public BaseColor BorderColor { get; set; }

        public BaseColor BackGroundColor { get; set; }

    }
}
