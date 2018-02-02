using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class PurchaseDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public TruckDto Truck { get; set; }

        [DataMember]
        public DriverDto Driver { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }

        [DataMember]
        public WeighBridgeInfoDto WeighBridgeInfo { get; set; }
    }
}