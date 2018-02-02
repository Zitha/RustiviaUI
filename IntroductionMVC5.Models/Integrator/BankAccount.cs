using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class BankAccount
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [Required]
        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Branch Number")]
        public string BranchNumber { get; set; }

        public virtual SupplierInfo SupplierInfo { get; set; }
    }
}