using System.Collections.Generic;
using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Web.ViewModel
{
    public class ProcessedViewModel 
    {
        public List<Purchase> Purchases { get; set; }

        public List<PastelInfo> PastelInfos { get; set; }
    }
}