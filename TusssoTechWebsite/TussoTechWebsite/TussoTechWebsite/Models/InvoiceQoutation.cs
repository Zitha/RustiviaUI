using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Models
{
    public class InvoiceQoutation
    {
        public List<OnceOffInvoice> Invoices
        {
            get;
            set;
        }

        public List<Qoutation> Qoutations
        {
            get;
            set;
        }
    }
}