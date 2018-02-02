using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Registration number")]
        [Required(ErrorMessage = "Truck Registration is required")]
        public string TruckRegNumber { get; set; }

        public string Image { get; set; }

        public bool Own { get; set; }

        public virtual SupplierInfo SupplierInfo { get; set; }

        public List<WeighBridgeInfo> WeighBridgeInfo { get; set; }
    }
}