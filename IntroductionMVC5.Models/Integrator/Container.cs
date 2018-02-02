using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class Container
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Container number")]
        [Required(ErrorMessage = "Container number is required")]
        public string ContainerNumber { get; set; }

        [DisplayName("Seal number")]
        public string Sealnumber { get; set; }

        [DisplayName("Gross weight")]
        public long GrossWeight { get; set; }

        [DisplayName("Tare weight")]
        public long TareWeight { get; set; }

        [DisplayName("Nett weight")]
        public long NettWeight { get; set; }

        public string Product { get; set; }

        public string Status { get; set; }

        [DisplayName("Delivery note")]
        public string DeliveryNote { get; set; }

        [DisplayName("Depot name")]
        public string DepotName { get; set; }

        public bool Paid { get; set; }

        [DisplayName("Truck registration number")]
        public string TruckRegNumber { get; set; }

        public string Invoice1 { get; set; }

        public string Invoice2 { get; set; }

        [DisplayName("Date In")]
        public DateTime DateIn { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual WeighBridgeInfo WeighBridgeInfo { get; set; }
    }
}