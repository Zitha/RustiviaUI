using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace RustiviaSolutions.Models
{
    public class ClientInfo
    {
        public ClientInfo()
        {
            Drivers = new List<Driver>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Supplier name")]
        public string ClientName { get; set; }

        public string Logo { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Tell number")]
        public string TellNumber { get; set; }
        public List<Driver> Drivers { get; set; }
    }
}