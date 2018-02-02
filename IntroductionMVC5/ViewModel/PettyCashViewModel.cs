using System;
using System.Collections.Generic;
using IntroductionMVC5.Models.PettyCash;
using IntroductionMVC5.Web.Utils.PagedList;

namespace IntroductionMVC5.Web.ViewModel
{
    public class PettyCashViewModel
    {
        public IPagedList<IPettyCashEntry> DailyActiviList { get; set; }
        public Account PettyCashAccount { get; set; }
        public EndDayBalance EndDayBalance { get; set; }
        public EndDayBalance BalanceInfomation { get; set; }
        public DateTime PettyCashDate { get; set; }
        public List<Account> Accounts { get; set; }
        public bool IsLastDayBalanced { get; set; }
    }
}