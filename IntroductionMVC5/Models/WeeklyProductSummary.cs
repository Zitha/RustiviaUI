using System.Collections.Generic;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using iTextSharp.text;

namespace IntroductionMVC5.Web.Models
{
    public class WeeklyProductSummary
    {
        public List<Purchase> Agrade { get; set; }
        public List<Purchase> Sugrade { get; set; }
        public List<Purchase> Bluesteel { get; set; }
        public List<Purchase> Lightsteel { get; set; }
        public List<Purchase> Generalsteel { get; set; }
    }
}