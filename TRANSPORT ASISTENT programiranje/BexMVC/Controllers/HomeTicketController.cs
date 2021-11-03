using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common;
using Bex.Common.Interfaces;
using Bex.DAL.EF.UOW;
using Bex.MVC.Exceptions;
using BexMVC.Filters;
using BexMVC.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace BexMVC.Controllers
{
    [Authorize]
    //[ClaimsAuthentication(Resource = "Home", Operation = "Read")]
    public class HomeTicketController : Controller
    {

        public HomeTicketController() : this(new BexUow(), new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new ExceptionSolver())
        { }
        public HomeTicketController(
            IBexUow bexUow,
            ISecurityUow securityUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            SecurityUow = securityUow; 
            ExceptionSolver = exceptionSolver;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetVozila()
        {

            var vozniPark = BexUow.VozniPark.AllAsNoTracking;
            
            int vozilaAktivna = vozniPark.Where(x => x.StatusVozilaId == 1).Count(); 
            int vozilaServis = vozniPark.Where(x => x.StatusVozilaId == 2).Count();

            var vozilaData = new
            {
                aktivnaVozila = vozilaAktivna,
                servisVozila = vozilaServis
            };

            return Json(vozilaData, "", JsonRequestBehavior.AllowGet);
            
        }

        [HttpGet]
        public ActionResult GetPutniNalozi()
        {

            var putniNalog = BexUow.PutniNalog.AllAsNoTracking;
            int nezatvoreniPutniNalozi = putniNalog.Where(x => x.Storno == false && x.KmUkupno == 0).Count();

            var vozniPark = BexUow.VozniPark.AllAsNoTracking.Where(x => x.FirmaOSid == 1 && !x.GarazniBroj.StartsWith("X") && x.StatusVozilaId != 3 && x.StatusVozilaId != 5);
            int bezPutnogNaloga = 0;
            foreach (var vp in vozniPark)
            {
                DateTime danas = DateTime.Now;
                var putniNalogLast = BexUow.PutniNalog.GetAll(true).Where(x => x.VpId == vp.Id && x.DatumStart == danas).OrderByDescending(z => z.Id).Take(1).FirstOrDefault();
                if(putniNalogLast == null)
                {
                    bezPutnogNaloga++;
                }
            }
           

            var vozilaData = new
            {
                nezatvoreniPutniNalozi,
                bezPutnogNaloga 
            };

            return Json(vozilaData, "", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetAlarm()
        {
            DateTime danas = DateTime.Today;
            DateTime danasPre15 = danas.AddDays(-Convert.ToDouble(15));
            DateTime danasZa15 = danas.AddDays(Convert.ToDouble(15));
            var alarm = BexUow.VozniParkAlarm.GetAll(true)
                                                    .Where(x=>x.DatumIsteka > danasPre15 && x.DatumIsteka < danasZa15)
                                                    .Where(x => x.VozniParkDnevnikTip.GrupaId == 11 || x.VozniParkDnevnikTip.GrupaId == 21 || x.VozniParkDnevnikTip.GrupaId == 31)
                                                    .Select(x =>
                                                         new AlarmIndexData
                                                         {
                                                             Id = x.Id,
                                                             Vozilo = x.VozniPark.NazivVozila,
                                                             Registracija=x.VozniPark.Oznaka,
                                                             Alarm = x.VozniParkDnevnikTip.NazivTipa,
                                                             Km = x.Km,
                                                             KmIsteka = x.KmIsteka,
                                                             Datum = x.Datum,
                                                             DatumIsteka = x.DatumIsteka,
                                                             DatumAlarma = x.DatumAlarma,
                                                             Napomena = x.Napomena.ToString()

                                                         }).OrderByDescending(x=>x.DatumIsteka)
                                                            .AsEnumerable();

            return Json(alarm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //[ClaimsAuthentication(Resource = "Home", Operation = "Poreklo, All")]
        public ActionResult GetGorivoChart(string dateForChart)
        {
            //2019-01-10 to 2019-01-10
            DateTime datumStatFirst, datumStatSecond;
            if (dateForChart != "")
            {
                string[] arrDatumAll = dateForChart.Split('t', 'o');

                string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                datumStatFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                datumStatSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

            }
            else
            {
                datumStatFirst = DateTime.Today;
                datumStatSecond = DateTime.Today;
            }


            var gorivoData = BexUow.GorivoTocenje.AllAsNoTracking
                        .GroupBy(x => x.Datum).OrderBy(x => x.Key)
                        .Select(g => new VozniParkPoDanuChart
                        {
                            PeriodDatumGorivo = g.Key.ToString(),
                            SumGorivo = g.Sum(x => x.Litara > 0 ? x.Litara : 0)
                        });


            return Json(gorivoData, "VozniParkPoDanuChart", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //[ClaimsAuthentication(Resource = "Home", Operation = "Poreklo, All")]
        public ActionResult GetKmChart(string dateForChart)
        {
            //2019-01-10 to 2019-01-10
            DateTime datumStatFirst, datumStatSecond;
            if (dateForChart != "")
            {
                string[] arrDatumAll = dateForChart.Split('t', 'o');

                string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                datumStatFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                datumStatSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

            }
            else
            {
                datumStatFirst = DateTime.Today;
                datumStatSecond = DateTime.Today;
            }

            var kmData = BexUow.PutniNalog.AllAsNoTracking
                        .GroupBy(x => x.DatumStart).OrderBy(x => x.Key)
                        .Select(g => new VozniParkPoDanuChart
                        {
                            
                            PeriodDatumKm = g.Key.ToString(),
                            SumKm = g.Sum(x => x.KmUkupno > 0 ? x.KmUkupno : 0)
                        });


            return Json(kmData, "VozniParkPoDanuChart", JsonRequestBehavior.AllowGet);
        }



        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }

        private ISecurityUow SecurityUow { get; set; }

       

    }
}