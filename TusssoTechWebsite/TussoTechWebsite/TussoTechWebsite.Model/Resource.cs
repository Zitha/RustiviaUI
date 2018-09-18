using System.ComponentModel.DataAnnotations;

namespace TussoTechWebsite.Model
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string OutCome { get; set; }

        public string Location { get; set; }

        public virtual Customer Customer { get; set; }
    }
}