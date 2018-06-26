using System;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.ArsloTrading
{
    public class ArsloProfomaDrawDown
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public decimal? Amount { get; set; }
    }
}