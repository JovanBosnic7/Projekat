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
using System.Globalization;

namespace DDtrafic.Controllers
{
  
    public class EvidencijaVozacaController : Controller
    {
        public EvidencijaVozacaController() : this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()), new BexUow())
        { }
        public EvidencijaVozacaController(
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
        public async Task<ActionResult> GetEvidencija(int pageSize, int pageNumber, string sortColumn, string sortOrder, string search, string searchColumn, string searchTerms)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);

            var skip = (pageNumber - 1) * pageSize;

            var evidencijaData = BexUow.EvidencijaVozaca.GetAll(true).Where(x => x.Zaposleni.FirmaId==korisnikPrograma.FirmaId)
                                                    .Select(x =>
                                                         new EvidencijaVozacaIndexData
                                                         {
                                                             Id = x.Id,
                                                             Vozac=x.Zaposleni.ImeIprezime,
                                                             Dan=x.Dan,
                                                             Datum=x.Datum,
                                                             VremePocetka=x.VremePocetka,
                                                             VremeZavrsetka=x.VremeZavrsetka,
                                                             RadniDan=x.RadniDan,
                                                             DnevnoRadnoVreme=x.DnevnoRadnoVreme,
                                                             UpravljanjeVozilom=x.UpravljanjeVozilom,
                                                             OstaloRadnoVreme=x.OstaloRadnoVreme,
                                                             VremeRaspolozivosti=x.VremeRaspolozivosti,
                                                             VremeOdmora=x.VremeOdmora,
                                                             PlacenoOdsustvo=x.PlacenoOdsustvo,
                                                             NocniRad=x.NocniRad
                                                         }).AsEnumerable();
            if (!String.IsNullOrEmpty(searchTerms))
            {
                evidencijaData = GetSearchEvidencijaData(searchTerms, evidencijaData).AsEnumerable();
            }

            var total = evidencijaData.Count();

            if (sortOrder.Equals("desc"))
                evidencijaData = evidencijaData.OrderByDescending(s => s.GetType().GetProperty(sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);
            else
                evidencijaData = evidencijaData.OrderBy(s => s.GetType().GetProperty((sortColumn == "") ? "Datum" : sortColumn).GetValue(s)).ToList().Skip(skip).Take(pageSize);


            var jsonData = new TableJsonIndexData<EvidencijaVozacaIndexData>()
            {
                total = total,
                rows = evidencijaData
            };

            var jsonResult = Json(jsonData, "TableJsonIndexData", JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public IEnumerable<EvidencijaVozacaIndexData> GetSearchEvidencijaData(string searchTerms, IEnumerable<EvidencijaVozacaIndexData> evidencijaData)
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

                    evidencijaData = evidencijaData.Where(k => k.Datum.HasValue &&  k.Datum >= datumFirst && k.Datum <= datumSecond);
                }               

                else if (searchColumn.Equals("Vozac") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.Vozac.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("Dan") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.Dan.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("VremePocetka") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.VremePocetka.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("VremeZavrsetka") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.VremeZavrsetka.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("RadniDan") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.RadniDan.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("DnevnoRadnoVreme") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.DnevnoRadnoVreme.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("UpravljanjeVozilom") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.UpravljanjeVozilom.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("OstaloRadnoVreme") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.OstaloRadnoVreme.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("VremeRaspolozivosti") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.VremeRaspolozivosti.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("VremeOdmora") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.VremeOdmora.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("PlacenoOdsustvo") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.PlacenoOdsustvo.ToUpper().Contains(searchTxt.ToUpper()));
                }
                else if (searchColumn.Equals("NocniRad") && !String.IsNullOrEmpty(searchTxt))
                {
                    evidencijaData = evidencijaData.Where(k => k.NocniRad.ToUpper().Contains(searchTxt.ToUpper()));
                }

            }
            return evidencijaData;
        }

        public ActionResult Create(int id)
        {
            var zaposleni = BexUow.Zaposleni.Find(id);

            EvidencijaVozaca evidencijaData = new EvidencijaVozaca
            {
                ZaposleniId = zaposleni.Id
            };

            ViewBag.Zaposleni = zaposleni.ImeIprezime;

            return View(evidencijaData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EvidencijaVozaca evidencija)
        {

            if (ModelState.IsValid)
            {
                BexUow.EvidencijaVozaca.Add(evidencija);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Index", "Gorivo");
                }
            }
            return View(evidencija);

        }

        public ActionResult Edit(int id)
        {
            var evidencijaData = BexUow.EvidencijaVozaca.Find(id);

            var zaposleni = BexUow.Zaposleni.AllAsNoTracking.Where(x => x.Id == evidencijaData.ZaposleniId).FirstOrDefault();
            ViewBag.Zaposleni = zaposleni.ImeIprezime;


            return View(evidencijaData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EvidencijaVozaca evidencija)
        {

            if (ModelState.IsValid)
            {
                BexUow.EvidencijaVozaca.Update(evidencija);
                var commandResult = BexUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                {
                    return RedirectToAction("Index", "EvidencijaVozaca");
                }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);

            }
            return HttpNotFound();

        }

        public ActionResult Delete(int id)
        {
            var evidencijaData = BexUow.EvidencijaVozaca.Find(id);

            BexUow.EvidencijaVozaca.Remove(evidencijaData);
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
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelFile, EvidencijaVozaca evidencija)
        {
            int brojGresaka = 0;
            int brojUspesno = 0;
            int brojUkupno = 0;
            var zaposleniId = evidencija.ZaposleniId;
            var dan = "";
            var datum = "";
            var vremePocetka = "";
            var vremeZavrsetka = "";
            var radniDan = "";
            var dnevnoRadnoVreme = "";
            var upravljanjeVozilom = "";
            var ostaloRadnoVreme = "";
            var vremeRaspolozivosti = "";
            var vremeOdmora = "";
            var placenoOdsustvo = "";
            var nocniRad = "";

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
                            if (brojKolone == 12)
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
                                                if (i == 0)//dan
                                                {
                                                    dan = dr.GetValue(i).ToString();
                                                }
                                                if (i == 1)//datum
                                                {
                                                    datum = dr.GetValue(i).ToString();                                                    
                                                }
                                                if (i == 2)//vreme start
                                                {
                                                    vremePocetka = dr.GetValue(i).ToString();
                                                }
                                                if (i == 3)//vreme stop
                                                {
                                                    vremeZavrsetka = dr.GetValue(i).ToString();
                                                }
                                                if (i == 4)//radni dan
                                                {
                                                    radniDan = dr.GetValue(i).ToString();
                                                }
                                                if (i == 5)//dnevno
                                                {
                                                    dnevnoRadnoVreme = dr.GetValue(i).ToString();
                                                }
                                                if (i == 6)//upravljanje
                                                {
                                                    upravljanjeVozilom = dr.GetValue(i).ToString();
                                                }
                                                if (i == 7)//ostalo
                                                {
                                                    ostaloRadnoVreme = dr.GetValue(i).ToString();
                                                }
                                                if (i == 8)//raspolozivost
                                                {
                                                    vremeRaspolozivosti = dr.GetValue(i).ToString();
                                                }
                                                if (i == 9)//odmor
                                                {
                                                    vremeOdmora = dr.GetValue(i).ToString();
                                                }
                                                if (i == 10)//odsustvo
                                                {
                                                    placenoOdsustvo = dr.GetValue(i).ToString();
                                                }
                                                if (i == 11)//nocni rad
                                                {
                                                    nocniRad = dr.GetValue(i).ToString();
                                                }
                                            }

                                        }
                                        catch (Exception k)
                                        {
                                            brojGresaka++;
                                            ViewBag.ErrorMessage = ViewBag.ErrorMessage + "\n" + k.Message;
                                        }
                                        BexUow.EvidencijaVozaca.EvidencijaImport(zaposleniId,dan,DateTime.ParseExact(datum.ToString(), "dd.MM.yyyy.", System.Globalization.CultureInfo.InvariantCulture),SubStringFromChar(vremePocetka," "), SubStringFromChar(vremeZavrsetka," "), SubStringFromChar(radniDan," "), SubStringFromChar(dnevnoRadnoVreme," "), SubStringFromChar(upravljanjeVozilom," "), SubStringFromChar(ostaloRadnoVreme," "), SubStringFromChar(vremeRaspolozivosti," "), SubStringFromChar(vremeOdmora," "), SubStringFromChar(placenoOdsustvo," "), SubStringFromChar(nocniRad," "));
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Driver ne može da pronađe 12 kolona. Snimite novi fajl.";
                                return View();
                            }
                            dr.Close();
                            konekcijaExcel.Close();
                            if (brojGresaka > 0)
                            {
                                ViewBag.ErrorMessage = ViewBag.ErrorMessage + "\n" + "Ukupno redova:" + brojUkupno.ToString() + " Ukupno grešaka:" + brojGresaka.ToString();
                                return View();
                            }
                            else
                            {
                                return RedirectToAction("Index", "EvidencijaVozaca");
                            }
                        }

                    }
                    catch (OleDbException ex1)
                    {
                        if (ex1.ErrorCode == -2147467259)
                        {
                            ViewBag.ErrorMessage = "Excel fajl koji pokušavate da učitate je otvoren u nekom drugom programu. Zatvorite ga pa pokušajte ponovo.";
                            return View();
                        }
                        else
                        {
                            ViewBag.ErrorMessage = ex1.ToString();
                            return View();
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Izaberite fajl nije u dobrom formatu.";
                    return View();
                }
            }

        }

        private string SubStringFromChar(string text, string stopAt)
        {
            int ukupno = text.Length;
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(charLocation+1,ukupno-(charLocation+1));
                }
            }

            return String.Empty;
        }

        public async Task<ActionResult> GetZaposleni(string query)
        {
            var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId()) as ApplicationUser;
            var korisnikPrograma = BexUow.KorisniciPrograma.Find(x => x.AspNetUserId == applicationUser.Id);

            return Json(_GetZaposleni(query, korisnikPrograma.FirmaId), JsonRequestBehavior.AllowGet);
        }
        private List<Autocomplete> _GetZaposleni(string query, int? firmaId)
        {


            List<Autocomplete> zaposleniList = new List<Autocomplete>();
            try
            {
                zaposleniList = BexUow.Zaposleni.AllAsNoTracking
                                                   .Where(x => x.ImeIprezime.ToUpper().Contains(query.ToUpper()) && x.FirmaId == firmaId)
                                                   .Select(a => new Autocomplete
                                                   {
                                                       Name = a.ImeIprezime,
                                                       Id = a.Id
                                                   }
                                                   )
                                                   .ToList();
            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }
            catch
            {
                throw;
            }
            return zaposleniList;
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
