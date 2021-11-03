
using System;


namespace BexMVC.Reports
{
    public partial class BaseReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            { return; }

            if (!string.IsNullOrEmpty(Request.QueryString["reportName"]))
            {
                string reportName = Request.QueryString["reportName"];
                

                //if (reportName == "SpecifikacijaRazduzenjaReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    string kurirNaziv = Request.QueryString["kurirNaziv"];
                //    new ReportController().PrepareSpecifikacijaRazduzenjaReport(ReportViewer1, reportName, kurirId, kurirNaziv);
                //}
                //else if (reportName == "PotvrdaRazduzenjaReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PreparePotvrdaRazduzenjaReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "KoverteZaZbirneOtkupeReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareKoverteZaZbirneOtkupeReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "SpecifikacijaZbirnihOtkupaReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareSpecifikacijaZbirnihOtkupaReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "KurirskaListaDostavaReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareKurirskaListaDostavaReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "SpisakDostavaReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareSpisakDostavaReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "SpisakOtkupninaReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareSpisakOtkupninaReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "SpisakTzadatakaReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareSpisakTzadatakaReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "KovertaZaOtkupninuReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareKovertaZaOtkupninuReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "KovertaZaDokumentReport")
                //{
                //    string kurirId = Request.QueryString["kurirId"];
                //    new ReportController().PrepareKovertaZaDokumentReport(ReportViewer1, reportName, kurirId);
                //}
                //else if (reportName == "SpecifikacijaCekovaReport")
                //{
                //    string datumRealizacije = Request.QueryString["datumRealizacije"];
                //    string banka = Request.QueryString["banka"];
                //    new ReportController().PrepareSpecifikacijaCekovaReport(ReportViewer1, reportName, datumRealizacije,banka);
                //}
                //else if (reportName == "BarKodVozilaReport")
                //{
                //    string vozniParkId = Request.QueryString["vozniParkId"];
                //    new ReportController().PrepareBarKodVozilaReport(ReportViewer1, reportName, vozniParkId);
                //}
                //else if (reportName == "TPocitavanjeReport")
                //{
                //    string ocitavanjeId = Request.QueryString["ocitavanjeId"];
                //    new ReportController().PrepareTPocitavanjeReport(ReportViewer1, reportName, ocitavanjeId);
                //}
            }
        }
    }
}
