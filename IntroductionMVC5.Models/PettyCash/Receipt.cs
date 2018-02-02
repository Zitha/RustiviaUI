using System;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.PettyCash
{
    public class Receipt : IPettyCashEntry
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Date")]
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Reference { get; set; }

        public string ExtraInfo { get; set; }

        public string User { get; set; }

        [Display(Name = "Reference no")]
        public string AutoGenNumber { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public decimal PettyAccount { get; set; }

        public Account Account { get; set; }

        [Display(Name = "Cleared")]
        public bool IsCleared { get; set; }

        [Display(Name = "Pastel number")]
        public string PastelNo { get; set; }
    }
}