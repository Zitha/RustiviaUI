using IntroductionMVC5.Models;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Web.ViewModel
{
    public class ProcessViewModel
    {
        public Purchase Purchase { get; set; }
        public SupplierInfo SupplierInfo { get; set; }
        public PastelInfo PastelInfo { get; set; }
        public Container Container { get; set; }
        public Sale Sale { get; set; }
    }
}