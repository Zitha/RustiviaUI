using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public string PaymentType { get; set; }

        public string PaymentReference { get; set; }

        public virtual Truck Truck { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        [Required]
        public Driver Driver { get; set; }

        [Required]
        public WeighBridgeInfo WeighBridgeInfo { get; set; }
    }
}