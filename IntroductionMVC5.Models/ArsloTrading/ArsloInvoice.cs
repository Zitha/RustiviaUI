using System;
using System.Collections.Generic;

namespace IntroductionMVC5.Models.ArsloTrading
{
    public class ArsloInvoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public string InvoiceLocation { get; set; }
        public ArsloCustomer Customer { get; set; }
        public List<ArsloInvoiceItem> InvoiceItems { get; set; }
        public string PointOfLoading { get; set; }
        public string PointOfDelivery { get; set; }
        public virtual ArsloProfoma Profoma { get; set; }
        public string  BookingNumber { get; set; }
        public string VesselNumber { get; set; }
    }
}
