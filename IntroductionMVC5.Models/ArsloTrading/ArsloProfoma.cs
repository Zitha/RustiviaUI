using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.ArsloTrading
{
    public class ArsloProfoma
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Profoma Number")]
        public string ProfomaNumber { get; set; }

        [DisplayName("UCRNumber")]
        public string UCRNumber { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Profomas")]
        public ArsloCustomer Customer { get; set; }

        [DisplayName("Invoices")]
        public List<ArsloInvoice> Invoices { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [Required]
        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        public string Location { get; set; }

        public List<ArsloProfomaItem> ProfomaItems { get; set; }
    }
}

