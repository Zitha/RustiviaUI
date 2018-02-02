using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class ProductDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal RustiviaPrice { get; set; }
    }
}