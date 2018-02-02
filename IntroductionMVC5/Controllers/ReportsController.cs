using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models.Integrator;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.CSharp.RuntimeBinder;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationUnit _unit = new ApplicationUnit();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult DailyPurchase(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTime searchDate = Convert.ToDateTime(date);

                List<Purchase> purchases = _unit.Purchase.GetAll().Include(d => d.Driver)
                    .Include(w => w.WeighBridgeInfo)
                    .OrderBy(s => s.WeighBridgeInfo.Product)
                    .ThenBy(s => s.WeighBridgeInfo.NettMass)
                    .Where(p => EntityFunctions.TruncateTime(p.WeighBridgeInfo.DateIn)
                                == EntityFunctions.TruncateTime(searchDate) && p.Driver.Id != 12).ToList();
                return PartialView(purchases);
            }
            else
            {
                List<Purchase> purchases = _unit.Purchase.GetAll().Include(d => d.Driver)
                    .Include(w => w.WeighBridgeInfo)
                    .OrderBy(s => s.WeighBridgeInfo.Product)
                    .ThenBy(s => s.WeighBridgeInfo.NettMass)
                    .Where(p => EntityFunctions.TruncateTime(p.WeighBridgeInfo.DateIn)
                                == EntityFunctions.TruncateTime(DateTime.Now) && p.Driver.Id != 12).ToList();
                return PartialView(purchases);
            }
        }

        public PartialViewResult ProductPurchase(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTime searchDate = Convert.ToDateTime(date);
                DayOfWeek delta = searchDate.DayOfWeek;
                if (delta > 0)
                    delta -= 7;
                CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                DateTime dayInWeek = searchDate;

                DayOfWeek firstDay = defaultCultureInfo.DateTimeFormat.FirstDayOfWeek;
                DateTime firstDayInWeek = dayInWeek.Date;

                while (firstDayInWeek.DayOfWeek != firstDay)
                    firstDayInWeek = firstDayInWeek.AddDays(-1);

                DateTime lastDayInWeek = firstDayInWeek.AddDays(6);

                List<IGrouping<DateTime, Purchase>> purchases =
                    _unit.Purchase.GetAll()
                        .Include(w => w.WeighBridgeInfo)
                        .Where(
                            p =>
                                (EntityFunctions.TruncateTime(firstDayInWeek) <=
                                 EntityFunctions.TruncateTime(p.WeighBridgeInfo.DateIn)
                                 && EntityFunctions.TruncateTime(p.WeighBridgeInfo.DateIn)
                                 <= EntityFunctions.TruncateTime(lastDayInWeek)) && p.Driver.Id != 12)
                        .ToLookup(p => p.WeighBridgeInfo.DateIn.Date)
                        .ToList();

                return PartialView(purchases);
            }
            else
            {
                DayOfWeek delta = DateTime.Now.DayOfWeek;
                if (delta > 0)
                    delta -= 7;
                CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                DateTime dayInWeek = DateTime.Now;

                DayOfWeek firstDay = defaultCultureInfo.DateTimeFormat.FirstDayOfWeek;
                DateTime firstDayInWeek = dayInWeek.Date;

                while (firstDayInWeek.DayOfWeek != firstDay)
                    firstDayInWeek = firstDayInWeek.AddDays(-1);

                DateTime lastDayInWeek = firstDayInWeek.AddDays(6);

                List<IGrouping<DateTime, Purchase>> purchases =
                    _unit.Purchase.GetAll()
                        .Include(w => w.WeighBridgeInfo)
                        .Where(
                            p =>
                                (EntityFunctions.TruncateTime(firstDayInWeek) <=
                                 EntityFunctions.TruncateTime(p.WeighBridgeInfo.DateIn)
                                 && EntityFunctions.TruncateTime(p.WeighBridgeInfo.DateIn)
                                 <= EntityFunctions.TruncateTime(lastDayInWeek)) && p.Driver.Id != 12)
                        .ToLookup(p => p.WeighBridgeInfo.DateIn.Date)
                        .ToList();

                return PartialView(purchases);
            }
        }
        public ActionResult WeeklyPurchase()
        {
            return
                Redirect(
                    "https://rustivia-pc/ReportServer_SQLEXPRESS/Pages/ReportViewer.aspx?%2fRustiviaReport%2fWeeklySupplies&rs:Command=Render");
        }

        public PartialViewResult MonthlyReport()
        {
            List<IGrouping<int, Purchase>> list = _unit.Purchase.GetAll().Include(w => w.WeighBridgeInfo)
                .Where(p => p.WeighBridgeInfo.DateIn.Year == DateTime.Now.Year && p.Driver.Id != 12).ToLookup<Purchase, int>(p =>
            {
                DateTime dateTime = p.WeighBridgeInfo.DateIn;
                dateTime = dateTime.Date;
                return dateTime.Month;
            }).ToList();


            object[] data = new object[12]
            {
                list.Where( d => d.Key == 1).FirstOrDefault() != null ? list.Where( d => d.Key == 1).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 2).FirstOrDefault() != null ? list.Where( d => d.Key == 2).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 3).FirstOrDefault() != null ? list.Where( d => d.Key == 3).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 4).FirstOrDefault() != null ? list.Where( d => d.Key == 4).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 5).FirstOrDefault() != null ? list.Where( d => d.Key == 5).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 6).FirstOrDefault() != null ? list.Where( d => d.Key == 6).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 7).FirstOrDefault() != null ? list.Where( d => d.Key == 7).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 8).FirstOrDefault() != null ? list.Where( d => d.Key == 8).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 9).FirstOrDefault() != null ? list.Where( d => d.Key == 9).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 10).FirstOrDefault() != null ? list.Where( d => d.Key == 10).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 11).FirstOrDefault() != null ? list.Where( d => d.Key == 11).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0,
                list.Where( d => d.Key == 12).FirstOrDefault() != null ? list.Where( d => d.Key == 12).FirstOrDefault().Sum( w => w.WeighBridgeInfo.NettMass) : 0
            };

            DotNet.Highcharts.Highcharts highcharts = new DotNet.Highcharts.Highcharts("chart").InitChart(new Chart()
            {
                DefaultSeriesType = new ChartTypes?(ChartTypes.Column)
            }).SetTitle(new Title() { Text = "Monthly Purchase" }).SetXAxis(new XAxis()
            {
                Categories = new string[12]
                {
                   "Jan",
                   "Feb",
                   "Mar",
                   "Apr",
                   "May",
                   "Jun",
                   "Jul",
                   "Aug",
                   "Sep",
                   "Oct",
                   "Nov",
                   "Dec"
                }
            }).SetYAxis(new YAxis()
            {
                Title = new YAxisTitle() { Text = "Quantity (KG)" }
            }).SetTooltip(new Tooltip()
            {
                Enabled = new bool?(true),
                Formatter = "function () { return '<b>' + this.series.name + '</b><br/>'+ this.x +': '+ this.y + ' KG'; }"
            }).SetPlotOptions(new PlotOptions()
            {
                Line = new PlotOptionsLine()
                {
                    DataLabels = new PlotOptionsLineDataLabels()
                    {
                        Enabled = new bool?(true)
                    },
                    EnableMouseTracking = new bool?(false)
                }
            }).SetSeries(new Series[1]
            {
               new Series() { Name = "Months", Data = new DotNet.Highcharts.Helpers.Data(data) }
            });

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string str = JsonConvert.SerializeObject(data, settings);


            //// ISSUE: reference to a compiler-generated field
            //if (ReportsController.\u003CMonthlyReport\u003Eo__SiteContainer15.\u003C\u003Ep__Site16 == null)
            //{
            //    // ISSUE: reference to a compiler-generated field
            //    ReportsController.\u003CMonthlyReport\u003Eo__SiteContainer15.\u003C\u003Ep__Site16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "MonthlyTotal", typeof(ReportsController), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[2]
            //    {
            //    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            //    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            //    }));
            //}
            //// ISSUE: reference to a compiler-generated field
            //// ISSUE: reference to a compiler-generated field
            //object obj = ReportsController.\u003CMonthlyReport\u003Eo__SiteContainer15.\u003C\u003Ep__Site16.Target((CallSite)ReportsController.\u003CMonthlyReport\u003Eo__SiteContainer15.\u003C\u003Ep__Site16, this.ViewBag, str);

            return PartialView(highcharts);
        }
    }
}