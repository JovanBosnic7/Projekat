using Bex.Models;
using Bex.MVC.Exceptions;
using Bex.DAL.EF.UOW;
using Bex.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BexMVC.Filters;

namespace BexMVC.Controllers
{
    public class ReportController : Controller
    {
        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
        public ReportController() : this(new BexUow(), new ExceptionSolver())
        { }
        public ReportController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;

        }

        public ActionResult Index()
        {
            return View();
        }

        // da izbegnete kobasičenje treba primeniti
        //  logiku za pronalaženje Validatora (entiteta) iz EF sloja
        //public ActionResult PrintReport(string reportName)
        //{
        //    var localReport = new LocalReport();

        //    string reportRoot = "~/Reports/Rdl/" + reportName + ".rdl";

        //    localReport.ReportPath =
        //        //Server.MapPath("~/Reports/Rdl/SpecifikacijaRazduzenje.rdl");
        //        Server.MapPath(reportRoot);

        //    var dataSource = BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking.ToList();

        //    var reportDataSource = new ReportDataSource();
        //    //reportDataSource.Name = "SpecifikacijaRazduzenjaDataSet";
        //    reportDataSource.Name = reportName;
        //    reportDataSource.Value = dataSource;


        //    localReport.DataSources.Clear();
        //    localReport.DataSources.Add(reportDataSource);

        //    string deviceInfo = "<DeviceInfo>" +
        //     "  <OutputFormat>PDF</OutputFormat>" +
        //     "</DeviceInfo>";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;
        //    string[] streams;
        //    Warning[] warnings;

        //    byte[] renderedBytes = localReport.Render(
        //        "PDF", deviceInfo, out mimeType, out encoding,
        //        out fileNameExtension, out streams, out warnings);

        //    Response.AddHeader("Content-Disposition",
        //        "attachment; filename=" + reportName + ".pdf");
        //    //"attachment; filename=SpecifikacijaRazduzenjaReport.pdf");

        //    return new FileContentResult(renderedBytes, mimeType);
        //}

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintSpecifikacijaRazduzenje")]
        public void PrepareSpecifikacijaRazduzenjaReport(ReportViewer reportViewer, string reportName, string kurirId, string kurirNaziv)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            // da izbegnete kobasičenje treba primeniti
            //  logiku za pronalaženje Validatora (entiteta) iz EF sloja
            if (reportName == "SpecifikacijaRazduzenjaReport")
            {
                var dataSource = (from specifikacija in BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking
                                  join zadatak in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(z => z.DatumIzvrsenja == DateTime.Today.Date) on new { Key1 = specifikacija.PosiljkaId, Key2 = specifikacija.TipZadatka } equals new { Key1 = zadatak.PosiljkaId, Key2 = zadatak.Tip }
                                  join posiljka in BexUow.Posiljke.AllAsNoTracking on specifikacija.PosiljkaId equals posiljka.Id
                                  join kategorija in BexUow.PosiljkaKategorija.AllAsNoTracking on posiljka.KategorijaPosiljkeId equals kategorija.Id
                                  select new
                                  {

                                      Klijent = zadatak.NazivKlijenta + ", " + zadatak.Adresa,
                                      Paketa = posiljka.UkupnoPaketa,
                                      Kategorija = kategorija.NazivKategorije,
                                      Pazar = specifikacija.Usluga,
                                      Otkup = specifikacija.Otkup,
                                      Ukupno = specifikacija.Usluga + specifikacija.Otkup,
                                      KurirId = specifikacija.KurirId,
                                      Datum = specifikacija.Datum,
                                      PosiljkaId = specifikacija.PosiljkaId,
                                      Adresa = zadatak.Adresa
                                  }).Where(x => x.KurirId == kurirIntId && x.Datum== DateTime.Today.Date && x.Ukupno > 0);

                

                var reportDataSource = new ReportDataSource();
                reportDataSource.Name = "SpecifikacijaRazduzenjaDataSet";
                reportDataSource.Value = dataSource;

                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportPath = @"Reports\Rdl\SpecifikacijaRazduzenje.rdl";
                ReportParameter[] parms = new ReportParameter[1];
                parms[0] = new ReportParameter("Kurir", kurirNaziv,true);
                reportViewer.LocalReport.SetParameters(parms);

                //reportViewer.LocalReport.Refresh();
            }
        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintPotvrdeRazduzenje")]
        public void PreparePotvrdaRazduzenjaReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from razduzenje in BexUow.KurirRazduzenje.AllAsNoTracking
                              join reon in BexUow.Reon.AllAsNoTracking on razduzenje.ReonId equals reon.Id
                              join user in BexUow.KorisniciPrograma.AllAsNoTracking on razduzenje.KurirId equals user.Id
                              join kontakt in BexUow.Kontakts.AllAsNoTracking on user.KontaktId equals kontakt.Id
                              select new
                                  {
                                      Naziv = kontakt.Naziv, //user.Kontakt.Naziv,
                                      OznReona = reon.OznReona,
                                      UkupnoOtkupa = razduzenje.UkupnoOtkupa,
                                      UkupnoPazara = razduzenje.UkupnoPazara,
                                      UkupnoNaplatio = razduzenje.UkupnoNaplatio,
                                      UkupnoRazduzio = razduzenje.UkupnoRazduzio,
                                      Razlika = razduzenje.Razlika,
                                      KurirId = razduzenje.KurirId,
                                      Datum = System.Data.Entity.DbFunctions.TruncateTime(razduzenje.Datum)
                                  }).Where(x => x.KurirId == kurirIntId && x.Datum == DateTime.Today.Date).Take(1);



                var reportDataSource = new ReportDataSource();
                reportDataSource.Name = "PotvrdaRazduzenjaDataSet";
                reportDataSource.Value = dataSource;

                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportPath = @"Reports\Rdl\PotvrdaRazduzenje.rdl";
                

                //reportViewer.LocalReport.Refresh();
            
        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintKoverteZbirne")]
        public void PrepareKoverteZaZbirneOtkupeReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from koverteZaZbirne in BexUow.PosiljkaOtkupZbirni.AllAsNoTracking
                              join reon in BexUow.Reon.AllAsNoTracking on koverteZaZbirne.ReonId equals reon.Id
                              join region in BexUow.Region.AllAsNoTracking on reon.RegionId equals region.Id
                              select new
                              {
                                  koverteZaZbirne.BarKod,
                                  koverteZaZbirne.BrojOtkupa,
                                  koverteZaZbirne.UkupnoOtkupa,
                                  koverteZaZbirne.NazivKlijenta,
                                  koverteZaZbirne.Adresa,
                                  koverteZaZbirne.Telefon,
                                  RegionReon = region.NazivSkraceni + reon.OznReona,
                                  koverteZaZbirne.ReonId
                                  //}).Where(x => x.ReonId == kurirIntId); //treba po kuriru, zbog testiranja zakucana jedna zbirna
                              }).Where(x => x.BarKod == "Z2T19030203501");
           




            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "KoverteZaZbirneOtkupeDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\KoverteZaZbirneOtkupe.rdl";


            //reportViewer.LocalReport.Refresh();

        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintSpecifikacijaZbirne")]
        public void PrepareSpecifikacijaZbirnihOtkupaReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from specifikacijaZbirnih in BexUow.PosiljkaOtkupZbirniStavka.AllAsNoTracking
                              join posiljkaOtkupZbirni in BexUow.PosiljkaOtkupZbirni.AllAsNoTracking on specifikacijaZbirnih.BarKod equals posiljkaOtkupZbirni.BarKod
                              join reon in BexUow.Reon.AllAsNoTracking on posiljkaOtkupZbirni.ReonId equals reon.Id
                              join region in BexUow.Region.AllAsNoTracking on reon.RegionId equals region.Id
                              join zadatak in BexUow.PosiljkaZadatak.AllAsNoTracking on specifikacijaZbirnih.PosiljkaId equals zadatak.PosiljkaId
                              join usluga in BexUow.PosiljkaUsluga.AllAsNoTracking on specifikacijaZbirnih.PosiljkaId equals usluga.PosiljkaId
                              select new
                              {
                                  specifikacijaZbirnih.PosiljkaId,
                                  specifikacijaZbirnih.BarKod,
                                  posiljkaOtkupZbirni.NazivKlijenta,
                                  RegionReon = region.NazivSkraceni + reon.OznReona,
                                  DatumNaplateOtkupnine = zadatak.DatumIzvrsenja,
                                  UplatilacOtkupnine = zadatak.NazivKlijenta,
                                  AdresaUplatiocaOtkupnine = zadatak.Adresa,
                                  Otkupnina = usluga.Komada,
                                  posiljkaOtkupZbirni.BrojOtkupa,
                                  posiljkaOtkupZbirni.UkupnoOtkupa,
                                  posiljkaOtkupZbirni.ReonId,
                                  zadatak.Tip,
                                  usluga.TipUslugeId
                              }).Where(x => x.BarKod == "Z2T19030203501" && x.Tip == "D" && x.TipUslugeId == 7);
            //}).Where(x => x.ReonId == reonIntId && x.Tip == "D" && x.TipUslugeId == 7); //treba po kuriru/reonu, zbog testiranja zakucana jedna zbirna

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SpecifikacijaZbirnihOtkupaDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\SpecifikacijaZbirnihOtkupa.rdl";


            //reportViewer.LocalReport.Refresh();

        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintKurirDostava")]
        public void PrepareKurirskaListaDostavaReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from kurirskaListaDostava in BexUow.KurirskaListaDostava.AllAsNoTracking
                              select new
                              {
                                kurirskaListaDostava.PosiljkaId,
                                kurirskaListaDostava.TipZadatka,
                                kurirskaListaDostava.NazivReoncica,
                                kurirskaListaDostava.AktuelanOd,
                                kurirskaListaDostava.VipD,
                                kurirskaListaDostava.UlicaNaziv,
                                kurirskaListaDostava.NazivKlijenta,
                                kurirskaListaDostava.Napomene,
                                kurirskaListaDostava.Telefoni,
                                kurirskaListaDostava.BezgotovinskoP,
                                kurirskaListaDostava.BezgotovinskoD,
                                kurirskaListaDostava.NazivKategorije,
                                kurirskaListaDostava.UkupnoPaketa,
                                kurirskaListaDostava.KurirId,
                                kurirskaListaDostava.Naziv,
                                kurirskaListaDostava.Status,
                                kurirskaListaDostava.Otpremnica,
                                kurirskaListaDostava.Povratnica,
                                kurirskaListaDostava.PlacenOdgovor,
                                kurirskaListaDostava.Otkup,
                                kurirskaListaDostava.Usluga,
                                kurirskaListaDostava.Id  
                            }).Where(x => x.KurirId == kurirIntId);

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "KurirskaListaDostavaDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\KurirskaListaDostava.rdl";


            //reportViewer.LocalReport.Refresh();

        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintDostava")]
        public void PrepareSpisakDostavaReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from spisakDostava in BexUow.SpisakDostava.AllAsNoTracking
                              
                              select new
                              {
                                  spisakDostava.PosiljkaId,
                                  spisakDostava.TipZadatka,
                                  spisakDostava.VipD,
                                  spisakDostava.UlicaNaziv,
                                  spisakDostava.NazivKlijenta,
                                  spisakDostava.Napomene,
                                  spisakDostava.Otkup,
                                  spisakDostava.Usluga,
                                  spisakDostava.UkupnoPaketa,
                                  spisakDostava.KurirId,
                                  spisakDostava.Naziv,
                                  spisakDostava.Id,
                                  spisakDostava.Status,
                                  spisakDostava.Otpremnica,
                                  spisakDostava.Povratnica,
                                  spisakDostava.PlacenOdgovor
                              }).Where(x => x.KurirId == kurirIntId);

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SpisakDostavaDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\SpisakDostava.rdl";


            //reportViewer.LocalReport.Refresh();

        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintOtkup")]
        public void PrepareSpisakOtkupninaReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from spisakOtkupnina in BexUow.SpisakOtkupnina.AllAsNoTracking
                                select new
                              {
                                    spisakOtkupnina.PosiljkaId,
                                    spisakOtkupnina.TipZadatka,
                                    spisakOtkupnina.RegionReonT,
                                    spisakOtkupnina.Posiljalac,
                                    spisakOtkupnina.AdresaP,
                                    spisakOtkupnina.KontaktTelefoniP,
                                    spisakOtkupnina.KurirId,
                                    spisakOtkupnina.Id,
                                    spisakOtkupnina.BarKod,
                                    spisakOtkupnina.Otkupnina,
                                    spisakOtkupnina.Status
                                }).Where(x => x.KurirId == kurirIntId);

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SpisakOtkupninaDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\SpisakOtkupnina.rdl";


            //reportViewer.LocalReport.Refresh();

        }

        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintTZadaci")]
        public void PrepareSpisakTzadatakaReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from kurirskaListaDostava in BexUow.KurirRazduzenjeSpecifikacija.AllAsNoTracking
                              join zadatak in BexUow.PosiljkaZadatak.AllAsNoTracking.Where(z => z.Tip == "T") on new { Key1 = kurirskaListaDostava.PosiljkaId, Key2 = kurirskaListaDostava.TipZadatka } equals new { Key1 = zadatak.PosiljkaId, Key2 = zadatak.Tip }
                              join reon in BexUow.Reon.AllAsNoTracking on kurirskaListaDostava.ReonId equals reon.Id
                              join region in BexUow.Region.AllAsNoTracking on reon.RegionId equals region.Id

                              select new
                              {
                                  kurirskaListaDostava.PosiljkaId,
                                  kurirskaListaDostava.TipZadatka,
                                  RegionReonT = region.NazivSkraceni + reon.OznReona,
                                  Posiljalac = zadatak.NazivKlijenta,
                                  AdresaP = zadatak.Adresa,
                                  KontaktTelefoniP = zadatak.KontaktTelefon + " " + zadatak.KontaktTelefon2,
                                  kurirskaListaDostava.KurirId,
                                  id = kurirskaListaDostava.Id,
                                  kurirskaListaDostava.Status
                              }).Where(x => x.KurirId == kurirIntId && x.Status == false);

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SpisakTzadatakaDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\SpisakTzadataka.rdl";


            //reportViewer.LocalReport.Refresh();

        }
        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintKovertePojedinacniOtkupi")]
        public void PrepareKovertaZaOtkupninuReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from koverteOtkupnina in BexUow.KovertaZaOtkupninu.AllAsNoTracking
                              select new
                              {
                                  koverteOtkupnina.PosiljkaId,
                                  koverteOtkupnina.TipZadatka,
                                  koverteOtkupnina.RegionReonN,
                                  koverteOtkupnina.RegionReonD,
                                  koverteOtkupnina.Naziv,
                                  koverteOtkupnina.Posiljalac,
                                  koverteOtkupnina.Primalac,
                                  koverteOtkupnina.AdresaP,
                                  koverteOtkupnina.AdresaD,
                                  koverteOtkupnina.BarKod,
                                  koverteOtkupnina.KurirId,
                                  koverteOtkupnina.Otkupnina,
                                  koverteOtkupnina.CenaUkupna,
                                  koverteOtkupnina.KontaktImeP,
                                  koverteOtkupnina.KontaktImeD,
                                  koverteOtkupnina.KontaktTelefoniP,
                                  koverteOtkupnina.KontaktTelefoniD,
                                  koverteOtkupnina.Otpremnica,
                                  koverteOtkupnina.Povratnica,
                                  koverteOtkupnina.PlacenOdgovor,
                                  koverteOtkupnina.LicnoUrucenje,
                                  koverteOtkupnina.Vrednost,
                                  koverteOtkupnina.KesPosiljalac,
                                  koverteOtkupnina.KesPrimalac,
                                  koverteOtkupnina.FakturaPosiljalac,
                                  koverteOtkupnina.FakturaPrimalac,
                                  koverteOtkupnina.KesN,
                                  koverteOtkupnina.Platilac
                              }).Where(x => x.KurirId == kurirIntId);

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "KovertaZaOtkupninuDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\KovertaZaOtkupninu.rdl";


            //reportViewer.LocalReport.Refresh();

        }
        [ClaimsAuthentication(Resource = "Magacin", Operation = "PrintTDokumenta")]
        public void PrepareKovertaZaDokumentReport(ReportViewer reportViewer, string reportName, string kurirId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int kurirIntId = int.Parse(kurirId);

            var dataSource = (from koverteDokumenta in BexUow.KovertaZaDokument.AllAsNoTracking
                              select new
                              {
                                  koverteDokumenta.PosiljkaId,
                                  koverteDokumenta.TipZadatka,
                                  koverteDokumenta.RegionReonT,
                                  koverteDokumenta.RegionReonD,
                                  koverteDokumenta.Naziv,
                                  koverteDokumenta.Posiljalac,
                                  koverteDokumenta.Primalac,
                                  koverteDokumenta.AdresaP,
                                  koverteDokumenta.AdresaD,
                                  koverteDokumenta.BarKod,
                                  koverteDokumenta.KurirId,
                                  koverteDokumenta.Otkupnina,
                                  koverteDokumenta.CenaUkupna,
                                  koverteDokumenta.KontaktImeP,
                                  koverteDokumenta.KontaktImeD,
                                  koverteDokumenta.KontaktTelefoniP,
                                  koverteDokumenta.KontaktTelefoniD,
                                  koverteDokumenta.Otpremnica,
                                  koverteDokumenta.Povratnica,
                                  koverteDokumenta.PlacenOdgovor,
                                  koverteDokumenta.LicnoUrucenje,
                                  koverteDokumenta.Vrednost,
                                  koverteDokumenta.KesPosiljalac,
                                  koverteDokumenta.KesPrimalac,
                                  koverteDokumenta.FakturaPosiljalac,
                                  koverteDokumenta.FakturaPrimalac,
                                  koverteDokumenta.KesN,
                                  koverteDokumenta.Platilac
                              }).Where(x => x.KurirId == kurirIntId);



            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "KovertaZaDokumentDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\KovertaZaDokument.rdl";


            //reportViewer.LocalReport.Refresh();

        }

        public void PrepareSpecifikacijaCekovaReport(ReportViewer reportViewer, string reportName, string datumRealizacije,string banka)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            DateTime? datum = null;
            int bankaId = 0;
            if (!String.IsNullOrEmpty(datumRealizacije))
            {
                string[] arrDatumFirst = datumRealizacije.Split('-');
                datum = new DateTime(int.Parse(arrDatumFirst[2]), int.Parse(arrDatumFirst[1]), int.Parse(arrDatumFirst[0]));
            }
            if (!String.IsNullOrEmpty(banka))
            {
                bankaId = System.Convert.ToInt32(banka);
            }

            if (reportName == "SpecifikacijaCekovaReport")
            {
                var dataSource = BexUow.Cekovi.GetAll(true).Where(x => x.BankaId == bankaId && x.DatumDospeca == datum)
                                                       .Select(x =>
                                                       new
                                                       {
                                                           x.Id,
                                                           x.BrojCeka,
                                                           x.BrojSpecifikacije,
                                                           x.DatumDospeca,
                                                           x.Banka.NazivBanke,
                                                           x.Banka.BrojRacuna,
                                                           x.IznosCeka,
                                                           bankaId=x.BankaId,
                                                           x.DatumRealizacije,
                                                           x.BrojTekucegRacuna                                                           
                                                       });


                var reportDataSource = new ReportDataSource();
                reportDataSource.Name = "SpecifikacijaCekovaDataSet1";
                reportDataSource.Value = dataSource;

                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportPath = @"Reports\Rdl\SpecifikacijaCekova.rdl";

            }
            
        }
        public void PrepareBarKodVozilaReport(ReportViewer reportViewer, string reportName, string vozniParkId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int vozniParkIdIntId = int.Parse(vozniParkId);

            var dataSource = (from vozniPark in BexUow.VozniPark.AllAsNoTracking
                              select new
                              {
                                  IdVP = vozniPark.Id,
                                  GarazniBroj = vozniPark.GarazniBroj,
                                  BarKod = vozniPark.BarKod,
                                  Oznaka = vozniPark.Oznaka,
                                  NazivVozila = vozniPark.NazivVozila,
                                  Opis = vozniPark.Opis
                              }).Where(x => x.IdVP == vozniParkIdIntId);

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "BarKodVozilaDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\BarKodVozila.rdl";


            //reportViewer.LocalReport.Refresh();

        }
        public void PrepareTPocitavanjeReport(ReportViewer reportViewer, string reportName, string ocitavanjeId)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;

            int ocitavanjeIdIntId = int.Parse(ocitavanjeId);


            var dataSource = (from tpOcitavanja in BexUow.TPocitavanja.AllAsNoTracking
                              join model in BexUow.VozniParkModel.AllAsNoTracking on tpOcitavanja.ModelId equals model.Id 
                              join marka in BexUow.VozniParkMarka.AllAsNoTracking on model.MarkaId equals marka.Id
                              join kategorija in BexUow.VozniParkKategorija.AllAsNoTracking on tpOcitavanja.KategorijaVozilaId equals kategorija.Id into kt
                              from kategorija in kt.DefaultIfEmpty()
                              join boja in BexUow.VozniParkBoja.AllAsNoTracking on tpOcitavanja.BojaId equals boja.Id into b
                              from boja in b.DefaultIfEmpty()
                                  //join zaposleni in BexUow.Zaposleni.AllAsNoTracking on tpOcitavanja.KontrolorId equals zaposleni.Id
                              join korisnik in BexUow.KorisniciPrograma.AllAsNoTracking on tpOcitavanja.UserUneoId equals korisnik.Id
                              join kontakt in BexUow.Kontakts.AllAsNoTracking on korisnik.KontaktId equals kontakt.Id
                              join karoserija in BexUow.VozniParkKaroserija.AllAsNoTracking on tpOcitavanja.KaroserijaVozilaId equals karoserija.Id into ks
                              from karoserija in ks.DefaultIfEmpty()
                              select new
                              {
                                  IdOcitavanja = tpOcitavanja.Id,
                                  NazivModela = model.NazivModela,
                                  NazivMarke = marka.NazivMarke,
                                  KategorijaNaziv = (kategorija.NazivPodkategorije != kategorija.KategorijaNaziv ? kategorija.KategorijaNaziv + " " + kategorija.NazivPodkategorije : kategorija.KategorijaNaziv) + " " + karoserija.NazivKaroserije,
                                  BrojMotora = tpOcitavanja.BrojMotora,
                                  BrojSasije = tpOcitavanja.BrojSasije,
                                  BrojVrata = tpOcitavanja.BrojVrata,
                                  NazivBoje = boja.NazivBoje,
                                  Naziv = kontakt.Naziv,
                                  DatumOcitavanja = tpOcitavanja.DatumOcitavanja
                              }).Where(x => x.IdOcitavanja == ocitavanjeIdIntId);

            var reportDataSource = new ReportDataSource();
            reportDataSource.Name = "TPocitavanjaDataSet";
            reportDataSource.Value = dataSource;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = @"Reports\Rdl\RegistracioniList.rdl";


            //reportViewer.LocalReport.Refresh();

        }

    }
}