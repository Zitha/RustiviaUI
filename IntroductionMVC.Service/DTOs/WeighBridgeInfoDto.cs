using System;
using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class WeighBridgeInfoDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Int64 FirstMass { get; set; }

        [DataMember]
        public Int64 SecondMass { get; set; }

        [DataMember]
        public Int64 NettMass { get; set; }

        [DataMember]
        public DateTime DateIn { get; set; }

        [DataMember]
        public DateTime? DateOut { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public string Product { get; set; }
    }
}