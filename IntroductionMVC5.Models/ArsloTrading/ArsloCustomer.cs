using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.ArsloTrading
{
    public class ArsloCustomer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Customer name")]
        public string CustomerName { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Tell number")]
        public string TellNumber { get; set; }
        
        [DisplayName("Profomas")]
        public List<ArsloProfoma> Profomas { get; set; }
    }
}
