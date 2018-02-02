using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class BookingDto
    {
        [DataMember]
        public string ReferenceNumber { get; set; }

        [DataMember]
        public List<ContainerDto> Containers { get; set; }
    }
}