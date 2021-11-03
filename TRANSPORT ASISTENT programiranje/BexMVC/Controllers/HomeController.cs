using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common;
using Bex.Common.Interfaces;
using Bex.DAL.EF.UOW;
using Bex.Models;
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
    public class HomeController : Controller
    {

        public HomeController() : this(new BexUow(), new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new ExceptionSolver())
        { }
        public HomeController(
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
            
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var userId = User.Identity.GetUserId();
            KorisniciPrograma korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == userId);

            if(korisnikPrograma is null)
            {
                return View();
            }
            else
            {
                string roleId = korisnikPrograma.RoleId;

                if (roleId.Equals("2"))//vozni park
                {
                    return RedirectToAction("Index", "HomeVozniPark");
                }
            }
            


            return View();
        }

        public ActionResult IndexVozniPark()
        {

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Account");
            }

            return View();
        }

        public ActionResult Error()
        {
            //if (!Request.IsAuthenticated)
            //{
            //    return RedirectToAction("LogOn", "Account");
            //}

            return View();
        }



        [ClaimsAuthentication(Resource = "Home", Operation = "EvidentiranePosiljke, All")]
        /* ne mogu dve uloge u ovom slucaju */
        [ClaimsAuthentication(Resource = "Home", Operation = "ProsecnaCenaPosiljke, All")]
        [HttpGet]
        public ActionResult GetEvidentiraneChart(string dateForChart)
        {
            //2019-01-10 
            DateTime datumStatFirst;
            if (dateForChart != "")
            {
                string[] arrDatumFirst = dateForChart.Trim().Split('-');
                datumStatFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

            }
            else
            {
                datumStatFirst = DateTime.Today;
            }



            DateTime datumJuce = datumStatFirst.AddDays(-Convert.ToDouble(1));
            var posiljkePetDanaData = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuce).FirstOrDefault().EvidentiranoPre5danaPosiljki;
                //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumStatFirst && x.DatumEvidentiranja > datumPetDana && x.StatusId == 1).Count();
            var paketiPetDanaData = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuce).FirstOrDefault().EvidentiranoPre5danaPaketa;
            //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumStatFirst && x.DatumEvidentiranja > datumPetDana && x.StatusId == 1).Sum(x=>x.UkupnoPaketa);


            List<PosiljkePoSatuUnionChartArea> posiljkeList = new List<PosiljkePoSatuUnionChartArea>();
            posiljkeList.Add(new PosiljkePoSatuUnionChartArea
            {
                PeriodStatus = 1,
                PeriodVreme = -1,
                PeriodDatum = datumStatFirst,
                CountPosiljke = posiljkePetDanaData,
                CountPaketa = paketiPetDanaData,
                CenaPoPosiljci = 0,
                CenaPoPaketu = 0,
                OdnosCenaPosPaket = 0
            });


            var lista = (from t in BexUow.StatistikaSat.AllAsNoTracking
                         where t.Datum == datumStatFirst
                         select new PosiljkePoSatuUnionChartArea
                         {
                             PeriodStatus = 1,
                             PeriodVreme = t.Sat,
                             PeriodDatum = t.Datum,
                             CountPosiljke = t.EvidentiranoPosiljki,
                             CountPaketa = t.EvidentiranoPaketa,
                             CenaPoPosiljci = t.ProsecnaCenaPoPosiljci,
                             CenaPoPaketu = t.ProsecnaCenaPoPaketu,
                             OdnosCenaPosPaket = t.PaketaPoPosiljci
                         });

            posiljkeList.AddRange(lista);

            DateTime datumPocetniPrveNedelje = datumStatFirst.AddDays(-Convert.ToDouble(7));
            DateTime datumJucePrveNedelje = datumPocetniPrveNedelje.AddDays(-Convert.ToDouble(1));

            //int? brojPosiljkiPrveNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumPocetniPrveNedelje)?.EvidentiranoPre5danaPosiljki;
            //int? brojPaketaPrveNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumPocetniPrveNedelje)?.EvidentiranoPre5danaPaketa;
            int? brojPosiljkiPrveNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumJucePrveNedelje)?.EvidentiranoPre5danaPosiljki;
            int? brojPaketaPrveNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumJucePrveNedelje)?.EvidentiranoPre5danaPaketa;
            if (brojPosiljkiPrveNedeljePetDana == null)
            {
                brojPosiljkiPrveNedeljePetDana= BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJucePrveNedelje).FirstOrDefault().EvidentiranoPre5danaPosiljki;
                //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumPocetniPrveNedelje && x.DatumEvidentiranja > datumPetDanaPrveNedelje && x.StatusId == 1).Count();
                brojPaketaPrveNedeljePetDana = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJucePrveNedelje).FirstOrDefault().EvidentiranoPre5danaPaketa;
                //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumPocetniPrveNedelje && x.DatumEvidentiranja > datumPetDanaPrveNedelje && x.StatusId == 1).Sum(x=>x.UkupnoPaketa);
            }

            posiljkeList.Add(new PosiljkePoSatuUnionChartArea
            {
                PeriodStatus = 2,
                PeriodVreme = -1,
                PeriodDatum = datumPocetniPrveNedelje,
                CountPosiljke = brojPosiljkiPrveNedeljePetDana,
                CountPaketa = brojPaketaPrveNedeljePetDana,
                CenaPoPosiljci = 0,
                CenaPoPaketu = 0,
                OdnosCenaPosPaket = 0
            });

            
            var lista1 = (from t in BexUow.StatistikaSat.AllAsNoTracking
                          where t.Datum == datumPocetniPrveNedelje

                          select new PosiljkePoSatuUnionChartArea
                          {
                              PeriodStatus = 2,
                              PeriodVreme = t.Sat,
                              PeriodDatum = t.Datum,
                              CountPosiljke = t.EvidentiranoPosiljki,
                              CountPaketa = t.EvidentiranoPaketa,
                              CenaPoPosiljci = t.ProsecnaCenaPoPosiljci,
                              CenaPoPaketu = t.ProsecnaCenaPoPaketu,
                              OdnosCenaPosPaket = t.PaketaPoPosiljci
                          });

            posiljkeList.AddRange(lista1);

            DateTime datumPocetniDrugeNedelje = datumPocetniPrveNedelje.AddDays(-Convert.ToDouble(7));
            DateTime datumJuceDrugeNedelje = datumPocetniDrugeNedelje.AddDays(-Convert.ToDouble(1));

            //int? brojPosiljkiDrugeNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumPocetniDrugeNedelje)?.EvidentiranoPre5danaPosiljki;
            //int? brojPaketaDrugeNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumPocetniDrugeNedelje)?.EvidentiranoPre5danaPaketa;
            int? brojPosiljkiDrugeNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumJuceDrugeNedelje)?.EvidentiranoPre5danaPosiljki;
            int? brojPaketaDrugeNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumJuceDrugeNedelje)?.EvidentiranoPre5danaPaketa;
            if (brojPosiljkiDrugeNedeljePetDana == null)
            {
                brojPosiljkiDrugeNedeljePetDana = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuceDrugeNedelje).FirstOrDefault().EvidentiranoPre5danaPosiljki;
                //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumPocetniDrugeNedelje && x.DatumEvidentiranja > datumPetDanaDrugeNedelje && x.StatusId == 1).Count();
                brojPaketaDrugeNedeljePetDana = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuceDrugeNedelje).FirstOrDefault().EvidentiranoPre5danaPaketa;
                //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumPocetniDrugeNedelje && x.DatumEvidentiranja > datumPetDanaDrugeNedelje && x.StatusId == 1).Sum(x=>x.UkupnoPaketa);
            }


            //var posiljkePetDanaDrugeNedeljeData = BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumPocetniDrugeNedelje && x.DatumEvidentiranja > datumPetDanaDrugeNedelje && x.StatusId == 1).ToList().Count();

            posiljkeList.Add(new PosiljkePoSatuUnionChartArea
            {
                PeriodStatus = 3,
                PeriodVreme = -1,
                PeriodDatum = datumPocetniDrugeNedelje,
                CountPosiljke= brojPosiljkiDrugeNedeljePetDana,
                CountPaketa = brojPaketaDrugeNedeljePetDana,
                CenaPoPosiljci = 0,
                CenaPoPaketu = 0,
                OdnosCenaPosPaket = 0
            });

            var lista2 =
                    (from t in BexUow.StatistikaSat.AllAsNoTracking
                     where t.Datum == datumPocetniDrugeNedelje
                     select new PosiljkePoSatuUnionChartArea
                     {
                         PeriodStatus = 3,
                         PeriodVreme = t.Sat,
                         PeriodDatum = t.Datum,
                         CountPosiljke = t.EvidentiranoPosiljki,
                         CountPaketa = t.EvidentiranoPaketa,
                         CenaPoPosiljci = t.ProsecnaCenaPoPosiljci,
                         CenaPoPaketu = t.ProsecnaCenaPoPaketu,
                         OdnosCenaPosPaket = t.PaketaPoPosiljci
                     });

            posiljkeList.AddRange(lista2);

            DateTime datumPocetniTreceNedelje = datumPocetniDrugeNedelje.AddDays(-Convert.ToDouble(7));
            DateTime datumJuceTreceNedelje = datumPocetniTreceNedelje.AddDays(-Convert.ToDouble(1));

            //int? brojPosiljkiTreceNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumPocetniTreceNedelje)?.EvidentiranoPre5danaPosiljki;
            //int? brojPaketaTreceNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumPocetniTreceNedelje)?.EvidentiranoPre5danaPaketa;
            int? brojPosiljkiTreceNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumJuceTreceNedelje)?.EvidentiranoPre5danaPosiljki;
            int? brojPaketaTreceNedeljePetDana = BexUow.StatistikaDan.Find(x => x.Datum == datumJuceTreceNedelje)?.EvidentiranoPre5danaPaketa;
            if (brojPosiljkiTreceNedeljePetDana == null)
            {
                brojPosiljkiTreceNedeljePetDana = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuceTreceNedelje).FirstOrDefault().EvidentiranoPre5danaPosiljki;
                //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumPocetniTreceNedelje && x.DatumEvidentiranja > datumPetDanaTreceNedelje && x.StatusId == 1).Count();
                brojPaketaTreceNedeljePetDana = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuceTreceNedelje).FirstOrDefault().EvidentiranoPre5danaPaketa;
                //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumPocetniTreceNedelje && x.DatumEvidentiranja > datumPetDanaTreceNedelje && x.StatusId == 1).Sum(x=>x.UkupnoPaketa);
            }

            
            posiljkeList.Add(new PosiljkePoSatuUnionChartArea
            {
                PeriodStatus = 4,
                PeriodVreme = -1,
                PeriodDatum = datumPocetniTreceNedelje,
                CountPosiljke=brojPosiljkiTreceNedeljePetDana,
                CountPaketa = brojPaketaTreceNedeljePetDana,
                CenaPoPosiljci = 0,
                CenaPoPaketu = 0,
                OdnosCenaPosPaket = 0
            });

            
            var lista3 =
                    (from t in BexUow.StatistikaSat.AllAsNoTracking
                     where t.Datum == datumPocetniTreceNedelje
                     select new PosiljkePoSatuUnionChartArea
                     {
                         PeriodStatus = 4,
                         PeriodVreme = t.Sat,
                         PeriodDatum = t.Datum,
                         CountPosiljke = t.EvidentiranoPosiljki,
                         CountPaketa = t.EvidentiranoPaketa,
                         CenaPoPosiljci = t.ProsecnaCenaPoPosiljci,
                         CenaPoPaketu = t.ProsecnaCenaPoPaketu,
                         OdnosCenaPosPaket = t.PaketaPoPosiljci
                     });

            posiljkeList.AddRange(lista3);

            List<PosiljkePoSatuUnionChartArea> posiljka = new List<PosiljkePoSatuUnionChartArea>();
            posiljkeList = posiljkeList.OrderBy(a => a.PeriodVreme).ThenBy(b => b.PeriodStatus).ToList();
            

            int currentSat = -1;
            DateTime? currentDate = datumStatFirst.Date;
            int? pocetnaNedelja = 0;
            int? pocetnaNedeljaPaketa = 0;
            int? prvaNedelja = 0; 
            int? pocetnaNedeljaPaketa1 = 0;
            int? drugaNedelja = 0;
            int? pocetnaNedeljaPaketa2 = 0;
            int? trecaNedelja = 0;
            int? pocetnaNedeljaPaketa3 = 0;

            decimal? cenaPoPosiljci = 0;
            decimal? cenaPoPosiljci1 = 0;
            decimal? cenaPoPosiljci2 = 0;
            decimal? cenaPoPosiljci3 = 0;

            decimal? cenaPoPaketu = 0;
            decimal? cenaPoPaketu1 = 0;
            decimal? cenaPoPaketu2 = 0;
            decimal? cenaPoPaketu3 = 0;

            decimal? paketaPoPosiljci = 0;
            decimal? paketaPoPosiljci1 = 0;
            decimal? paketaPoPosiljci2 = 0;
            decimal? paketaPoPosiljci3 = 0;
            List<PosiljkeEvidentiraneChartArea> dataForChart = new List<PosiljkeEvidentiraneChartArea>();

            foreach (var red in posiljkeList)
            {
                int sati = red.PeriodVreme;
                int? statusId = red.PeriodStatus;
                int? ukupnoPosiljki = red.CountPosiljke;
                int? ukupnoPaketa = red.CountPaketa;
                decimal? cenaPoPosiljciSat = red.CenaPoPosiljci;
                decimal? cenaPoPaketuSat = red.CenaPoPaketu;
                decimal? odnosCena = red.OdnosCenaPosPaket;
                DateTime? datum = red.PeriodDatum;

                
                    if (currentSat != sati)
                    {
                    //if (currentSat != -1)
                    //{
                    var chart = new PosiljkeEvidentiraneChartArea()
                    {
                            period = currentSat.ToString(),
                            evidentiraneUkupno = pocetnaNedelja,
                            evidentiranoPaketa = pocetnaNedeljaPaketa,
                            odnosPaketPosiljka = paketaPoPosiljci,
                            //(pocetnaNedeljaPaketa == 0 ? 0 : Math.Round(Decimal.Divide(System.Convert.ToDecimal(pocetnaNedeljaPaketa), System.Convert.ToDecimal(pocetnaNedelja)) , 2)),
                            evidentiraneUkupno1Nedelja = prvaNedelja,
                            evidentiranoPaketa1 = pocetnaNedeljaPaketa1,
                            odnosPaketPosiljka1 = paketaPoPosiljci1,
                            //(pocetnaNedeljaPaketa1 == 0 ? 0 : Math.Round(Decimal.Divide(System.Convert.ToDecimal(pocetnaNedeljaPaketa1), System.Convert.ToDecimal(prvaNedelja)) , 2)),
                            evidentiraneUkupno2Nedelja = drugaNedelja,
                            evidentiranoPaketa2 = pocetnaNedeljaPaketa2,
                            odnosPaketPosiljka2 = paketaPoPosiljci2,
                            //(pocetnaNedeljaPaketa2 == 0 ? 0 : Math.Round(Decimal.Divide(System.Convert.ToDecimal(pocetnaNedeljaPaketa2), System.Convert.ToDecimal(drugaNedelja)) , 2)),
                            evidentiraneUkupno3Nedelja = trecaNedelja,
                            evidentiranoPaketa3 = pocetnaNedeljaPaketa3,
                            odnosPaketPosiljka3 = paketaPoPosiljci3,
                            //(pocetnaNedeljaPaketa3 == 0 ? 0 : Math.Round(Decimal.Divide(System.Convert.ToDecimal(pocetnaNedeljaPaketa3), System.Convert.ToDecimal(trecaNedelja)) , 2)),
                            cenaPoPosiljci = cenaPoPosiljci,
                            cenaPoPaketu = cenaPoPaketu,
                            cenaPoPosiljci1 = cenaPoPosiljci1,
                            cenaPoPaketu1 = cenaPoPaketu1,
                            cenaPoPosiljci2 = cenaPoPosiljci2,
                            cenaPoPaketu2 = cenaPoPaketu2,
                            cenaPoPosiljci3 = cenaPoPosiljci3,
                            cenaPoPaketu3 = cenaPoPaketu3
                        };

                            dataForChart.Add(chart);
                        
                        currentSat = sati;

                    }
                    if (currentSat == sati)
                    {
                        if (statusId == 1)
                        {
                            pocetnaNedelja += ukupnoPosiljki;
                            pocetnaNedeljaPaketa += ukupnoPaketa;
                            cenaPoPosiljci = cenaPoPosiljciSat;
                            cenaPoPaketu = cenaPoPaketuSat;
                            paketaPoPosiljci = odnosCena;
                        }
                        if (statusId == 2)
                        {
                            prvaNedelja += ukupnoPosiljki;
                            pocetnaNedeljaPaketa1 += ukupnoPaketa;
                            cenaPoPosiljci1 = cenaPoPosiljciSat;
                            cenaPoPaketu1 = cenaPoPaketuSat;
                            paketaPoPosiljci1 = odnosCena;
                        }
                        if (statusId == 3)
                        {
                            drugaNedelja += ukupnoPosiljki;
                            pocetnaNedeljaPaketa2 += ukupnoPaketa;
                            cenaPoPosiljci2 = cenaPoPosiljciSat;
                            cenaPoPaketu2 = cenaPoPaketuSat;
                            paketaPoPosiljci2 = odnosCena;
                        }
                        if (statusId == 4)
                        {
                            trecaNedelja += ukupnoPosiljki;
                            pocetnaNedeljaPaketa3 += ukupnoPaketa;
                            cenaPoPosiljci3 = cenaPoPosiljciSat;
                            cenaPoPaketu3 = cenaPoPaketuSat;
                            paketaPoPosiljci3 = odnosCena;
                        }
                    }

                
                
            }


            return Json(dataForChart, "PosiljkeEvidentiraneChartArea", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ClaimsAuthentication(Resource = "Home", Operation = "Poreklo, All")]
        public ActionResult GetEvidentiranePorekloChart(string dateForChart)
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


            var posiljkeDataP = (from t in BexUow.StatistikaPorekloPosiljke.AllAsNoTracking

                                 select new PosiljkePoSatuUnionChartArea
                                 {
                                     PeriodStatus = t.Poreklo,
                                     PeriodVreme = t.Sat,
                                     PeriodDatum = t.Datum,
                                     CountPosiljke = t.UkupnoPosiljki
                                 });

           

            List<PosiljkePoSatuUnionChartArea> posiljka = new List<PosiljkePoSatuUnionChartArea>();
            if (datumStatFirst.Equals(datumStatSecond))
            {
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum == datumStatFirst);
                posiljka = posiljkeDataP.OrderBy(a => a.PeriodVreme).ThenBy(b => b.PeriodStatus).ToList();
            }
            else
            {
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum >= datumStatFirst && x.PeriodDatum <= datumStatSecond);
                posiljka = posiljkeDataP.OrderBy(a => a.PeriodDatum).ThenBy(b => b.PeriodStatus).ToList();
            }

            int currentSat = 0;
            DateTime? currentDate = datumStatFirst.Date;
            int? callcenter = 0; // program BZ 
            int? klijentski = 0; // program BK 
            int? integracija = 0; //integracije xml, excel, json
            int? web = 0; // shipping
            int br = 0;
            List<PosiljkeEvidentiranePorekloChartArea> dataForChart = new List<PosiljkeEvidentiranePorekloChartArea>();

            foreach (var red in posiljka)
            {
                int sati = red.PeriodVreme;
                int? statusId = red.PeriodStatus;
                int? ukupnoPosiljki = red.CountPosiljke;
                DateTime? datum = red.PeriodDatum;
                br++;

                if (datumStatFirst.Equals(datumStatSecond))
                {
                    if (currentSat != sati)
                    {
                        if (currentSat != 0)
                        {
                            var chart = new PosiljkeEvidentiranePorekloChartArea()
                            {
                                period = currentSat.ToString(),
                                callcenter = callcenter,
                                klijentski = klijentski,
                                integracija = integracija,
                                web = web
                            };

                            dataForChart.Add(chart);
                        }

                        callcenter = 0;
                        klijentski = 0;
                        integracija = 0;
                        web = 0;
                        currentSat = sati;

                    }
                    if (currentSat == sati)
                    {
                        if (statusId == 1) callcenter += ukupnoPosiljki;
                        if (statusId == 2) klijentski += ukupnoPosiljki;
                        if (statusId == 3) integracija += ukupnoPosiljki;
                        if (statusId == 4) web += ukupnoPosiljki;
                        //if (statusId == 1) callcenter = ukupnoPosiljki;
                        //if (statusId == 2) klijentski = ukupnoPosiljki;
                        //if ((statusId == 3) || (statusId == 21) || (statusId == 22)) integracija += ukupnoPosiljki;
                        //if ((statusId == 31) || (statusId == 32)) web += ukupnoPosiljki;
                    }

                }
                else
                {
                    if (currentDate != datum || posiljka.Count() == br)
                    {
                        var chart = new PosiljkeEvidentiranePorekloChartArea()
                        {
                            period = currentDate.ToString(),
                            callcenter = callcenter,
                            klijentski = klijentski,
                            integracija = integracija,
                            web = web
                        };

                        dataForChart.Add(chart);

                        callcenter = 0;
                        klijentski = 0;
                        integracija = 0;
                        web = 0;
                        currentDate = datum;

                    }

                    if (currentDate == datum)
                    {
                        if (statusId == 1) callcenter += ukupnoPosiljki;
                        if (statusId == 2) klijentski += ukupnoPosiljki;
                        if (statusId == 3) integracija += ukupnoPosiljki;
                        if (statusId == 4) web += ukupnoPosiljki;
                        //if (statusId == 1) callcenter += ukupnoPosiljki;
                        //if (statusId == 2) klijentski += ukupnoPosiljki;
                        //if ((statusId == 3) || (statusId == 21) || (statusId == 22)) integracija += ukupnoPosiljki;
                        //if ((statusId == 31) || (statusId == 32)) web += ukupnoPosiljki;
                    }
                }
            }


            return Json(dataForChart, "PosiljkeEvidentiranePorekloChartArea", JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthentication(Resource = "Home", Operation = "Poreklo, All")]
        [HttpGet]
        public ActionResult GetEvidentiranePorekloDonutChart(string dateForChart)
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

            DateTime datumStat = DateTime.Today.Date;
            var posiljkeDataP = (from t in BexUow.Posiljke.AllAsNoTracking
                                 group t by new { Col1 = (t.NacinUnosaId != 1 && t.NacinUnosaId != 2) ?
                                                        ((t.NacinUnosaId == 3 || t.NacinUnosaId == 21 || t.NacinUnosaId == 22) ? 3 : 31) : t.NacinUnosaId
                                                , Col2 = t.DatumEvidentiranja }
                                into grp
                                 select new PosiljkePoSatuUnionChartArea
                                 {
                                    PeriodStatus = grp.Key.Col1,
                                    PeriodDatum = grp.Key.Col2,
                                    CountPosiljke = grp.Count()
                                });

            List<PosiljkePoSatuUnionChartArea> posiljka = new List<PosiljkePoSatuUnionChartArea>();
            if (datumStatFirst.Equals(datumStatSecond)) 
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum == datumStatFirst);
            else
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum >= datumStatFirst && x.PeriodDatum <= datumStatSecond);

            posiljka = posiljkeDataP.OrderBy(b => b.PeriodStatus).ToList();

            List<PosiljkePoStatusuChartDonut> dataForChart = new List<PosiljkePoStatusuChartDonut>();
            int? curentPorekloId = 1;
            int? ukupnoPosiljkiPorekla = 0;
            var posiljkeCount = posiljka.Count();
            for(int i=0; i <= posiljkeCount-1; i++)
            {
                
                int? porekloId = posiljka[i].PeriodStatus;
                int? ukupnoPosiljki = posiljka[i].CountPosiljke;
                DateTime? datum = posiljka[i].PeriodDatum;
                string labelStatus = "";

                if (datumStatFirst.Equals(datumStatSecond))
                {
                    if (porekloId == 1) labelStatus = "Call center";
                    else if (porekloId == 2) labelStatus = "Klijentski";
                    else if (porekloId == 3) labelStatus = "Integracija";
                    else labelStatus = "Web";


                    var chart = new PosiljkePoStatusuChartDonut
                    {
                        label = labelStatus,
                        value = ukupnoPosiljki
                    };

                    dataForChart.Add(chart);
                }
                else
                {
                    if (curentPorekloId != porekloId)
                    {
                       
                        if (curentPorekloId == 1) labelStatus = "Call center";
                        else if (curentPorekloId == 2) labelStatus = "Klijentski";
                        else if (curentPorekloId == 3) labelStatus = "Integracija";
                        else labelStatus = "Web";


                        var chart = new PosiljkePoStatusuChartDonut
                        {
                            label = labelStatus,
                            value = ukupnoPosiljkiPorekla
                        };

                        dataForChart.Add(chart);

                        ukupnoPosiljkiPorekla = 0;
                        curentPorekloId = porekloId;

                    }

                    if (curentPorekloId == porekloId)
                    {
                        ukupnoPosiljkiPorekla += ukupnoPosiljki;
                        if(i == (posiljkeCount-1))
                        {
                            if (curentPorekloId == 1) labelStatus = "Call center";
                            else if (curentPorekloId == 2) labelStatus = "Klijentski";
                            else if (curentPorekloId == 3) labelStatus = "Integracija";
                            else labelStatus = "Web";


                            var chart = new PosiljkePoStatusuChartDonut
                            {
                                label = labelStatus,
                                value = ukupnoPosiljkiPorekla
                            };

                            dataForChart.Add(chart);
                        }
                    }
                }

            }

            return Json(dataForChart, "PosiljkePoStatusuChartDonut", JsonRequestBehavior.AllowGet);
        }

        

        [HttpGet]
        public ActionResult GetEvidentiranePosiljkeCount()
        {
           

            DateTime datumStat = DateTime.Today.Date;
            var posiljkeData = BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja == datumStat ).ToList().Count(); //&& x.Storno == false
            var paketiData = BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja == datumStat ).Sum(x => x.UkupnoPaketa); //&& x.Storno == false

            DateTime datumJuce = datumStat.AddDays(-Convert.ToDouble(1));
            var posiljkePetDanaData = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuce).FirstOrDefault().EvidentiranoPre5danaPosiljki;
            //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumStat && x.DatumEvidentiranja > datumPetDana && x.StatusId == 1).ToList().Count();
            var paketiPetDanaData = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuce).FirstOrDefault().EvidentiranoPre5danaPaketa;
            //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumStat && x.DatumEvidentiranja > datumPetDana && x.StatusId == 1).Sum(x => x.UkupnoPaketa);

            var ukupnoPosiljke = posiljkeData + posiljkePetDanaData;
            var ukupnoPaketi = paketiData + paketiPetDanaData;

            //String returnTxt = ("Prijavljeno danas: " + posiljkeData + "posiljki/" + paketiData + "paketa" + System.Environment.NewLine
            //                + "Prijavljeno prethodnih 5 dana: " + posiljkePetDanaData + "posiljki/" + paketiPetDanaData + "paketa" + System.Environment.NewLine
            //                + "Ukupno: " + ukupnoPosiljke + "posiljki/" + ukupnoPaketi + "paketa").Replace(Environment.NewLine, "<br />");

            var evidentiranePosiljkeData = new 
            {
                prijavljenoPosiljkiDanas = posiljkeData,
                prijavljenoPaketaDanas = paketiData,
                prijavljenoPosiljkePetDana = posiljkePetDanaData,
                prijavljenoPaketiPetDana = paketiPetDanaData,
                ukupnoPosiljke,
                ukupnoPaketi 
            };

            return Json(evidentiranePosiljkeData, "", JsonRequestBehavior.AllowGet);
            //return returnTxt;
        }

        // ovo se moze zameniti sa var claims = AuthenticationManager.User.Claims;
        public async Task<List<string>> getAllClaims()
        {
            List<string> listaRegiona = new List<string>();
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;

            foreach (var c in applicationUser.Claims)
            {
                if (c.ClaimType.Split('/').Last().Equals("Region"))
                {
                    if (c.ClaimValue.Equals("All"))
                    {
                        listaRegiona = BexUow.Region.AllAsNoTracking.Select(r => r.Id).ToList().ConvertAll<string>(delegate (int i) { return i.ToString(); });
                    }
                    else
                    {
                        listaRegiona.Add(c.ClaimValue);
                    }
                }

            }

            return listaRegiona;
        }

        [HttpGet]
        public async Task<ActionResult> GetPreuzetiZadaciData(int? id)
        {
            List<string> listaRegiona = await getAllClaims();
            string regionId = listaRegiona.Last();

            //var posiljkePreuzimanjaDanasObavljene = BexUow.StatistikaSat.AllAsNoTracking.Where(x => x.Datum == DateTime.Today.Date).Sum(x => x.PreuzetoPosiljki);

            int posiljkePreuzimanjaDanasObavljene = (
                                         from z in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(x => x.Tip == "P")
                                                                               .Where(x => x.Status == 1 && System.Data.Entity.DbFunctions.TruncateTime(x.DatumIzvrsenja) == DateTime.Today.Date)
                                         from r in BexUow.Reon.AllAsNoTracking.Where(r => r.Id == z.ReonId).DefaultIfEmpty()
                                         from reg in BexUow.Region.AllAsNoTracking.Where(reg => reg.Id == r.RegionId).DefaultIfEmpty()
                                         select new
                                         {
                                             PosiljkaId = z.PosiljkaId,
                                             RegionId = reg.Id
                                         }).Where(x => listaRegiona.Contains(x.RegionId.ToString())).Count();

            int paketiPreuzimanjaDanasObavljene = (
                                         from z in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(x => x.Tip == "P")
                                                                               .Where(x => x.Status == 1 && System.Data.Entity.DbFunctions.TruncateTime(x.DatumIzvrsenja) == DateTime.Today.Date)
                                         from p in BexUow.Posiljke.AllAsNoTracking.Where(p => p.Id == z.PosiljkaId)
                                         from r in BexUow.Reon.AllAsNoTracking.Where(r => r.Id == z.ReonId).DefaultIfEmpty()
                                         from reg in BexUow.Region.AllAsNoTracking.Where(reg => reg.Id == r.RegionId).DefaultIfEmpty()
                                         select new
                                         {
                                             UkupnoPaketa = p.UkupnoPaketa,
                                             RegionId = reg.Id
                                         }).Where(x => listaRegiona.Contains(x.RegionId.ToString())).Sum(x => x.UkupnoPaketa);

            /* postoji vec kao deo procedure GetEvidentiranePosiljkeCount */
            DateTime datumStat = DateTime.Today.Date;
            var posiljkeData = BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja == datumStat).ToList().Count(); //&& x.Storno == false
            var paketiData = BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja == datumStat).Sum(x => x.UkupnoPaketa); //&& x.Storno == false

            DateTime datumJuce = datumStat.AddDays(-Convert.ToDouble(1));
            var posiljkePetDanaData = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuce).FirstOrDefault().EvidentiranoPre5danaPosiljki;
            //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumStat && x.DatumEvidentiranja > datumPetDana && x.StatusId == 1).ToList().Count();
            var paketiPetDanaData = BexUow.StatistikaDan.AllAsNoTracking.Where(x => x.Datum == datumJuce).FirstOrDefault().EvidentiranoPre5danaPaketa;
            //BexUow.Posiljke.AllAsNoTracking.Where(x => x.DatumEvidentiranja < datumStat && x.DatumEvidentiranja > datumPetDana && x.StatusId == 1).Sum(x => x.UkupnoPaketa);

            var ukupnoPosiljke = posiljkeData + posiljkePetDanaData;
            var ukupnoPaketi = paketiData + paketiPetDanaData;

            decimal procPosiljke = Math.Round(Decimal.Divide(posiljkePreuzimanjaDanasObavljene, ukupnoPosiljke) * 100, 2);
            decimal procPaketi = Math.Round(Decimal.Divide(paketiPreuzimanjaDanasObavljene, ukupnoPaketi) * 100, 2);


            decimal? ukupnaCenaPosiljkeOsnovneUsluge = (
                             from z in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(x => x.Tip == "P")
                                                                   .Where(x => x.Status == 1 && System.Data.Entity.DbFunctions.TruncateTime(x.DatumIzvrsenja) == DateTime.Today.Date)
                             from p in BexUow.PosiljkaUsluga.AllAsNoTracking.Where(p => p.PosiljkaId == z.PosiljkaId && p.TipUslugeId == 11)
                             from r in BexUow.Reon.AllAsNoTracking.Where(r => r.Id == z.ReonId).DefaultIfEmpty()
                             from reg in BexUow.Region.AllAsNoTracking.Where(reg => reg.Id == r.RegionId).DefaultIfEmpty()
                             select new
                             {
                                 CenaOsnovneUsluge = p.CenaUsluge,
                                 RegionId = reg.Id
                             }).Where(x => listaRegiona.Contains(x.RegionId.ToString())).Sum(x => x.CenaOsnovneUsluge);

            var cenaPosiljkeOsnovneUsluge = Math.Round(Convert.ToDouble(ukupnaCenaPosiljkeOsnovneUsluge / posiljkePreuzimanjaDanasObavljene), 2);
            var cenaPaketaOsnovneUsluge = Math.Round(Convert.ToDouble(ukupnaCenaPosiljkeOsnovneUsluge / paketiPreuzimanjaDanasObavljene), 2);


            decimal? ukupnaCenaPosiljkeUsluge = (
                             from z in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(x => x.Tip == "P")
                                                                   .Where(x => x.Status == 1 && System.Data.Entity.DbFunctions.TruncateTime(x.DatumIzvrsenja) == DateTime.Today.Date)
                             from p in BexUow.Posiljke.AllAsNoTracking.Where(p => p.Id == z.PosiljkaId)
                             from r in BexUow.Reon.AllAsNoTracking.Where(r => r.Id == z.ReonId).DefaultIfEmpty()
                             from reg in BexUow.Region.AllAsNoTracking.Where(reg => reg.Id == r.RegionId).DefaultIfEmpty()
                             select new
                             {
                                 CenaUsluge = p.CenaUkupna,
                                 RegionId = reg.Id
                             }).Where(x => listaRegiona.Contains(x.RegionId.ToString())).Sum(x => x.CenaUsluge);


            var cenaPosiljkeUsluge = Math.Round(Convert.ToDouble(ukupnaCenaPosiljkeUsluge / posiljkePreuzimanjaDanasObavljene), 2);
            var cenaPaketaUsluge = Math.Round(Convert.ToDouble(ukupnaCenaPosiljkeUsluge / paketiPreuzimanjaDanasObavljene), 2);

            var preuzetePosiljkeData = new
            {
                preuzetePosiljkiDanas = posiljkePreuzimanjaDanasObavljene,
                preuzetoPaketaDanas = paketiPreuzimanjaDanasObavljene,
                procPosiljke,
                procPaketi,
                cenaPosiljkeOsnovneUsluge,
                cenaPaketaOsnovneUsluge,
                cenaPosiljkeUsluge,
                cenaPaketaUsluge
            };

            return Json(preuzetePosiljkeData, "", JsonRequestBehavior.AllowGet);

            //String returnTxt = ("Preuzeto danas: " + posiljkePreuzimanjaDanasObavljene + "pos/" + paketiPreuzimanjaDanasObavljene + "paketa" + " " + procPosiljke + "% /" + procPaketi + "%" + System.Environment.NewLine
            //                + "Cena osnovne usluge: " + cenaOsnovneUslugePreuzimanjaDanasObavljene + " / " + cenaPaketaOsnovnaUsluga + System.Environment.NewLine
            //                + "Cena ukupno: " + cenaUslugePreuzimanjaDanasObavljene + " / " + cenaPaketa).Replace(Environment.NewLine, "<br />");

            //return returnTxt;
        }

        [HttpGet]
        public async Task<ActionResult> GetNaplacenoData(int? id)
        {
            List<string> listaRegiona = await getAllClaims();
            //string regionId = listaRegiona.Last();

            var naplacenoOsnovneUsluge = (from s in BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking.Where(x => x.Datum == DateTime.Today.Date && x.Usluga > 0)
                                          from p in BexUow.PosiljkaUsluga.AllAsNoTracking.Where(p => p.PosiljkaId == s.PosiljkaId)
                                          from r in BexUow.Reon.AllAsNoTracking.Where(r => r.Id == s.ReonId).DefaultIfEmpty()
                                          from reg in BexUow.Region.AllAsNoTracking.Where(reg => reg.Id == r.RegionId).DefaultIfEmpty()

                                          select new
                                          {
                                              TipUsluge = p.TipUslugeId,
                                              CenaUsluge = p.CenaUsluge,
                                              Otkup = s.Otkup,
                                              RegionId = reg.Id
                                          }
                              ).Where(x => listaRegiona.Contains(x.RegionId.ToString()));

            var osnovneUsluge = naplacenoOsnovneUsluge.Where(x => x.TipUsluge == 11).Sum(x => x.CenaUsluge);
            var dodatneUsluge = naplacenoOsnovneUsluge.Where(x => x.TipUsluge != 11).Sum(x => x.CenaUsluge);
            var otkupnine = naplacenoOsnovneUsluge.Where(x => x.TipUsluge == 7).Sum(x => x.Otkup);

            var naplacenoData = new
            {
                osnovneUsluge,
                dodatneUsluge,
                otkupnine
            };

            return Json(naplacenoData, "", JsonRequestBehavior.AllowGet);

            //String returnTxt = ("Pazar - osnovne usluge: " + osnovneUsluge + System.Environment.NewLine
            //                + "Pazar - dodatne usluge: " + dodatneUsluge + System.Environment.NewLine
            //                + "Otkupnine ukupno: " + otkupnine).Replace(Environment.NewLine, "<br />");

            //return returnTxt;
        }

        //[HttpGet]
        //public async Task<ActionResult> GetZaposleniData(int? id)
        //{
        //    List<string> listaRegiona = await getAllClaims();

            //var zaposleniDataAll = BexUow.Zaposleni.AllAsNoTracking.Where(x => x.Aktivan == true).Where(x => listaRegiona.Contains(x.RegionId.ToString())).Count();
        //    var zaposleniDataKurir = BexUow.Zaposleni.AllAsNoTracking.Where(x => x.Aktivan == true && x.RadnoMestoId==16).Where(x => listaRegiona.Contains(x.RegionId.ToString())).Count();

        //    var zapocetaSmena = (from s in BexUow.SkenStart.AllAsNoTracking.Where(x => x.Datum==DateTime.Today.Date)
        //                         from k in BexUow.KorisniciPrograma.AllAsNoTracking.Where(k => k.idStari == s.UserId).DefaultIfEmpty()
        //                         from z in BexUow.Zaposleni.AllAsNoTracking.Where(z => z.KontaktId == k.KontaktId && z.Aktivan == true).DefaultIfEmpty()
        //                         select new
        //                        {
        //                             zaposleniId=z.Id,
        //                             radnoMestoId=z.RadnoMestoId,
        //                             RegionId=z.RegionId
        //                         }).Where(x => listaRegiona.Contains(x.RegionId.ToString()));

        //    var zaposleniEvidencija = (from e in BexUow.ZaposleniDan.AllAsNoTracking.Where(x => x.Datum == DateTime.Today.Date)
        //                         from z in BexUow.Zaposleni.AllAsNoTracking.Where(z => z.Id == e.ZaposleniID).DefaultIfEmpty()
        //                         select new
        //                         {
        //                             zaposleniId = e.ZaposleniID,
        //                             RegionId = z.RegionId
        //                         }).Where(x => listaRegiona.Contains(x.RegionId.ToString()));

        //    var kuriraZapoceloSmenu = zapocetaSmena.Where(x => x.radnoMestoId == 16).Distinct().Count();

        //    var evidencijaRada = zaposleniEvidencija.Distinct().Count();
            

        //    var zaposleniData = new
        //    {
        //        zaposleniDataAll,
        //        kuriraZapoceloSmenu,
        //        evidencijaRada,
        //        zaposleniDataKurir
        //    };

        //    return Json(zaposleniData, "", JsonRequestBehavior.AllowGet);

        //}


        [HttpGet]
        [ClaimsAuthentication(Resource = "Home", Operation = "Brisane, All")]
        public ActionResult GetObrisanePorekloChart(string dateForChart)
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

            var posiljkeDataP = (from t in BexUow.StatistikaRazlogBrisanjaPosiljke.AllAsNoTracking

                                 select new PosiljkePoSatuUnionChartArea
                                 {
                                     PeriodStatus = t.RazlogBrisanja,
                                     PeriodVreme = t.Sat,
                                     PeriodDatum = t.Datum,
                                     CountPosiljke = t.UkupnoPosiljki
                                 });

            
            List<PosiljkePoSatuUnionChartArea> posiljka = new List<PosiljkePoSatuUnionChartArea>();
            if (datumStatFirst.Equals(datumStatSecond))
            {
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum == datumStatFirst);
                posiljka = posiljkeDataP.OrderBy(a => a.PeriodVreme).ThenBy(b => b.PeriodStatus).ToList();
            }
            else
            {
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum >= datumStatFirst && x.PeriodDatum <= datumStatSecond);
                posiljka = posiljkeDataP.OrderBy(a => a.PeriodDatum).ThenBy(b => b.PeriodStatus).ToList();
            }

            int currentSat = 0;
            DateTime? currentDate = datumStatFirst.Date;
            int? klijentski = 0; 
            int? shipping = 0; 
            int? magacioner = 0; 
            int? test = 0;
            int? duplirana = 0;
            int? posiljalac = 0;
            int? narucilac = 0;
            int? gabarit = 0;
            List<PosiljkeObrisanePorekloChartArea> dataForChart = new List<PosiljkeObrisanePorekloChartArea>();
            int br = 0;
            foreach (var red in posiljka)
            {
                int sati = red.PeriodVreme;
                int? statusId = red.PeriodStatus;
                int? ukupnoPosiljki = red.CountPosiljke;
                DateTime? datum = red.PeriodDatum;
                br++;
                if (datumStatFirst.Equals(datumStatSecond))
                {
                    if (currentSat != sati)
                    {
                        if (currentSat != 0)
                        {
                            var chart = new PosiljkeObrisanePorekloChartArea()
                            {
                                period = currentSat.ToString(),
                                klijentski = klijentski,
                                shipping = shipping,
                                magacioner = magacioner,
                                test = test,
                                duplirana = duplirana,
                                posiljalac = posiljalac,
                                narucilac = narucilac,
                                gabarit = gabarit
                            };

                            dataForChart.Add(chart);
                        }

                        klijentski = 0;
                        shipping = 0;
                        magacioner = 0;
                        test = 0;
                        duplirana = 0;
                        posiljalac = 0;
                        narucilac = 0;
                        gabarit = 0;
                        currentSat = sati;

                    }
                    if (currentSat == sati)
                    {
                        if (statusId == 1) klijentski += ukupnoPosiljki;
                        else if (statusId == 2) shipping += ukupnoPosiljki;
                        else if (statusId == 3) magacioner += ukupnoPosiljki;
                        else if (statusId == 34) test += ukupnoPosiljki;
                        else if (statusId == 35) duplirana += ukupnoPosiljki;
                        else if (statusId == 36) posiljalac += ukupnoPosiljki;
                        else if (statusId == 37) narucilac += ukupnoPosiljki;
                        else if (statusId == 38) gabarit += ukupnoPosiljki;
                    }

                }
                else
                {
                    if (currentDate != datum || br == posiljka.Count())
                    {
                        var chart = new PosiljkeObrisanePorekloChartArea()
                        {
                            period = currentDate.ToString(),
                            klijentski = klijentski,
                            shipping = shipping,
                            magacioner = magacioner,
                            test = test,
                            duplirana = duplirana,
                            posiljalac = posiljalac,
                            narucilac = narucilac,
                            gabarit = gabarit
                        };

                        dataForChart.Add(chart);

                        klijentski = 0;
                        shipping = 0;
                        magacioner = 0;
                        test = 0;
                        duplirana = 0;
                        posiljalac = 0;
                        narucilac = 0;
                        gabarit = 0;
                        currentDate = datum;

                    }

                    if (currentDate == datum)
                    {
                        if (statusId == 1) klijentski += ukupnoPosiljki;
                        else if (statusId == 2) shipping += ukupnoPosiljki;
                        else if (statusId == 3) magacioner += ukupnoPosiljki;
                        else if (statusId == 34) test += ukupnoPosiljki;
                        else if (statusId == 35) duplirana += ukupnoPosiljki;
                        else if (statusId == 36) posiljalac += ukupnoPosiljki;
                        else if (statusId == 37) narucilac += ukupnoPosiljki;
                        else if (statusId == 38) gabarit += ukupnoPosiljki;

                        
                    }
                }
            }


            return Json(dataForChart, "PosiljkeObrisanePorekloChartArea", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ClaimsAuthentication(Resource = "Home", Operation = "Brisane, All")]
        public ActionResult GetObrisanePorekloDonutChart(string dateForChart)
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

            DateTime datumStat = DateTime.Today.Date;
            var posiljkeDataP = (from t in BexUow.PosiljkaObrisana.AllAsNoTracking
                                 group t by new
                                 {
                                     Col1 = (t.IzKlijentskog == true && t.NapomenaPodTipId == null) ? 1 :
                                                            (t.IzKlijentskog == false && t.NapomenaPodTipId == null) ? 2 :
                                                            (t.IzKlijentskog == false && t.NapomenaPodTipId == 0) ? 3 : t.NapomenaPodTipId,
                                     
                                     Col2 = System.Data.Entity.DbFunctions.TruncateTime(t.DatumBrisanja)
                                 }
                                into grp
                                 select new PosiljkePoSatuUnionChartArea
                                 {
                                     PeriodStatus = grp.Key.Col1,
                                     PeriodDatum = grp.Key.Col2,
                                     CountPosiljke = grp.Count()
                                 });

            List<PosiljkePoSatuUnionChartArea> posiljka = new List<PosiljkePoSatuUnionChartArea>();
            if (datumStatFirst.Equals(datumStatSecond))
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum == datumStatFirst);
            else
                posiljkeDataP = posiljkeDataP.Where(x => x.PeriodDatum >= datumStatFirst && x.PeriodDatum <= datumStatSecond);

            posiljka = posiljkeDataP.OrderBy(b => b.PeriodStatus).ToList();

            List<PosiljkePoStatusuChartDonut> dataForChart = new List<PosiljkePoStatusuChartDonut>();
            int? curentPorekloId = 1;
            int? ukupnoPosiljkiPorekla = 0;
            var posiljkeCount = posiljka.Count();
            for (int i = 0; i <= posiljkeCount - 1; i++)
            {

                int? porekloId = posiljka[i].PeriodStatus;
                int? ukupnoPosiljki = posiljka[i].CountPosiljke;
                DateTime? datum = posiljka[i].PeriodDatum;
                string labelStatus = "";

                if (datumStatFirst.Equals(datumStatSecond))
                {
                    if (porekloId == 1) labelStatus = "Klijentski";
                    else if (porekloId == 2) labelStatus = "Shipping";
                    else if (porekloId == 3) labelStatus = "Magacioner";
                    else if (porekloId == 34) labelStatus = "Test";
                    else if (porekloId == 35) labelStatus = "Duplirana posiljka";
                    else if (porekloId == 36) labelStatus = "Posiljalac odustao";
                    else if (porekloId == 37) labelStatus = "Narucilac odustao";
                    else if (porekloId == 38) labelStatus = "Gabarit";
                   


                    var chart = new PosiljkePoStatusuChartDonut
                    {
                        label = labelStatus,
                        value = ukupnoPosiljki
                    };

                    dataForChart.Add(chart);
                }
                else
                {
                    if (curentPorekloId != porekloId)
                    {

                        if (curentPorekloId == 1) labelStatus = "Klijentski";
                        else if (curentPorekloId == 2) labelStatus = "Shipping";
                        else if (curentPorekloId == 3) labelStatus = "Magacioner";
                        else if (curentPorekloId == 34) labelStatus = "Test";
                        else if (curentPorekloId == 35) labelStatus = "Duplirana posiljka";
                        else if (curentPorekloId == 36) labelStatus = "Posiljalac odustao";
                        else if (curentPorekloId == 37) labelStatus = "Narucilac odustao";
                        else if (curentPorekloId == 38) labelStatus = "Gabarit";


                        var chart = new PosiljkePoStatusuChartDonut
                        {
                            label = labelStatus,
                            value = ukupnoPosiljkiPorekla
                        };

                        dataForChart.Add(chart);

                        ukupnoPosiljkiPorekla = 0;
                        curentPorekloId = porekloId;

                    }

                    if (curentPorekloId == porekloId)
                    {
                        ukupnoPosiljkiPorekla += ukupnoPosiljki;
                        if (i == (posiljkeCount - 1))
                        {
                            if (curentPorekloId == 1) labelStatus = "Klijentski";
                            else if (curentPorekloId == 2) labelStatus = "Shipping";
                            else if (curentPorekloId == 3) labelStatus = "Magacioner";
                            else if (curentPorekloId == 34) labelStatus = "Test";
                            else if (curentPorekloId == 35) labelStatus = "Duplirana posiljka";
                            else if (curentPorekloId == 36) labelStatus = "Posiljalac odustao";
                            else if (curentPorekloId == 37) labelStatus = "Narucilac odustao";
                            else if (curentPorekloId == 38) labelStatus = "Gabarit";


                            var chart = new PosiljkePoStatusuChartDonut
                            {
                                label = labelStatus,
                                value = ukupnoPosiljkiPorekla
                            };

                            dataForChart.Add(chart);
                        }
                    }
                }

            }




            return Json(dataForChart, "PosiljkePoStatusuChartDonut", JsonRequestBehavior.AllowGet);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }

        private ISecurityUow SecurityUow { get; set; }

       

    }
}