using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class WeighBridgeInfo
    {
        [Key]
        public int Id { get; set; }

        public Int64 FirstMass { get; set; }

        public Int64 SecondMass { get; set; }

        [Required]
        [DisplayName("Nett mass")]
        public Int64 NettMass { get; set; }

        [Required]
        public DateTime DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public string Comments { get; set; }

        public string Product { get; set; }
    }
}