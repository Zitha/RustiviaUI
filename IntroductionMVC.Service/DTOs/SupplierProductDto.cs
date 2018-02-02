using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    public class SupplierProductDto
    {
        [DataMember]
        public int SupplierId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public decimal SupplierPrice { get; set; }
    }
}