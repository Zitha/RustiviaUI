using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models.Integrator
{
    public class Booking
    {
        public Booking()
        {
            Containers = new List<Container>();
        }

        [Key]
        [DisplayName("Reference number")]
        public string Reference { get; set; }

        [DisplayName("Date In")]
        public DateTime DateIn { get; set; }

        [DisplayName("Date out")]
        public DateTime? DateOut { get; set; }

        public string Status { get; set; }

        public Transporter Transporter { get; set; }

        public virtual Customer Customer { get; set; }

        public List<Container> Containers { get; set; }
    }
}