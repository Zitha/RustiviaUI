using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.PettyCash
{
    public class PaymentType
    {
        public int Id { get; set; }

        [Display(Name = "Payment type")]
        [Required]
        public string Type { get; set; }
    }
}