using IntroductionMVC5.Models.ArsloTrading;
using IntroductionMVC5.Web.Utils.PagedList;
using System.Collections.Generic;

namespace IntroductionMVC5.Web.ViewModel
{
    public class ArsloViewModel
    {
        public List<ArsloCustomer> Customers { get; set; }

        public IPagedList<ArsloProfoma> Profomas { get; set; }

        public List<ArsloInvoice> Invoices { get; set; }

        public string ActiveTab { get; set; }
    }
}