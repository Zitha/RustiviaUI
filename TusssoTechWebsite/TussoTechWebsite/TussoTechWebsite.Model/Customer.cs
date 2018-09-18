using System.Collections.Generic;
using System.ComponentModel;

namespace TussoTechWebsite.Model
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }

        public string EmailAddress { get; set; }

        [DisplayName("VAT number")]
        public string VatNumber { get; set; }

        public List<Invoice> Invoices { get; set; }

        public List<Resource> Resources { get; set; }
    }
}