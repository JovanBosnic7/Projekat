using System.Linq;
using System.Net;
using System.Web.Mvc;
using Bex.Models;

using Bex.DAL.EF.UOW;
using Bex.Common;

using System;

using System.Collections.Generic;


using System.Web.UI;
using Bex.Common.Interfaces;
using AspNet.DAL.EF.UOW.Security;
using System.Web;
using AspNet.DAL.EF.Models.Security;

using System.Threading.Tasks;
using DDtrafic.ViewModels;
using DDtrafic.MVC.Exceptions;
using System.Web;
using DDtrafic.Helpers;
using System.Data.Entity.Core;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.OleDb;

namespace DDtrafic.Controllers
{
  
    public class GorivoController : Controller
    {
        public GorivoController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public GorivoController(
            ISecurityUow securityUow, IBexUow bexUow)
        {
            SecurityUow = securityUow;
            BexUow = bexUow;
        }
        public ActionResult Index()
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            return View();

        }

        [HttpGet]
        public async Task<ActionResult> GetGorivo(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);

            var skip = (pageNumber - 1) * pageSize;

            var gorivoData = BexUow.GorivoTocenje.GetAll(true).Where(x => x.PutniNalog.VozniPark.FirmaId==korisnikPrograma.FirmaId)
                                                    .Select(x =>
                                                         new GorivoIndexData
                                                         {
                                                             Id = x.Id,
                                                             Datum=x.Datum,
                                                             Vreme=x.Vreme,
                                                             Registracija = x.PutniNalog.VozniPark.Oznaka,
                                                             PropisanaPotrosnja=x.PutniNalog.VozniPark.PropisanaPotrosnja,
                                                             Pumpa=x.GorivoPumpa.NazivPumpe,
                                                             Kilometraza=x.Km,
                                                             Litara=x.Litara,
                                                             Cena=x.Cena,
                                                             Iznos=x.Iznos,
                                                             PresaoKmOdPrethodnogSipanja=0,
                                                             ProsekOdPrethodnog=0,
                                                             Napomena=x.Napomena

                                                         }).AsEnumerable();
            if (!String.IsNullOrEmpty(searchTerms))
            {
                gorivoData = GetSearchGorivoData(searchTerms, gorivoData).AsEnumerable();
            }

            var total = gorivoData.Count();

            if (sortOrder.Equals("desc"))
                gorivoData = gorivoData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                gorivoData = gorivoData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Datum" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<GorivoIndexData>()
            {
                total = total,
                rows = gorivoData
            };

            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public IEnumerable<GorivoIndexData> GetSearchGorivoData(string searchTerms, IEnumerable<GorivoIndexData> gorivoData)
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

                    gorivoData = gorivoData.Where(k => k.Datum.HasValue && k.Datum >= datumFirst && k.Datum <= datumSecond);
                }               

                else if (searchColumn.Equals("Registracija") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivoData = gorivoData.Where(k => k.Registracija.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("PropisanaPotrosnja") && !String.IsNullOrEmpty(searchTxt))
                {
                    int kmInt = System.Convert.ToInt32(searchTxt);
                    gorivoData = gorivoData.Where(k => k.PropisanaPotrosnja.Equals(kmInt));
                }
                else if (searchColumn.Equals("Pumpa") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivoData = gorivoData.Where(k => k.Pumpa.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Kilometraza") && !String.IsNullOrEmpty(searchTxt))
                {
                    int KilometrazaInt = System.Convert.ToInt32(searchTxt);
                    gorivoData = gorivoData.Where(k => k.Kilometraza.Equals(KilometrazaInt));
                }
                else if (searchColumn.Equals("Litara") && !String.IsNullOrEmpty(searchTxt))
                {
                    decimal LitaraInt = System.Convert.ToDecimal(searchTxt);
                    gorivoData = gorivoData.Where(k => k.Litara.Equals(LitaraInt));
                }
                else if (searchColumn.Equals("Cena") && !String.IsNullOrEmpty(searchTxt))
                {
                    decimal CenaInt = System.Convert.ToDecimal(searchTxt);
                    gorivoData = gorivoData.Where(k => k.Cena.Equals(CenaInt));
                }
                else if (searchColumn.Equals("Iznos") && !String.IsNullOrEmpty(searchTxt))
                {
                    decimal IznosInt = System.Convert.ToDecimal(searchTxt);
                    gorivoData = gorivoData.Where(k => k.Iznos.Equals(IznosInt));
                }
                else if (searchColumn.Equals("PresaoKmOdPrethodnogSipanja") && !String.IsNullOrEmpty(searchTxt))
                {
                    int PresaoKmOdPrethodnogSipanjaInt = System.Convert.ToInt32(searchTxt);
                    gorivoData = gorivoData.Where(k => k.PresaoKmOdPrethodnogSipanja.Equals(PresaoKmOdPrethodnogSipanjaInt));
                }
                else if (searchColumn.Equals("ProsekOdPrethodnog") && !String.IsNullOrEmpty(searchTxt))
                {
                    decimal ProsekOdPrethodnogInt = System.Convert.ToDecimal(searchTxt);
                    gorivoData = gorivoData.Where(k => k.ProsekOdPrethodnog.Equals(ProsekOdPrethodnogInt));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    gorivoData = gorivoData.Where(k => k.Napomena.ToUpper().Contains(searchTxt.ToUpper()));
                }

            }
            return gorivoData;
        }

        public ActionResult Create(int id)
        {
            var putniNalog = BexUow.PutniNalog.Find(id);

            GorivoTocenje gorivoData = new GorivoTocenje
            {
                PutniNalogId = putniNalog.Id,
                Datum = putniNalog.DatumStart,
                Vreme = TimeSpan.FromHours(8)
            };

            var vozilo = BexUow.VozniPark.AllAsNoTracking.Where(x => x.Id == putniNalog.VpId).FirstOrDefault();
            ViewBag.Vozilo = vozilo.Oznaka;
            ViewBag.PropisanaPotrosnja = vozilo.PropisanaPotrosnja;
            ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe");

            return View(gorivoData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GorivoTocenje gorivoTocenje)
        {
            var putniNalog = BexUow.PutniNalog.Find(gorivoTocenje.PutniNalogId);
            try
            {
                int kilomtraza = 0;
                
                var tocenje = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.PutniNalog.VpId == putniNalog.VpId && x.Storno == false).OrderByDescending(z => z.Datum).ThenByDescending(z => z.Vreme).Take(1).FirstOrDefault();
                kilomtraza = (tocenje == null) ? 0 : tocenje.Km;

                gorivoTocenje.PresaoKmOdPrethodnogSipanja = gorivoTocenje.Km - kilomtraza;
                gorivoTocenje.ProsekOdPrethodnog = (gorivoTocenje.Litara * 100) / gorivoTocenje.PresaoKmOdPrethodnogSipanja;
            }
            catch
            {
                gorivoTocenje.PresaoKmOdPrethodnogSipanja = 0;
                gorivoTocenje.ProsekOdPrethodnog = 0;
            }
            

            if (ModelState.IsValid)
            {
                BexUow.GorivoTocenje.Add(gorivoTocenje);
                putniNalog.BrojSipanja = putniNalog.BrojSipanja + 1;
                putniNalog.Litara = putniNalog.Litara + gorivoTocenje.Litara;
                BexUow.PutniNalog.Update(putniNalog);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Index", "Gorivo");
                }
            }


            return View(gorivoTocenje);

        }

        public ActionResult Edit(int id)
        {
            var gorivoData = BexUow.GorivoTocenje.Find(id);

            ViewBag.PumpaId = new SelectList(BexUow.GorivoPumpa.AllAsNoTracking, "Id", "NazivPumpe", gorivoData.PumpaId);

            var putniNalog = BexUow.PutniNalog.Find(gorivoData.PutniNalogId);
            var vozilo = BexUow.VozniPark.AllAsNoTracking.Where(x => x.Id == putniNalog.VpId).FirstOrDefault();
            ViewBag.Vozilo = vozilo.Oznaka;
            ViewBag.PropisanaPotrosnja = vozilo.PropisanaPotrosnja;


            return View(gorivoData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GorivoTocenje gorivoTocenje)
        {
            int kilomtraza = 0;
            var putniNalog = BexUow.PutniNalog.Find(gorivoTocenje.PutniNalogId);
            var tocenje = BexUow.GorivoTocenje.AllAsNoTracking.Where(x => x.PutniNalog.VpId == putniNalog.VpId && x.Storno == false && x.Id<gorivoTocenje.Id).OrderByDescending(z => z.Datum).ThenByDescending(z => z.Vreme).Take(1).FirstOrDefault();
            kilomtraza = (tocenje == null) ? 0 : tocenje.Km;

            gorivoTocenje.PresaoKmOdPrethodnogSipanja = gorivoTocenje.Km - kilomtraza;
            gorivoTocenje.ProsekOdPrethodnog = (gorivoTocenje.Litara * 100) / gorivoTocenje.PresaoKmOdPrethodnogSipanja;

            if (ModelState.IsValid)
            {
                BexUow.GorivoTocenje.Update(gorivoTocenje);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Index", "Gorivo");
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);

            }
            return HttpNotFound();

        }

        public ActionResult Delete(int id)
        {
            var gorivoData = BexUow.GorivoTocenje.Find(id);

            BexUow.GorivoTocenje.Remove(gorivoData);
            var commandResult = BexUow.SubmitChanges();

            if (commandResult.IsSuccessful)
            {
                return RedirectToAction("Index");
                //alert('@TempData["alertMessage"]');
            }

            ExceptionSolver.PrepareModelState(ModelState, commandResult);
            return View();
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
            if (excelFile == null || excelFile.ContentLength == 0)
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
                                                    if (registracija == "")
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
                            if (brojGresaka > 0)
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

        private ISecurityUow SecurityUow { get; }
    }
}
