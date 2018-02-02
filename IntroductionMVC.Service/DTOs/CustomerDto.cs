using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class CustomerDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string CustomerCode { get; set; }


        [DataMember]
        public List<BookingDto> Bookings { get; set; }
    }
}