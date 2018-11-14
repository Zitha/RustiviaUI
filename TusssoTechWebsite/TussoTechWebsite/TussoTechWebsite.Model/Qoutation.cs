using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TussoTechWebsite.Model
{
    public class Qoutation
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public DateTime DateSent
        {
            get;
            set;
        }

        public string QoutationNumber
        {
            get;
            set;
        }

        public double Total
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public string CustomerName
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public int Customer_Id
        {
            get;
            set;
        }

        public List<Item> Items
        {
            get;
            set;
        }
    }
}
