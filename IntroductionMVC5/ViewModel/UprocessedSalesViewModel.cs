using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Web.ViewModel
{
    public class UprocessedSalesViewModel
    {
        public List<Sale> Sales { get; set; }

        public List<Container> Containers { get; set; }
    }
}