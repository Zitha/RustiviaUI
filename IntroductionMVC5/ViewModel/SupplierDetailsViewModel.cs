using System.Collections.Generic;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Web.ViewModel
{
    public class SupplierDetailsViewModel : ProcessedViewModel
    {
        public SupplierInfo SupplierInfo { get; set; }

        public List<SupplierProduct> SupplierProducts { get; set; }

        public List<BankAccount> SupplierBankAccounts { get; set; }
    }
}