using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TussoTechWebsite.Model
{
    public class Company
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Contacts
        {
            get;
            set;
        }

        public virtual List<Resource> Resources
        {
            get;
            set;
        }

        public List<CompanyDocument> Documents
        {
            get;
            set;
        }

        public List<Expense> Expenses
        {
            get;
            set;
        }
    }
}
