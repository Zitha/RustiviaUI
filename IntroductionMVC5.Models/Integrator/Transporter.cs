using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class Transporter
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
