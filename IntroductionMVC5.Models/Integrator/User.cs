using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICollection<Role> Roles { get; set; }

        public User()
        {
            this.Roles = new List<Role>();
        }
    }
}