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
    public class HomeVozniParkController : Controller
    {

        public HomeVozniParkController() : this(new BexUow(), new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new ExceptionSolver())
        { }
        public HomeVozniParkController(
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
           // ViewBag.VpAlarmTip = new SelectList(BexUow.VozniParkDnevnikTip.AllAsNoTracking.Where(x => x.GrupaId == 11 || x.GrupaId == 21 || x.GrupaId == 31), "NazivTipa", "NazivTipa");

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
        public ActionResult GetAlarm(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var skip = (pageNumber - 1) * pageSize;

            DateTime danas = DateTime.Today;
            DateTime danasPre7 = danas.AddDays(-Convert.ToDouble(7));
            DateTime danasZaMesec = danas.AddMonths(1);

            var alarmData = BexUow.VozniParkAlarm.GetAll(true)
                                                    //.Where(x=>x.DatumIsteka > danasPre15 && x.DatumIsteka < danasZa15)
                                                    .Where(x => x.DatumIsteka > danasPre7 && x.DatumIsteka < danasZaMesec)
                                                    .Where(x => x.VozniParkDnevnikTip.GrupaId == 11 || x.VozniParkDnevnikTip.GrupaId == 21 || x.VozniParkDnevnikTip.GrupaId == 31)
                                                    .Select(x =>
                                                         new AlarmIndexData
                                                         {
                                                             Id = x.Id,
                                                             VpDnevnikId = x.VpDnevnikId,
                                                             Vozilo = x.VozniPark.NazivVozila,
                                                             Registracija=x.VozniPark.Oznaka,
                                                             Alarm = x.VozniParkDnevnikTip.NazivTipa,
                                                             Km = x.Km,
                                                             KmIsteka = x.KmIsteka,
                                                             Datum = x.Datum,
                                                             DatumIsteka = x.DatumIsteka,
                                                             DatumAlarma = x.DatumAlarma,
                                                             Napomena = x.Napomena.ToString()

                                                         }).AsEnumerable();
            if (!String.IsNullOrEmpty(searchTerms))
            {
                alarmData = GetSearchAlarmData(searchTerms, alarmData).AsEnumerable();
            }

            var total = alarmData.Count();

            //if (sortColumn == "")
            //{
            //    sortColumn = "DatumIsteka";
            //    sortOrder = "desc";
            //}

            if (sortOrder.Equals("desc"))
                alarmData = alarmData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                alarmData = alarmData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "DatumIsteka" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<AlarmIndexData>()
            {
                total = total,
                rows = alarmData
            };

            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        public IEnumerable<AlarmIndexData> GetSearchAlarmData(string searchTerms, IEnumerable<AlarmIndexData> alarmData)
        {
            string[] terms = searchTerms.Split(',');

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');

                string searchColumn = "";
                string searchTxt = "";

                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("Datum") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    alarmData = alarmData.Where(k => k.Datum >= datumFirst && k.Datum <= datumSecond);
                }
                else if (searchColumn.Equals("DatumIsteka") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    alarmData = alarmData.Where(k => k.DatumIsteka.Value.Date >= datumFirst && k.DatumIsteka.Value.Date <= datumSecond);

                }
                else if (searchColumn.Equals("DatumAlarma") && !String.IsNullOrEmpty(searchTxt))
                {
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));
                    alarmData = alarmData.Where(k => k.DatumAlarma.Value >= datumFirst && k.DatumAlarma.Value <= datumSecond);
                }

                else if (searchColumn.Equals("Vozilo") && !String.IsNullOrEmpty(searchTxt))
                {
                    alarmData = alarmData.Where(k => k.Vozilo.ToUpper().Contains(searchTxt.ToUpper()));
                }
                
                else if (searchColumn.Equals("Registracija") && !String.IsNullOrEmpty(searchTxt))
                {
                    alarmData = alarmData.Where(k => k.Registracija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Alarm") && !String.IsNullOrEmpty(searchTxt))
                {
                    alarmData = alarmData.Where(k => k.Alarm.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Km") && !String.IsNullOrEmpty(searchTxt))
                {
                    int kmInt = System.Convert.ToInt32(searchTxt);
                    alarmData = alarmData.Where(k => k.Km.Equals(kmInt));
                }
                else if (searchColumn.Equals("KmIsteka") && !String.IsNullOrEmpty(searchTxt))
                {
                    int KmIstekaInt = System.Convert.ToInt32(searchTxt);
                    alarmData = alarmData.Where(k => k.Km.Equals(KmIstekaInt));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    alarmData = alarmData.Where(k => k.Alarm.ToUpper().Contains(searchTxt.ToUpper()));
                }


            }
            return alarmData;
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

            DateTime datum = DateTime.Today.AddDays(-Convert.ToDouble(10));
            var gorivoData = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.Datum >= datum)
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

            DateTime datum = DateTime.Today.AddDays(-Convert.ToDouble(10));

            var kmData = BexUow.PutniNalog.AllAsNoTracking.Where(x => x.DatumStart>= datum)
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