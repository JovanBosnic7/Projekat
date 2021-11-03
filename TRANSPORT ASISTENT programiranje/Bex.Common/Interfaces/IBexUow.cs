using System;
using Bex.Models;

namespace Bex.Common
{
    public interface IBexUow : IDisposable
    {
        bool IsLogOverwritting { get; set; }
        string LogPath { get; set; }
        IRepository<Novosti> Novosti { get; }
        IRepository<StatKasnjenja> StatKasnjenja { get; }
        IRepository<Gallery> Gallery { get; }
        IRepository<WebFiles> WebFiles { get; }
        IRepository<WebFilesTip> WebFilesTip { get; }
        IKontaktRepository Kontakts { get; }

        IPosiljkaRepository Posiljke { get; }
        IRepository<PosiljkaStatus> PosiljkaStatus { get; }
        IRepository<PosiljkaSadrzaj> PosiljkaSadrzaj { get; }
        IRepository<PosiljkaVrsta> PosiljkaVrsta { get; }
        IRepository<PosiljkaKategorija> PosiljkaKategorija { get; }
        IMagacinRepository ZaduzenjaKurira { get; }
        IRepository<KurirRazduzenje> KurirRazduzenje { get; }
        IRepository<KurirRazduzenjeSpecifikacija> KurirRazduzenjeSpecifikacija { get; }
        IRepository<NapomenaPosiljka> NapomenaPosiljka { get; }

        IRepository<NapomenaPosiljkaPodTip> NapomenaPosiljkaPodTip { get; }

        IRepository<Paket> Paket { get; }
        IRepository<PaketTip> PaketTip { get; }
        IRepository<PosiljkaUsluga> PosiljkaUsluga { get; }
        IRepository<PosiljkaUslugaTip> PosiljkaUslugaTip { get; }
        IRepository<PosiljkaPlacanje> PosiljkaPlacanje { get; }
        IRepository<PosiljkaPlacanjeTip> PosiljkaPlacanjeTip { get; }
        IRepository<PosiljkaNapomena> PosiljkaNapomena { get; }
        IRepository<PosiljkaNapomenaTip> PosiljkaNapomenaTip { get; }
        IRepository<PaketZadatak> PaketZadatak { get; }
        IRepository<PosiljkaZadatak> PosiljkaZadatak { get; }
        IRepository<PosiljkaObrisana> PosiljkaObrisana { get; }

        //IKontaktAdresaRepository KontaktAdresa { get; }
        IRepository<KontaktAdresa> KontaktAdresa { get; }
        IRepository<KontaktDelatnost> KontaktDelatnost { get; }
        IRepository<KontaktEmail> KontaktEmail { get; }
        IRepository<KontaktFizickoLice> KontaktFizickoLice { get; }
        IRepository<KontaktPravnoLice> KontaktPravnoLice { get; }
        IRepository<KontaktTelefon> KontaktTelefon { get; }
        IRepository<KontaktZiroRacun> KontaktZiroRacun { get; }
        IRepository<KontaktUloga> KontaktUloga { get; }
        IRepository<KontaktUlogaTip> KontaktUlogaTip { get; }

        //IRepository<AdresaZaIzbor> AdreseZaIzbor { get; }
        IZaposleniRepository Zaposleni { get; }
        IEvidencijaVozacaRepository EvidencijaVozaca { get; }
        IRepository<ZaposleniNapomena> ZaposleniNapomena { get; }
        IRepository<ZaposleniDan> ZaposleniDan { get; }
        IRepository<ZaposleniDanStatus> ZaposleniDanStatus { get; }

        IRepository<PAK> Pakovi { get; }

        IAddressRepository Addresses { get; }
        IStreetRepository Street { get; }
        //IReonRepository Reoni { get; }
        IReonRepository Reon { get; }
        IReoncicRepository Reoncic { get; }
        IPlaceRepository Place { get; }
        IRepository<Opstina> Opstina { get; }
        IRepository<Region> Region { get; }
        IRepository<Sektor> Sektor { get; }
        IRepository<Zona> Zona { get; }
        IRepository<DimenzijeNav> DimenzijeNav { get; }
        IRepository<ZaposleniZanimanje> ZaposleniZanimanje { get; }
        IRepository<ZaposleniRazlogOtkaza> ZaposleniRazlogOtkaza { get; }
        IRepository<ZaposleniStatus> ZaposleniStatus { get; }
        IRepository<ZaposleniStrucnaSprema> ZaposleniStrucnaSprema { get; }
        IRepository<ZaposleniRadnoMesto> ZaposleniRadnoMesto { get; }
        IRepository<Firma> Firma { get; }
        IRepository<FirmaVP> FirmaVP { get; }
        IRepository<ZaposleniOsnovOsiguranja> ZaposleniOsnovOsiguranja { get; }
        IRepository<ZaposleniProgramZaposlenja> ZaposleniProgramZaposlenja { get; }
        IRepository<ZaposleniSlava> ZaposleniSlava { get; }
        IRepository<ZaposleniPlata> ZaposleniPlata { get; }
        IRepository<ReonTip> ReonTip { get; }

        IKorisniciProgramaRepository KorisniciPrograma { get; }
       
        IRepository<KorisniciProgramaClaims> KorisniciProgramaClaims { get; }
        IRepository<KorisniciProgramaClaimsRoles> KorisniciProgramaClaimsRoles { get; }

        IRepository<PutniNalog> PutniNalog { get; }
        IRepository<PutniNalogTip> PutniNalogTip { get; }
        IVozniParkRepository VozniPark { get; }
        IRepository<Gorivo> Gorivo { get; }
        IRepository<GorivoKartica> GorivoKartica { get; }
        IRepository<GorivoPumpa> GorivoPumpa { get; }
        IGorivoRepository GorivoTocenje { get; }
        //IRepository<GorivoTocenje> GorivoTocenje { get; }
        IRepository<VozniParkBoja> VozniParkBoja { get; }
        IRepository<VozniParkKarakteristike> VozniParkKarakteristike { get; }
        IRepository<VozniParkKaroserija> VozniParkKaroserija { get; }
        IRepository<VozniParkPodKaroserija> VozniParkPodKaroserija { get; }
        IRepository<VozniParkMenjac> VozniParkMenjac { get; }
        IRepository<VozniParkKategorija> VozniParkKategorija { get; }
        IRepository<VozniParkModel> VozniParkModel { get; }

        IRepository<VozniParkMarka> VozniParkMarka { get; }
        IRepository<VozniParkStatus> VozniParkStatus { get; }
        IRepository<VozniParkTip> VozniParkTip { get; }
        IRepository<VozniParkAlarm> VozniParkAlarm { get; }
        IRepository<VpAlarmGrupa> VpAlarmGrupa { get; }
        IRepository<VpAlarmTip> VpAlarmTip { get; }
        IRepository<VpTrosak> VpTrosak { get; }
        IRepository<VpPopravke> VpPopravke { get; }
        IRepository<VpPopravkeTip> VpPopravkeTip { get; }
        IRepository<VozniParkDnevnikTip> VozniParkDnevnikTip { get; }
        IRepository<VozniParkDnevnik> VozniParkDnevnik { get; }
        IRepository<VpOpremaGrupa> VpOpremaGrupa { get; }
        IRepository<VpOpremaTip> VpOpremaTip { get; }
        IRepository<VpOpremaPodTip> VpOpremaPodTip { get; }
        IRepository<VpOprema> VpOprema { get; }



        IRepository<KalendarPlaner> KalendarPlaner { get; }



        IRepository<Sken> Sken { get; }
        IRepository<SkenStart> SkenStart { get; }
        IRepository<SkenPracenje> SkenPracenje { get; }
        IRepository<SkenRead> SkenRead { get; }
        IRepository<SkenTip> SkenTip { get; }


        IRepository<PosiljkaOtkupZbirni> PosiljkaOtkupZbirni { get; }
        IRepository<PosiljkaOtkupZbirniStavka> PosiljkaOtkupZbirniStavka { get; }


        IRepository<Banke> Banke { get; }
        IRepository<BankaPazara> BankaPazara { get; }

        IRepository<BankaPazaraSpecifikacija> BankaPazaraSpecifikacija { get; }
        IRepository<Cekovi> Cekovi { get; }
        IRepository<CekoviProvizija> CekoviProvizija { get; }

        IUgovorRepository Ugovor { get; }
        //IRepository<Ugovor> Ugovor { get; }
        IRepository<UgovorFakturisanje> UgovorFakturisanje { get; }
        IRepository<UgovorKontaktBex> UgovorKontaktBex { get; }
        IRepository<UgovorNacinNaplate> UgovorNacinNaplate { get; }
        IRepository<UgovorNapomena> UgovorNapomena { get; }
        IRepository<UgovorNapomenaTip> UgovorNapomenaTip { get; }
        IRepository<UgovorObracunCena> UgovorObracunCena { get; }
        IRepository<UgovorPovlascenaCenaTip> UgovorPovlascenaCenaTip { get; }
        IRepository<UgovorVip> UgovorVip { get; }
        IRepository<KovertaZaDokument> KovertaZaDokument { get; }
        IRepository<KovertaZaOtkupninu> KovertaZaOtkupninu { get; }
        IRepository<KurirskaListaDostava> KurirskaListaDostava { get; }
        IRepository<SpisakDostava> SpisakDostava { get; }
        IRepository<SpisakOtkupnina> SpisakOtkupnina { get; }
        ICenovnikRepository Cenovnik { get; }
        IRepository<Cene> Cene { get; }
        IRepository<UgovorRpt> UgovorRpt { get; }
        IRepository<UgovorRptCenovnik1> UgovorRptCenovnik1 { get; }
        IRepository<UgovorRptCenovnik2> UgovorRptCenovnik2 { get; }

        IRepository<StatistikaDan> StatistikaDan { get; }
        IRepository<StatistikaSat> StatistikaSat { get; }
        IRepository<StatistikaPorekloPosiljke> StatistikaPorekloPosiljke { get; }
        IRepository<StatistikaRazlogBrisanjaPosiljke> StatistikaRazlogBrisanjaPosiljke { get; }

        IPrijavaRepository PrijavaReklamacijaZalba { get; }
        IRepository<PrijavaReklamacijaZalbaLog> PrijavaReklamacijaZalbaLog { get; }
        IRepository<PrijavaTip> PrijavaTip { get; }
        IRepository<PrijavaPodTip> PrijavaPodTip { get; }
        IRepository<PrijavaNacin> PrijavaNacin { get; }
        IRepository<PrijavaNapomena> PrijavaNapomena { get; }
        IRepository<PrijavaStatus> PrijavaStatus { get; }

        IRepository<Ticket> Ticket { get; }
        IRepository<TicketKategorija> TicketKategorija { get; }
        IRepository<TicketPost> TicketPost { get; }
        IRepository<TicketPrimalac> TicketPrimalac { get; }
        IRepository<TicketPrimalacTip> TicketPrimalacTip { get; }
        IRepository<TicketStatus> TicketStatus { get; }


        IRepository<Trebovanje> Trebovanje { get; }
        IRepository<TrebovanjeStavke> TrebovanjeStavke { get; }
        IRepository<Artikli> Artikli { get; }
        IRepository<ArtikliGrupa> ArtikliGrupa { get; }
        IRepository<ArtikliVrsta> ArtikliVrsta { get; }
        IRepository<MagacinSpisak> MagacinSpisak { get; }
        IRepository<Lager> Lager { get; }
        IRepository<TPtermini> TPtermini { get; }
        ITPterminiRepository TPzakljuci { get; }
        IRepository<TPstatusTermina> TPstatusTermina { get; }
        IRepository<TPlokacije> TPlokacije { get; }
        IRepository<TPocitavanja> TPocitavanja { get; }
        IRepository<StetaTip> StetaTip { get; }
        IRepository<VozniParkSteta> VozniParkSteta { get; }

        IRepository<PravniPostupak> PravniPostupak { get; }
        IRepository<PPvrsta> PPvrsta { get; }
        IRepository<PPoblast> PPoblast { get; }

        IUowCommandResult SubmitChanges();
    }
}
