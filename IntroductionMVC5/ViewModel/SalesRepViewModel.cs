using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Web.Models;

namespace IntroductionMVC5.Web.ViewModel
{
    public class SalesRepViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public SupplierInfo Supplier { get; set; }
        public List<Purchase> Purchases { get; set; }
    }
}