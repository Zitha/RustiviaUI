using IntroductionMVC5.Models.ArsloTrading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntroductionMVC5.Web.ViewModel
{
    public class ArsloViewModel
    {
        public List<ArsloCustomer> Customers { get; set; }

        public List<ArsloProfoma> Profomas { get; set; }

        public List<ArsloInvoice> Invoices { get; set; }
    }
}