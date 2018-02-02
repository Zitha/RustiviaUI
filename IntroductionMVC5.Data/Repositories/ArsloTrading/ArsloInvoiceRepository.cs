﻿using IntroductionMVC5.Models.ArsloTrading;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionMVC5.Data.Repositories.ArsloTrading
{
    public class ArsloInvoiceRepository : GenericRepository<ArsloInvoice>
    {
        public ArsloInvoiceRepository(DbContext context)
            : base(context)
        {
        }
    }
}
