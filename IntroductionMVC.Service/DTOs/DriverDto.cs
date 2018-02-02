using System.Runtime.Serialization;

namespace IntroductionMVC5.Service.DTOs
{
    [DataContract]
    public class DriverDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Firstname { get; set; }

        [DataMember]
        public string IdNumber { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public virtual SupplierInfoDto SupplierInfo { get; set; }
    }
}