using System.Collections.Generic;

namespace IntroductionMVC5.Models.Integrator
{
    public class Role
    {
        public Role()
        {
            Users = new List<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}