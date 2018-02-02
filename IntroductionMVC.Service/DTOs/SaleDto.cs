using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class SaleDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string TruckRegNumber { get; set; }

        [DataMember]
        public string ExtraInfo { get; set; }

        [DataMember]
        public WeighBridgeInfoDto WeighBridgeInfo { get; set; }

        [DataMember]
        public CustomerDto Customer { get; set; }
    }
}