using System;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.PettyCash
{
    public class EndDayBalance
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string SystemUser { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal ClosingBalance { get; set; }
    }
}