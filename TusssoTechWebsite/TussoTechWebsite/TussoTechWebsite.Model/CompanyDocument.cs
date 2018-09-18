using System.ComponentModel.DataAnnotations;

namespace TussoTechWebsite.Model
{
    public class CompanyDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }

        public string Type { get; set; }
    }
}