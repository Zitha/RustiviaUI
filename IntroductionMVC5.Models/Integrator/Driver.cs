using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace IntroductionMVC5.Models.Integrator
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First name")]
        [MaxLength(20)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "ID number\\Passport is required")]
        [DisplayName("ID number\\Passport")]
        [MaxLength(15)]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [MaxLength(20)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "ID number\\Passport file is required")]
        [DisplayName("ID number\\Passport file")]
        [JsonIgnore]
        public string IdLocation { get; set; }

        [Required]
        public string ImageName { get; set; }

        public virtual SupplierInfo SupplierInfo { get; set; }

        public bool IsBlocked { get; set; }
    }
}