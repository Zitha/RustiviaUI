using System;
using System.ComponentModel.DataAnnotations;

namespace TussoTechWebsite.Model
{
    public class BankStatement
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateSent { get; set; }

        public double AccountAmount { get; set; }

        public string Location { get; set; }
    }
}