using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class SupplierInfoDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string SupplierCode { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public List<DriverDto> Drivers { get; set; }

        [DataMember]
        public List<TruckDto> Trucks { get; set; }

        [DataMember]
        public List<SupplierProductDto> SupplierProductDtos { get; set; }
    }
}