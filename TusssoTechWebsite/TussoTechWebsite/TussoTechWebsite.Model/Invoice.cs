using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TussoTechWebsite.Model
{
    public class Invoice
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

        public string InvoiceNumber
        {
            get;
            set;
        }

        public double Total
        {
            get;
            set;
        }

        public string Status
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

        public Customer Customer
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
