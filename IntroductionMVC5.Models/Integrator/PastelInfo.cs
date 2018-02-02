using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Models
{
    public class PastelInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Pastel number")]
        public string PastelNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please upload pastel file")]
        public string FileLocation { get; set; }

        public string VatFile { get; set; }

        [Required]
        public WeighBridgeInfo WeighBridgeInfo { get; set; }
    }
}