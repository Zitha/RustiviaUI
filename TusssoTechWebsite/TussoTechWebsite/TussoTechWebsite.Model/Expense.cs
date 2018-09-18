using System;
using System.ComponentModel.DataAnnotations;

namespace TussoTechWebsite.Model
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateSent { get; set; }

        public string Type { get; set; }

        public string PurchaseNumber { get; set; }

        public double Total { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Employee { get; set; }

        public virtual Company Company { get; set; }
    }
}