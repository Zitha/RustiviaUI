using System;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.PettyCash
{
    public class Payment : IPettyCashEntry
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        [Required]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Display(Name = "Pastel number")]
        public string PastelNo { get; set; }

        [Display(Name = "Reference no")]
        public string AutoGenNumber { get; set; }

        public decimal Amount { get; set; }

        public decimal PettyAccount { get; set; }

        [Display(Name = "Cleared")]
        public bool IsCleared { get; set; }

        public PaymentType PaymentType { get; set; }

        public Account Account { get; set; }

        public string User { get; set; }
    }
}