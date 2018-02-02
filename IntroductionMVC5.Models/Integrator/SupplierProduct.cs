using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntroductionMVC5.Models.Integrator
{
    public class SupplierProduct
    {
        [Key, Column(Order = 0)]
        public int SupplierId { get; set; }

        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        public virtual SupplierInfo Supplier { get; set; }
        public virtual Product Product { get; set; }

        public decimal SupplierPrice { get; set; }
    }
}