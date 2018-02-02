using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class SupplierInfo
    {
        public SupplierInfo()
        {
            Drivers = new List<Driver>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Supplier name")]
        public string SupplierName { get; set; }

        public string Logo { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Tell number")]
        public string TellNumber { get; set; }

        [DisplayName("Supplier code")]
        public string Suppliercode { get; set; }

        [DisplayName("Company registration number")]
        [Required]
        public string CompanyRegNumber { get; set; }

        [DisplayName("VAT number")]
        public string VatNumber { get; set; }

        [DisplayName("Legal note")]
        public string Legalnote { get; set; }

        public List<Driver> Drivers { get; set; }

        public List<Truck> Trucks { get; set; }

        public virtual List<SupplierProduct> SupplierProducts { get; set; }

        public string UserId { get; set; }

        public List<BankAccount> BankAccounts { get; set; }

        public bool IsBlocked { get; set; }
    }
}