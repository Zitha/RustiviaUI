using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public string TruckRegNumber { get; set; }

        public string ExtraInfo { get; set; }

        [Required]
        public WeighBridgeInfo WeighBridgeInfo { get; set; }

        [Required]
        public Customer Customer { get; set; }
    }
}