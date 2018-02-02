using IntroductionMVC5.Models.ArsloTrading;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionMVC5.Data.Configuration.ArsloTrading
{
    public class ArsloProfomaItemConfiguration : EntityTypeConfiguration<ArsloProfomaItem>
    {
        public ArsloProfomaItemConfiguration()
        {
            HasKey(a => a.Id);
        }
    }
}
