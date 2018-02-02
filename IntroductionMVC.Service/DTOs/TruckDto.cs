using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class TruckDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string TruckRegNumber { get; set; }

        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public bool Own { get; set; }

        [DataMember]
        public virtual SupplierInfoDto SupplierInfo { get; set; }

        [DataMember]
        public List<WeighBridgeInfoDto> WeighBridgeInfo { get; set; }
    }
}