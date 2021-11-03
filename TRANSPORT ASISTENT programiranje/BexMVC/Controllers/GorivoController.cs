using System.Linq;
using System.Net;
using System.Web.Mvc;
using Bex.Models;
using Bex.MVC.Exceptions;
using Bex.DAL.EF.UOW;
using Bex.Common;
using BexMVC.ViewModels;
using System;
using PagedList;
using System.Collections.Generic;
using BexMVC.Helpers;
using System.Data.Entity.Core;
using BexMVC.Filters;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

namespace BexMVC.Controllers
{
    [Authorize]
    [ClaimsAuthentication(Resource = "Gorivo", Operation = "Read, All")]
    public class GorivoController : Controller
    {
        public GorivoController() : this(new BexUow(), new ExceptionSolver())
        { }
        public GorivoController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;

        }
        public ActionResult Index()
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();
           

            //ViewBag.Region = new SelectList(BexUow.Region.AllAsNoTracking, "NazivSkraceni", "NazivSkraceni");
            //ViewBag.Pumpa = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "NazivPumpe", "NazivPumpe");
            //ViewBag.Kartica = new SelectList(BexUow.GorivoKartica.AllAsNoTracking, "NazivKartice", "NazivKartice");

            return View();
        }

        [HttpGet]      
        public ActionResult GetGorivo(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {

            var skip = (pageNumber - 1) * pageSize;



            //join v in BexUow.VozniPark.AllAsNoTracking on p.VpId equals v.Id


            //from r in BexUow.GorivoTocenje.AllAsNoTracking.Where(r => r.Storno == false)
            //let putniNalog = (from z in BexUow.PutniNalog.AllAsNoTracking
            //                  where z.Id == r.PutniNalogId
            //                  select z).FirstOrDefault()
            //let vozilo = (from a in BexUow.VozniPark.AllAsNoTracking
            //              where a.Id == putniNalog.VpId
            //              select a).FirstOrDefault()

            //let pumpa = (from d in BexUow.GorivoPumpa.AllAsNoTracking
            //             where d.Id == r.PumpaId
            //             select d).FirstOrDefault()

            //let kartica = (from u in BexUow.GorivoKartica.AllAsNoTracking
            //               where u.Id == r.KarticaId
            //               select u).FirstOrDefault()
            //var gorivoData = (from g in BexUow.GorivoTocenje.AllAsNoTracking.Where(r => r.Storno == false)
            //                  join p in BexUow.PutniNalog?.AllAsNoTracking on g.PutniNalogId equals p.Id into gorivoPutni
            //                  from p in gorivoPutni.DefaultIfEmpty()
            //                  select new GorivoIndexData
            //                                        {
            //                                            Id = g.Id,
            //                                            Datum =g.Datum,
            //                                            Vreme = g.Vreme,
            //                                            Vozilo = p == null ? String.Empty : p.VpId.ToString(),
            //                                            Registracija = p == null ? String.Empty : p.VpId.ToString(),
            //                                            Region = p == null ? String.Empty : p.VpId.ToString(),
            //                                            PropisanaPotrosnja = p == null ? 0 : p.VpId,
            //                                            Pumpa = g.GorivoKartica.GorivoPumpa.NazivPumpe,
            //                                            Kartica = g.GorivoKartica.NazivKartice,
            //                                            Kilometraza = g.Km,
            //                                            Litara = g.Litara,
            //                                            Cena = g.Cena,
            //                                            Iznos = g.Iznos,
            //                                            KmOdPrethodnog = g.Km - g.KmPrethodno,
            //                                            Napomena = g.Napomena ?? "",
            //                                            ProsekOdPrethodnog = g.ProsekOdPrethodnog

            //                                        }).AsEnumerable();


            var gorivoData = BexUow.GorivoTocenje.GetAll(true).Where(x => x.Storno == false).Select(x =>
                                                           new GorivoIndexData
                                                           {
                                                               Id = x.Id,
                                                               Datum = x.Datum,
                                                               Vreme = x.Vreme,
                                                               Vozilo = x.PutniNalog.VozniPark.NazivVozila.ToString(),
                                                               Registracija = x.PutniNalog.VozniPark.Oznaka.ToString(),
                                                               Region = x.PutniNalog.VozniPark.Region.NazivSkraceni.ToString(),
                                                               PropisanaPotrosnja = x.PutniNalog.VozniPark.PropisanaPotrosnja,
                                                               Pumpa = x.GorivoKartica.GorivoPumpa.NazivPumpe.ToString(),
                                                               Kartica = x.GorivoKartica.NazivKartice.ToString(),
                                                               Kilometraza = x.Km,
                                                               Litara = x.Litara,
                                                               Cena = x.Cena,
                                                               Iznos = x.Iznos,
                                                               KmOdPrethodnog = x.Km - x.KmPrethodno,
                                                               Napomena = x.Napomena ?? "",
                                                               ProsekOdPrethodnog = x.ProsekOdPrethodnog
                                                           }).AsEnumerable();





            if (!String.IsNullOrEmpty(searchTerms))
            {
                gorivoData = GetSearchGorivoData(searchTerms, gorivoData).AsEnumerable();
            }

            var total = gorivoData.Count();

            if (sortColumn == "")
            {
                sortColumn = "Datum";
                sortOrder = "desc";
            }

            if (sortOrder.Equals("desc"))
                gorivoData = gorivoData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                gorivoData = gorivoData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Id" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<GorivoIndexData>()
            {
                total = total,
                rows = gorivoData
            };
            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            
        }

        public IEnumerable<GorivoIndexData> GetSearchGorivoData(string searchTerms, IEnumerable<GorivoIndexData> searchDataSet)
        {
            string[] terms = searchTerms.Split(',');

            var gorivo = searchDataSet;



            foreach (string t in terms)
            {
                string searchColumn = "";
                string searchTxt = "";

                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("Datum") && !String.IsNullOrEmpty(searchTxt))
                {
                    //string[] arrDatum = searchTxt.Split('/');
                    //DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    //2019-01-10 to 2019-01-10
                    DateTime datumFirst, datumSecond;
                    string[] arrDatumAll = searchTxt.Split('t', 'o');

                    string[] arrDatumFirst = arrDatumAll[0].Trim().Split('-');
                    datumFirst = new DateTime(int.Parse(arrDatumFirst[0]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[2]));

                    string[] arrDatumSecond = arrDatumAll[2].Trim().Split('-');
                    datumSecond = new DateTime(int.Parse(arrDatumSecond[0]), int.Parse(arrDatumSecond[1]), int.Parse(arrDatumSecond[2]));

                    gorivo = gorivo.Where(k => k.Datum >= datumFirst && k.Datum <= datumSecond);

                }
                else if (searchColumn.Equals("Vreme") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Vreme.Equals(searchTxt));
                }
                else if (searchColumn.Equals("Vozilo") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Vozilo.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Registracija") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Registracija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Region.ToUpper().Equals(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("PropisanaPotrosnja") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.PropisanaPotrosnja.Equals(System.Convert.ToDecimal(searchTxt)));
                }
               
                else if (searchColumn.Equals("Pumpa") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => (String.IsNullOrEmpty(k.Pumpa) ? "": k.Pumpa.ToUpper()).Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Kartica") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => (String.IsNullOrEmpty(k.Kartica) ? "" : k.Kartica.ToUpper()).Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Kilometraza") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Kilometraza.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("Litara") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Litara.Equals(System.Convert.ToDecimal(searchTxt)));
                }
                else if (searchColumn.Equals("Cena") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Cena.Equals(System.Convert.ToDecimal(searchTxt)));
                }
                else if (searchColumn.Equals("Iznos") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Iznos.Equals(System.Convert.ToDecimal(searchTxt)));
                }
                else if (searchColumn.Equals("KmOdPrethodnog") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.KmOdPrethodnog.Equals(System.Convert.ToInt32(searchTxt)));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.Napomena.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("ProsekOdPrethodnog") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivo = gorivo.Where(k => k.ProsekOdPrethodnog.Equals(System.Convert.ToDecimal(searchTxt)));
                }
               

            }
            return gorivo;
        }

        [ClaimsAuthentication(Resource = "Gorivo", Operation = "Create, All")]
        public ActionResult Create(int id)
        {
            
            int kilomtraza = 0;
            var putniNalog = BexUow.PutniNalog.Find(id);
            //var tocenje = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.PutniNalog.VpId == putniNalog.VpId && x.Storno==false).OrderByDescending(z => z.Datum).ThenByDescending(z=> z.Vreme).Take(1).FirstOrDefault();
            var tocenje = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.PutniNalog.VpId == putniNalog.VpId && x.Storno == false).OrderByDescending(z => z.Datum).ThenByDescending(z => z.Vreme).Take(1).FirstOrDefault();
            kilomtraza = (tocenje == null) ? 0 : tocenje.Km;

            GorivoTocenje gorivoData = new GorivoTocenje {
                PutniNalogId = putniNalog.Id,
                Datum = putniNalog.DatumStart,
                Vreme = TimeSpan.FromHours(8),
                KmPrethodno = kilomtraza
            };

            ViewBag.KarticaId = new SelectList(BexUow.GorivoKartica.AllAsNoTracking, "Id", "NazivKartice");

            var vozilo = BexUow.VozniPark.AllAsNoTracking.Where(x => x.Id == putniNalog.VpId).FirstOrDefault();
            ViewBag.Vozilo = vozilo.NazivVozila + ", " + vozilo.Oznaka;
            ViewBag.PropisanaPotrosnja = vozilo.PropisanaPotrosnja;


            return View(gorivoData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GorivoTocenje gorivoTocenje)
        {
           

            if (ModelState.IsValid)
            {
                BexUow.GorivoTocenje.Add(gorivoTocenje);
                var putniNalog = BexUow.PutniNalog.Find(gorivoTocenje.PutniNalogId);
                putniNalog.BrojSipanja = putniNalog.BrojSipanja + 1;
                putniNalog.Litara = putniNalog.Litara + gorivoTocenje.Litara;
                BexUow.PutniNalog.Update(putniNalog);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return Json("Uspešno ste dodali točenje goriva", JsonRequestBehavior.AllowGet);
                    //RedirectToAction("Index");
                }
                //ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }


            return Json("Greška pri dodavanju točenja goriva", JsonRequestBehavior.AllowGet);

        }

        [ClaimsAuthentication(Resource = "Gorivo", Operation = "Edit, All")]
        public ActionResult Edit(int id) 
        { 
            var gorivoData = BexUow.GorivoTocenje.Find(id);

            ViewBag.KarticaId = new SelectList(BexUow.GorivoKartica.AllAsNoTracking, "Id", "NazivKartice", gorivoData.KarticaId);

            var putniNalog = BexUow.PutniNalog.Find(gorivoData.PutniNalogId);
            var vozilo = BexUow.VozniPark.AllAsNoTracking.Where(x => x.Id == putniNalog.VpId).FirstOrDefault();
            ViewBag.Vozilo = vozilo.NazivVozila + ", " + vozilo.Oznaka;
            ViewBag.PropisanaPotrosnja = vozilo.PropisanaPotrosnja;


            return View(gorivoData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GorivoTocenje gorivoTocenje)
        {
            if (ModelState.IsValid)
            {
                BexUow.GorivoTocenje.Update(gorivoTocenje);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { //return RedirectToAction("Index", "Gorivo"); 
                    return Json("Uspešno ste izmenili točenje goriva", JsonRequestBehavior.AllowGet);
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
                
            }

            //ovo ovde je u slucaju greske da ostane na istoj stranici???
            //ViewBag.KarticaId = new SelectList(BexUow.GorivoKartica.AllAsNoTracking, "Id", "NazivKartice", gorivoTocenje.KarticaId);
            return Json("Greška pri izmeni točenja goriva", JsonRequestBehavior.AllowGet);

        }

        [ClaimsAuthentication(Resource = "Gorivo", Operation = "Delete, All")]
        public ActionResult Delete(int id)
        {
            var gorivoData = BexUow.GorivoTocenje.Find(id);
            gorivoData.Storno = true;
            BexUow.GorivoTocenje.Update(gorivoData);
            var commandResult = BexUow.SubmitChanges();

            if (commandResult.IsSuccessful)
            { return Json("Uspešno ste obrisali gorivo", JsonRequestBehavior.AllowGet); }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);
            return Json("Greška pri brisanju goriva", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Import()
        {
            ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelFile, GorivoTocenje gorivoTocenje)
        {
            int brojGresaka = 0;
            int brojUspesno = 0;
            int brojUkupno = 0;
            var datum = gorivoTocenje.Datum;
            var cena = gorivoTocenje.Cena;
            var pumpaId = gorivoTocenje.PumpaId;
            var registracija = "";
            var kolicina = "";
            var km = "";
            string vreme = "";
            bool pm = false;
            if (excelFile==null || excelFile.ContentLength==0)
            {
                ViewBag.ErrorMessage = "Izaberite excel fajl.";
                return View();
            }
            else
            {
                string fileExtension = System.IO.Path.GetExtension(excelFile.FileName);
                if (fileExtension.EndsWith(".xls") || fileExtension.EndsWith(".xlsx"))
                {
                    string path = Server.MapPath("~/Doc/" + excelFile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelFile.SaveAs(path);

                    try
                    {
                        string konecijaZaExcel = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Mode=Read;data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1;ReadOnly=true\";", path);
                        if (path.EndsWith(".xls"))
                        {
                            konecijaZaExcel = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", path);
                        }
                        else if (path.EndsWith(".xlsx"))
                        {
                            konecijaZaExcel = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", path);
                        }


                        using (OleDbConnection konekcijaExcel = new OleDbConnection(konecijaZaExcel))
                        {

                            konekcijaExcel.Open();

                            //******** citanje podataka iz prvog sheeta ************//
                            DataTable dt = konekcijaExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            
                            if (dt == null)
                            {
                                ViewBag.ErrorMessage = "Zatvorite excel fajl koji importujete.";
                                ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
                                return View();
                            }
                            String[] excelSheets = new String[dt.Rows.Count];
                            int a = 0;
                            string pageName = "";

                            foreach (DataRow row in dt.Rows)
                            {
                                excelSheets[a] = row["TABLE_NAME"].ToString();
                                pageName = excelSheets[a];
                                a++;
                            }
                            pageName = excelSheets[0];

                            string sqlUpit = "select * from [" + pageName + "]";

                            OleDbCommand cmd = new OleDbCommand(sqlUpit, konekcijaExcel);
                            OleDbDataReader dr = cmd.ExecuteReader();

                            int brojKolone = dr.FieldCount;
                            if (brojKolone == 4)
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        brojUkupno++;
                                        try
                                        {
                                            for (int i = 0; i <= brojKolone - 1; i++)
                                            {
                                                if (i == 0)//registracija
                                                {
                                                    registracija = dr.GetValue(i).ToString();
                                                    if (registracija=="")
                                                    {
                                                        dr.Close();
                                                        konekcijaExcel.Close();
                                                        return RedirectToAction("Index", "Gorivo");
                                                    }
                                                }
                                                if (i == 1)//vreme
                                                {
                                                    vreme = dr.GetValue(i).ToString();
                                                    if ((vreme.Contains("AM")) || (vreme.Contains("PM")))
                                                    {
                                                        if (vreme.Contains("PM")) pm = true;
                                                        vreme = vreme.Replace("AM", "").TrimEnd();
                                                        vreme = vreme.Replace("PM", "").TrimEnd();
                                                    }
                                                    if (vreme.Length < 19)
                                                    {
                                                        vreme = (vreme.Length == 17) ? "00" + vreme : "0" + vreme;
                                                    }
                                                    vreme = vreme.Substring(11, vreme.Length - 11);
                                                    if (pm) vreme = TimeAmPm(vreme);
                                                    pm = false;
                                                }
                                                if (i == 2)//kolicina
                                                {
                                                    kolicina = dr.GetValue(i).ToString();
                                                }
                                                if (i == 3)//km
                                                {
                                                    km = dr.GetValue(i).ToString();
                                                }
                                            }

                                        }
                                        catch (Exception k)
                                        {
                                            brojGresaka++;
                                            ViewBag.ErrorMessage = ViewBag.ErrorMessage + "\n" + k.Message;
                                        }
                                        BexUow.GorivoTocenje.GorivoImport(registracija, System.Convert.ToDecimal(kolicina), System.Convert.ToInt32(km), System.Convert.ToDecimal(cena), vreme, datum, System.Convert.ToInt32(pumpaId));
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
                                ViewBag.ErrorMessage = "Driver ne može da pronađe 4 kolone. Snimite novi fajl.";
                                return View();
                            }
                            dr.Close();
                            konekcijaExcel.Close();
                            if (brojGresaka>0)
                            {
                                ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
                                ViewBag.ErrorMessage = ViewBag.ErrorMessage + "\n" + "Ukupno redova:" + brojUkupno.ToString() + " Ukupno grešaka:" + brojGresaka.ToString();
                                return View();
                            }
                            else
                            {
                                //ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
                                //ViewBag.Error = "Uspešno ste uvezli fajl.";
                                return RedirectToAction("Index", "Gorivo");
                            }                                
                        }
                                           
                    }
                    catch (OleDbException ex1)
                    {
                        if (ex1.ErrorCode == -2147467259)
                        {
                            ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
                            ViewBag.ErrorMessage = "Excel fajl koji pokušavate da učitate je otvoren u nekom drugom programu. Zatvorite ga pa pokušajte ponovo.";
                            return View();
                        }
                        else
                        {
                            ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
                            ViewBag.ErrorMessage = ex1.ToString();
                            return View();
                        }
                    }
                }
                else
                {
                    ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");
                    ViewBag.ErrorMessage = "Izaberite fajl nije u dobrom formatu.";
                    return View();
                }                
            }          

        }  
        
        private string TimeAmPm(string time)
        {
            string hour = "";
            string minuteSecond = "";
            if (time.Length == 7)
            {
                hour = time.Substring(0, 1);
                minuteSecond = time.Substring(1, 6);
            }
            else
            {
                hour = time.Substring(0, 2);
                minuteSecond = time.Substring(2, 6);
            }      

            switch (hour)
            {
                case "1":
                    time = "13" + minuteSecond;
                    break;
                case "2":
                    time = "14" + minuteSecond;
                    break;
                case "3":
                    time = "15" + minuteSecond;
                    break;
                case "4":
                    time = "16" + minuteSecond;
                    break;
                case "5":
                    time = "17" + minuteSecond;
                    break;
                case "6":
                    time = "18" + minuteSecond;
                    break;
                case "7":
                    time = "19" + minuteSecond;
                    break;
                case "8":
                    time = "20" + minuteSecond;
                    break;
                case "9":
                    time = "21" + minuteSecond;
                    break;
                case "10":
                    time = "22" + minuteSecond;
                    break;
                case "11":
                    time = "23" + minuteSecond;
                    break;
                case "12":
                    time = "00" + minuteSecond;
                    break;
            }
            return time;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { BexUow.Dispose(); }
            base.Dispose(disposing);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
    }
}
