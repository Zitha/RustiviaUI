using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.PettyCash
{
    public class Account
    {
        public int Id { get; set; }

        [Display(Name = "First name")]
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        public string Status { get; set; }
    }
}