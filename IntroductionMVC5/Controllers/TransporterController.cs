using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class TransporterController : Controller
    {
        private readonly ApplicationUnit _unit = new ApplicationUnit();
        private List<Transporter> _transporters = new List<Transporter>();

        public TransporterController()
        {
            _transporters = GetTransporters();
        }

        //
        // GET: /Transpoter/
        public ActionResult Index()
        {
            return View(_transporters);
        }

        [HttpPost]
        public ActionResult Create(Transporter transporter)
        {
            if (ModelState.IsValid)
            {
                _unit.Transporter.Add(transporter);
                _unit.SaveChanges();
                _transporters = GetTransporters();
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public ActionResult Create()
        {
            if (ModelState.IsValid)
            {
                //                _unit.Transporter.Add(transporter);
                _unit.SaveChanges();
                _transporters = GetTransporters();
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        private List<Transporter> GetTransporters()
        {
            List<Transporter> transporters = _unit.Transporter.GetAll().ToList();
            return transporters;
        }
    }
}