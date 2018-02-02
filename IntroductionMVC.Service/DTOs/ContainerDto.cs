using System;
using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class ContainerDto
    {
        [DataMember]
        public string ContainerNumber { get; set; }

        [DataMember]
        public string Sealnumber { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string TruckRegNumber { get; set; }

        [DataMember]
        public long NettWeight { get; set; }

        [DataMember]
        public DateTime DateIn { get; set; }

        [DataMember]
        public long TareWeight { get; set; }

        [DataMember]
        public BookingDto Booking { get; set; }

        [DataMember]
        public WeighBridgeInfoDto WeighBridgeInfo { get; set; }
    }
}