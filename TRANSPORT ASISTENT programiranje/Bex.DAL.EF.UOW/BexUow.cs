using Bex.Common;
using Bex.Models;
using Bex.DAL.EF.Common;
using Bex.DAL.EF.Factory;
using Bex.DAL.EF.Logging;
using Bex.DAL.EF.UOW;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Transactions;
using Bex.DAL.EF.Contexts.Main;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bex.DAL.EF.UOW
{
    public class BexUow : IBexUow, IDisposable
    {
        static BexUow()
        {
            string logPath = null;
            //demo purpose
            //var dateString = DateTime.Now.Date.ToString("yyyy-MM-dd");
            //logPath = $@"C:\1MyProj\Bexeexpress\EFLogs\BexUow {dateString}.log";

            SqlLogger = new SqlLogger { LogPath = logPath };
            DbInterception.Add(SqlLogger);
        }
        public BexUow(bool withLogging = false) :
            this(new List<DbContext>
                {
                   new BexMainDbContext()
                    
                },
                new RepositoryProvider(),
                new UowCommandResultFactory(),
               
                withLogging)
        { }
        public BexUow(
            IList<DbContext> dbContexts,
            IRepositoryProvider repositoryProvider,
            IUowCommandResultFactory uowCommandResultFactory,
            bool withLogging)
        {
            UowCommandResultFactory = uowCommandResultFactory;
            DbContexts = dbContexts;
            ConfigureDbContexts();

            RepositoryProvider = repositoryProvider;
            PrimaryDbContext = DbContexts[0];
            RepositoryProvider.DbContext = PrimaryDbContext;

            LogPath = SqlLogger.LogPath;
            IsLogOverwritting = SqlLogger.IsOverwritting;

            if (withLogging)
            { SqlLogger.StartLogging(); }
            else
            { SqlLogger.StopLogging(); }
        }

        
        public bool IsLogOverwritting { get; set; }
        public string LogPath { get; set; }


        //public IRepository<Kontakt> Kontakts =>
        //    RepositoryProvider.GetEntityRepository<Kontakt>(PrimaryDbContext);
        public IRepository<Novosti> Novosti =>
           RepositoryProvider.GetEntityRepository<Novosti>(PrimaryDbContext);
        public IRepository<Trebovanje> Trebovanje =>
            RepositoryProvider.GetEntityRepository<Trebovanje>(PrimaryDbContext);
        public IRepository<TrebovanjeStavke> TrebovanjeStavke =>
            RepositoryProvider.GetEntityRepository<TrebovanjeStavke>(PrimaryDbContext);
        public IRepository<Artikli> Artikli =>
            RepositoryProvider.GetEntityRepository<Artikli>(PrimaryDbContext);
        public IRepository<ArtikliGrupa> ArtikliGrupa =>
            RepositoryProvider.GetEntityRepository<ArtikliGrupa>(PrimaryDbContext);
        public IRepository<ArtikliVrsta> ArtikliVrsta =>
            RepositoryProvider.GetEntityRepository<ArtikliVrsta>(PrimaryDbContext);

        public IRepository<MagacinSpisak> MagacinSpisak =>
            RepositoryProvider.GetEntityRepository<MagacinSpisak>(PrimaryDbContext);

        public IRepository<Lager> Lager =>
            RepositoryProvider.GetEntityRepository<Lager>(PrimaryDbContext);
        public IRepository<TPtermini> TPtermini =>
            RepositoryProvider.GetEntityRepository<TPtermini>(PrimaryDbContext);
        
        public ITPterminiRepository TPzakljuci =>
            RepositoryProvider.GetRepository<ITPterminiRepository>(
                dbContext => new TPterminiRepository(PrimaryDbContext));
        public IRepository<TPstatusTermina> TPstatusTermina =>
            RepositoryProvider.GetEntityRepository<TPstatusTermina>(PrimaryDbContext);
        public IRepository<TPlokacije> TPlokacije =>
            RepositoryProvider.GetEntityRepository<TPlokacije>(PrimaryDbContext);
        public IRepository<TPocitavanja> TPocitavanja =>
            RepositoryProvider.GetEntityRepository<TPocitavanja>(PrimaryDbContext);
        public IRepository<StetaTip> StetaTip =>
            RepositoryProvider.GetEntityRepository<StetaTip>(PrimaryDbContext);
        public IRepository<StetaKategorija> StetaKategorija =>
            RepositoryProvider.GetEntityRepository<StetaKategorija>(PrimaryDbContext);

        public IRepository<VozniParkSteta> VozniParkSteta =>
            RepositoryProvider.GetEntityRepository<VozniParkSteta>(PrimaryDbContext);
        public IRepository<StatKasnjenja> StatKasnjenja =>
            RepositoryProvider.GetEntityRepository<StatKasnjenja>(PrimaryDbContext);

        public IRepository<Gallery> Gallery =>
            RepositoryProvider.GetEntityRepository<Gallery>(PrimaryDbContext);

        public IRepository<WebFiles> WebFiles =>
            RepositoryProvider.GetEntityRepository<WebFiles>(PrimaryDbContext);

        public IRepository<WebFilesTip> WebFilesTip =>
           RepositoryProvider.GetEntityRepository<WebFilesTip>(PrimaryDbContext);

        public IKontaktRepository Kontakts =>
            RepositoryProvider.GetRepository<IKontaktRepository>(
                dbContext => new KontaktRepository(PrimaryDbContext));

        public IPosiljkaRepository Posiljke =>
            RepositoryProvider.GetRepository<IPosiljkaRepository>(
                dbContext => new PosiljkaRepository(PrimaryDbContext));

        public IRepository<PosiljkaStatus> PosiljkaStatus =>
            RepositoryProvider.GetEntityRepository<PosiljkaStatus>(PrimaryDbContext);

        public IRepository<PosiljkaKategorija> PosiljkaKategorija =>
            RepositoryProvider.GetEntityRepository<PosiljkaKategorija>(PrimaryDbContext);

        public IRepository<PosiljkaSadrzaj> PosiljkaSadrzaj =>
            RepositoryProvider.GetEntityRepository<PosiljkaSadrzaj>(PrimaryDbContext);

        public IRepository<PosiljkaVrsta> PosiljkaVrsta =>
            RepositoryProvider.GetEntityRepository<PosiljkaVrsta>(PrimaryDbContext);

        public IRepository<PosiljkaZadatak> PosiljkaZadatak =>
            RepositoryProvider.GetEntityRepository<PosiljkaZadatak>(PrimaryDbContext);

        public IRepository<PosiljkaObrisana> PosiljkaObrisana =>
           RepositoryProvider.GetEntityRepository<PosiljkaObrisana>(PrimaryDbContext);

        public IMagacinRepository ZaduzenjaKurira =>
           RepositoryProvider.GetRepository<IMagacinRepository>(
               dbContext => new MagacinRepository(PrimaryDbContext));

        public IRepository<KurirRazduzenje> KurirRazduzenje =>
            RepositoryProvider.GetEntityRepository<KurirRazduzenje>(PrimaryDbContext);

        public IRepository<KurirRazduzenjeSpecifikacija> KurirRazduzenjeSpecifikacija =>
           RepositoryProvider.GetEntityRepository<KurirRazduzenjeSpecifikacija>(PrimaryDbContext);

        public IRepository<NapomenaPosiljka> NapomenaPosiljka =>
           RepositoryProvider.GetEntityRepository<NapomenaPosiljka>(PrimaryDbContext);

        public IRepository<NapomenaPosiljkaPodTip> NapomenaPosiljkaPodTip =>
           RepositoryProvider.GetEntityRepository<NapomenaPosiljkaPodTip>(PrimaryDbContext);

        public IRepository<Paket> Paket =>
            RepositoryProvider.GetEntityRepository<Paket>(PrimaryDbContext);

        public IRepository<PaketTip> PaketTip =>
            RepositoryProvider.GetEntityRepository<PaketTip>(PrimaryDbContext);
        public IRepository<PosiljkaUsluga> PosiljkaUsluga =>
            RepositoryProvider.GetEntityRepository<PosiljkaUsluga>(PrimaryDbContext);
        public IRepository<PosiljkaUslugaTip> PosiljkaUslugaTip =>
            RepositoryProvider.GetEntityRepository<PosiljkaUslugaTip>(PrimaryDbContext);
        public IRepository<PosiljkaPlacanje> PosiljkaPlacanje =>
            RepositoryProvider.GetEntityRepository<PosiljkaPlacanje>(PrimaryDbContext);
        public IRepository<PosiljkaPlacanjeTip> PosiljkaPlacanjeTip =>
            RepositoryProvider.GetEntityRepository<PosiljkaPlacanjeTip>(PrimaryDbContext);
        public IRepository<PosiljkaNapomena> PosiljkaNapomena =>
            RepositoryProvider.GetEntityRepository<PosiljkaNapomena>(PrimaryDbContext);
        public IRepository<PosiljkaNapomenaTip> PosiljkaNapomenaTip =>
            RepositoryProvider.GetEntityRepository<PosiljkaNapomenaTip>(PrimaryDbContext);
        public IRepository<PaketZadatak> PaketZadatak =>
            RepositoryProvider.GetEntityRepository<PaketZadatak>(PrimaryDbContext);



        //public IKontaktAdresaRepository KontaktAdresa =>
        //    RepositoryProvider.GetRepository<IKontaktAdresaRepository>(
        //        dbContext => new KontaktAdresaRepository(PrimaryDbContext));

        public IRepository<KontaktAdresa> KontaktAdresa =>
                            RepositoryProvider.GetEntityRepository<KontaktAdresa>(PrimaryDbContext);
        public IRepository<KontaktPravnoLice> KontaktPravnoLice =>
           RepositoryProvider.GetEntityRepository<KontaktPravnoLice>(PrimaryDbContext);
        public IRepository<KontaktFizickoLice> KontaktFizickoLice =>
           RepositoryProvider.GetEntityRepository<KontaktFizickoLice>(PrimaryDbContext);
        public IRepository<KontaktEmail> KontaktEmail =>
           RepositoryProvider.GetEntityRepository<KontaktEmail>(PrimaryDbContext);
        public IRepository<KontaktTelefon> KontaktTelefon =>
           RepositoryProvider.GetEntityRepository<KontaktTelefon>(PrimaryDbContext);

        public IRepository<KontaktZiroRacun> KontaktZiroRacun =>
           RepositoryProvider.GetEntityRepository<KontaktZiroRacun>(PrimaryDbContext);

        public IRepository<KontaktDelatnost> KontaktDelatnost =>
           RepositoryProvider.GetEntityRepository<KontaktDelatnost>(PrimaryDbContext);

        public IRepository<KontaktUloga> KontaktUloga =>
          RepositoryProvider.GetEntityRepository<KontaktUloga>(PrimaryDbContext);

        public IRepository<KontaktUlogaTip> KontaktUlogaTip =>
          RepositoryProvider.GetEntityRepository<KontaktUlogaTip>(PrimaryDbContext);

        public IRepository<PAK> Pakovi =>
          RepositoryProvider.GetEntityRepository<PAK>(PrimaryDbContext);
        public IAddressRepository Addresses =>
            RepositoryProvider.GetRepository<IAddressRepository>(
                dbContext => new AddressRepository(PrimaryDbContext));

        public IStreetRepository Street =>
            RepositoryProvider.GetRepository<IStreetRepository>(
                dbContext => new StreetRepository(PrimaryDbContext));

        public IReonRepository Reon =>
            RepositoryProvider.GetRepository<IReonRepository>(
                dbContext => new ReonRepository(PrimaryDbContext));
        //public IRepository<Reon> Reon =>
        //    RepositoryProvider.GetEntityRepository<Reon>(PrimaryDbContext);

        public IReoncicRepository Reoncic =>
            RepositoryProvider.GetRepository<IReoncicRepository>(
                dbContext => new ReoncicRepository(PrimaryDbContext));

        public IRepository<Region> Region =>
          RepositoryProvider.GetEntityRepository<Region>(PrimaryDbContext);

        //public IRepository<Place> Place =>
        //  RepositoryProvider.GetEntityRepository<Place>(PrimaryDbContext);
        public IPlaceRepository Place =>
            RepositoryProvider.GetRepository<IPlaceRepository>(
                dbContext => new PlaceRepository(PrimaryDbContext));
        public IRepository<Opstina> Opstina =>
            RepositoryProvider.GetEntityRepository<Opstina>(PrimaryDbContext);

        public IRepository<DimenzijeNav> DimenzijeNav =>
            RepositoryProvider.GetEntityRepository<DimenzijeNav>(PrimaryDbContext);

        //public IRepository<AdresaZaIzbor> AdreseZaIzbor =>
        //  RepositoryProvider.GetEntityRepository<AdresaZaIzbor>(PrimaryDbContext);

        public IRepository<Zona> Zona =>
            RepositoryProvider.GetEntityRepository<Zona>(PrimaryDbContext);
        public IRepository<Sektor> Sektor =>
            RepositoryProvider.GetEntityRepository<Sektor>(PrimaryDbContext);

        public IEvidencijaVozacaRepository EvidencijaVozaca =>
            RepositoryProvider.GetRepository<IEvidencijaVozacaRepository>(
                dbContext => new EvidencijaVozacaRepository(PrimaryDbContext));

        public IRepository<ZaposleniZanimanje> ZaposleniZanimanje =>
            RepositoryProvider.GetEntityRepository<ZaposleniZanimanje>(PrimaryDbContext);
        public IRepository<ZaposleniRazlogOtkaza> ZaposleniRazlogOtkaza =>
            RepositoryProvider.GetEntityRepository<ZaposleniRazlogOtkaza>(PrimaryDbContext);

        public IRepository<ZaposleniStatus> ZaposleniStatus =>
            RepositoryProvider.GetEntityRepository<ZaposleniStatus>(PrimaryDbContext);
        public IRepository<ZaposleniStrucnaSprema> ZaposleniStrucnaSprema =>
            RepositoryProvider.GetEntityRepository<ZaposleniStrucnaSprema>(PrimaryDbContext);
        public IRepository<ZaposleniRadnoMesto> ZaposleniRadnoMesto =>
            RepositoryProvider.GetEntityRepository<ZaposleniRadnoMesto>(PrimaryDbContext);
        public IRepository<ZaposleniDan> ZaposleniDan =>
            RepositoryProvider.GetEntityRepository<ZaposleniDan>(PrimaryDbContext);
        public IRepository<ZaposleniDanStatus> ZaposleniDanStatus =>
            RepositoryProvider.GetEntityRepository<ZaposleniDanStatus>(PrimaryDbContext);
        public IRepository<Firma> Firma =>
            RepositoryProvider.GetEntityRepository<Firma>(PrimaryDbContext);
        public IRepository<FirmaVP> FirmaVP =>
            RepositoryProvider.GetEntityRepository<FirmaVP>(PrimaryDbContext);
        public IRepository<ZaposleniOsnovOsiguranja> ZaposleniOsnovOsiguranja =>
            RepositoryProvider.GetEntityRepository<ZaposleniOsnovOsiguranja>(PrimaryDbContext);
        public IRepository<ZaposleniProgramZaposlenja> ZaposleniProgramZaposlenja =>
            RepositoryProvider.GetEntityRepository<ZaposleniProgramZaposlenja>(PrimaryDbContext);
        public IRepository<ZaposleniSlava> ZaposleniSlava =>
            RepositoryProvider.GetEntityRepository<ZaposleniSlava>(PrimaryDbContext);
        public IRepository<ZaposleniPlata> ZaposleniPlata =>
            RepositoryProvider.GetEntityRepository<ZaposleniPlata>(PrimaryDbContext);

        public IRepository<ZaposleniNapomena> ZaposleniNapomena =>
           RepositoryProvider.GetEntityRepository<ZaposleniNapomena>(PrimaryDbContext);


        public IRepository<ReonTip> ReonTip =>
            RepositoryProvider.GetEntityRepository<ReonTip>(PrimaryDbContext);

        //public IRepository<KorisniciPrograma> KorisniciPrograma =>
        //    RepositoryProvider.GetEntityRepository<KorisniciPrograma>(PrimaryDbContext);

        public IRepository<KorisniciProgramaClaims> KorisniciProgramaClaims =>
            RepositoryProvider.GetEntityRepository<KorisniciProgramaClaims>(PrimaryDbContext);

        public IRepository<KorisniciProgramaClaimsRoles> KorisniciProgramaClaimsRoles =>
            RepositoryProvider.GetEntityRepository<KorisniciProgramaClaimsRoles>(PrimaryDbContext);

        public IKorisniciProgramaRepository KorisniciPrograma =>
            RepositoryProvider.GetRepository<IKorisniciProgramaRepository>(
                dbContext => new KorisniciProgramaRepository(PrimaryDbContext));

        public IZaposleniRepository Zaposleni =>
            RepositoryProvider.GetRepository<IZaposleniRepository>(
                dbContext => new ZaposleniRepository(PrimaryDbContext));

        public IRepository<PutniNalog> PutniNalog =>
            RepositoryProvider.GetEntityRepository<PutniNalog>(PrimaryDbContext);
        public IRepository<PutniNalogTip> PutniNalogTip =>
            RepositoryProvider.GetEntityRepository<PutniNalogTip>(PrimaryDbContext);

        public IVozniParkRepository VozniPark =>
            RepositoryProvider.GetRepository<IVozniParkRepository>(
                dbContext => new VozniParkRepository(PrimaryDbContext));
        public IRepository<Gorivo> Gorivo =>
            RepositoryProvider.GetEntityRepository<Gorivo>(PrimaryDbContext);


        public IRepository<GorivoKartica> GorivoKartica => 
            RepositoryProvider.GetEntityRepository<GorivoKartica>(PrimaryDbContext);

        public IRepository<GorivoPumpa> GorivoPumpa => 
            RepositoryProvider.GetEntityRepository<GorivoPumpa>(PrimaryDbContext);

        public IGorivoRepository GorivoTocenje =>
            RepositoryProvider.GetRepository<IGorivoRepository>(
                dbContext => new GorivoRepository(PrimaryDbContext));
        //public IRepository<GorivoTocenje> GorivoTocenje => 
        //    RepositoryProvider.GetEntityRepository<GorivoTocenje>(PrimaryDbContext);
        public IRepository<VozniParkBoja> VozniParkBoja =>
            RepositoryProvider.GetEntityRepository<VozniParkBoja>(PrimaryDbContext);
        public IRepository<VozniParkKarakteristike> VozniParkKarakteristike =>
            RepositoryProvider.GetEntityRepository<VozniParkKarakteristike>(PrimaryDbContext);
        public IRepository<VozniParkKaroserija> VozniParkKaroserija =>
            RepositoryProvider.GetEntityRepository<VozniParkKaroserija>(PrimaryDbContext);
        public IRepository<VozniParkPodKaroserija> VozniParkPodKaroserija =>
           RepositoryProvider.GetEntityRepository<VozniParkPodKaroserija>(PrimaryDbContext);
        public IRepository<VozniParkMenjac> VozniParkMenjac =>
            RepositoryProvider.GetEntityRepository<VozniParkMenjac>(PrimaryDbContext);
        public IRepository<VozniParkKategorija> VozniParkKategorija =>
            RepositoryProvider.GetEntityRepository<VozniParkKategorija>(PrimaryDbContext);
        public IRepository<VozniParkModel> VozniParkModel =>
            RepositoryProvider.GetEntityRepository<VozniParkModel>(PrimaryDbContext);

        public IRepository<VozniParkMarka> VozniParkMarka =>
            RepositoryProvider.GetEntityRepository<VozniParkMarka>(PrimaryDbContext);
        public IRepository<VozniParkStatus> VozniParkStatus =>
            RepositoryProvider.GetEntityRepository<VozniParkStatus>(PrimaryDbContext);
        public IRepository<VozniParkTip> VozniParkTip =>
            RepositoryProvider.GetEntityRepository<VozniParkTip>(PrimaryDbContext);
        public IRepository<VozniParkAlarm> VozniParkAlarm =>
            RepositoryProvider.GetEntityRepository<VozniParkAlarm>(PrimaryDbContext);
        public IRepository<VpAlarmGrupa> VpAlarmGrupa =>
            RepositoryProvider.GetEntityRepository<VpAlarmGrupa>(PrimaryDbContext);
        public IRepository<VpAlarmTip> VpAlarmTip =>
            RepositoryProvider.GetEntityRepository<VpAlarmTip>(PrimaryDbContext);
        public IRepository<VpTrosak> VpTrosak =>
            RepositoryProvider.GetEntityRepository<VpTrosak>(PrimaryDbContext);
        public IRepository<VpPopravke> VpPopravke =>
            RepositoryProvider.GetEntityRepository<VpPopravke>(PrimaryDbContext);
        public IRepository<VpPopravkeTip> VpPopravkeTip =>
            RepositoryProvider.GetEntityRepository<VpPopravkeTip>(PrimaryDbContext);
        public IRepository<VozniParkDnevnikTip> VozniParkDnevnikTip =>
            RepositoryProvider.GetEntityRepository<VozniParkDnevnikTip>(PrimaryDbContext);
        public IRepository<VozniParkDnevnik> VozniParkDnevnik =>
            RepositoryProvider.GetEntityRepository<VozniParkDnevnik>(PrimaryDbContext);
        public IRepository<VpOpremaGrupa> VpOpremaGrupa =>
            RepositoryProvider.GetEntityRepository<VpOpremaGrupa>(PrimaryDbContext);
        public IRepository<VpOpremaTip> VpOpremaTip =>
            RepositoryProvider.GetEntityRepository<VpOpremaTip>(PrimaryDbContext);
        public IRepository<VpOpremaPodTip> VpOpremaPodTip =>
            RepositoryProvider.GetEntityRepository<VpOpremaPodTip>(PrimaryDbContext);
        public IRepository<VpOprema> VpOprema =>
            RepositoryProvider.GetEntityRepository<VpOprema>(PrimaryDbContext);
        public IRepository<Sken> Sken =>
            RepositoryProvider.GetEntityRepository<Sken>(PrimaryDbContext);
        public IRepository<SkenStart> SkenStart =>
            RepositoryProvider.GetEntityRepository<SkenStart>(PrimaryDbContext);
        public IRepository<SkenRead> SkenRead =>
            RepositoryProvider.GetEntityRepository<SkenRead>(PrimaryDbContext);
        public IRepository<SkenPracenje> SkenPracenje =>
            RepositoryProvider.GetEntityRepository<SkenPracenje>(PrimaryDbContext);
        public IRepository<SkenTip> SkenTip =>
            RepositoryProvider.GetEntityRepository<SkenTip>(PrimaryDbContext);

        public IRepository<PosiljkaOtkupZbirni> PosiljkaOtkupZbirni =>
            RepositoryProvider.GetEntityRepository<PosiljkaOtkupZbirni>(PrimaryDbContext);

        public IRepository<PosiljkaOtkupZbirniStavka> PosiljkaOtkupZbirniStavka =>
            RepositoryProvider.GetEntityRepository<PosiljkaOtkupZbirniStavka>(PrimaryDbContext);
        public IRepository<BankaPazara> BankaPazara =>
            RepositoryProvider.GetEntityRepository<BankaPazara>(PrimaryDbContext);
        
        public IRepository<Banke> Banke =>
            RepositoryProvider.GetEntityRepository<Banke>(PrimaryDbContext);

       
        public IRepository<BankaPazaraSpecifikacija> BankaPazaraSpecifikacija =>
            RepositoryProvider.GetEntityRepository<BankaPazaraSpecifikacija>(PrimaryDbContext);
        public IRepository<Cekovi> Cekovi =>
            RepositoryProvider.GetEntityRepository<Cekovi>(PrimaryDbContext);
        public IRepository<CekoviProvizija> CekoviProvizija =>
            RepositoryProvider.GetEntityRepository<CekoviProvizija>(PrimaryDbContext);

        public IUgovorRepository Ugovor =>
            RepositoryProvider.GetRepository<IUgovorRepository>(
                dbContext => new UgovorRepository(PrimaryDbContext));
        //public IRepository<Ugovor> Ugovor =>
        //    RepositoryProvider.GetEntityRepository<Ugovor>(PrimaryDbContext);
        public IRepository<UgovorFakturisanje> UgovorFakturisanje =>
            RepositoryProvider.GetEntityRepository<UgovorFakturisanje>(PrimaryDbContext);
        public IRepository<UgovorKontaktBex> UgovorKontaktBex =>
            RepositoryProvider.GetEntityRepository<UgovorKontaktBex>(PrimaryDbContext);
        public IRepository<UgovorNacinNaplate> UgovorNacinNaplate  =>
            RepositoryProvider.GetEntityRepository<UgovorNacinNaplate>(PrimaryDbContext);
        public IRepository<UgovorNapomena> UgovorNapomena =>
            RepositoryProvider.GetEntityRepository<UgovorNapomena>(PrimaryDbContext);
        public IRepository<UgovorNapomenaTip> UgovorNapomenaTip =>
            RepositoryProvider.GetEntityRepository<UgovorNapomenaTip>(PrimaryDbContext);
        public IRepository<UgovorObracunCena> UgovorObracunCena =>
            RepositoryProvider.GetEntityRepository<UgovorObracunCena>(PrimaryDbContext);
        public IRepository<UgovorPovlascenaCenaTip> UgovorPovlascenaCenaTip =>
            RepositoryProvider.GetEntityRepository<UgovorPovlascenaCenaTip>(PrimaryDbContext);
        public IRepository<UgovorVip> UgovorVip =>
            RepositoryProvider.GetEntityRepository<UgovorVip>(PrimaryDbContext);
        public IRepository<KovertaZaDokument> KovertaZaDokument =>
            RepositoryProvider.GetEntityRepository<KovertaZaDokument>(PrimaryDbContext);
        public IRepository<KovertaZaOtkupninu> KovertaZaOtkupninu =>
            RepositoryProvider.GetEntityRepository<KovertaZaOtkupninu>(PrimaryDbContext);
        public IRepository<KurirskaListaDostava> KurirskaListaDostava =>
            RepositoryProvider.GetEntityRepository<KurirskaListaDostava>(PrimaryDbContext);
        public IRepository<SpisakDostava> SpisakDostava =>
            RepositoryProvider.GetEntityRepository<SpisakDostava>(PrimaryDbContext);
        public IRepository<SpisakOtkupnina> SpisakOtkupnina =>
            RepositoryProvider.GetEntityRepository<SpisakOtkupnina>(PrimaryDbContext);
        public ICenovnikRepository Cenovnik =>
            RepositoryProvider.GetRepository<ICenovnikRepository>(
                dbContext => new CenovnikRepository(PrimaryDbContext));
        public IRepository<Cene> Cene =>
            RepositoryProvider.GetEntityRepository<Cene>(PrimaryDbContext);
        public IRepository<UgovorRpt> UgovorRpt =>
            RepositoryProvider.GetEntityRepository<UgovorRpt>(PrimaryDbContext);
        public IRepository<UgovorRptCenovnik1> UgovorRptCenovnik1 =>
            RepositoryProvider.GetEntityRepository<UgovorRptCenovnik1>(PrimaryDbContext);
        public IRepository<UgovorRptCenovnik2> UgovorRptCenovnik2 =>
            RepositoryProvider.GetEntityRepository<UgovorRptCenovnik2>(PrimaryDbContext);


        public IRepository<StatistikaDan> StatistikaDan =>
            RepositoryProvider.GetEntityRepository<StatistikaDan>(PrimaryDbContext);
        public IRepository<StatistikaSat> StatistikaSat =>
            RepositoryProvider.GetEntityRepository<StatistikaSat>(PrimaryDbContext);
        public IRepository<StatistikaPorekloPosiljke> StatistikaPorekloPosiljke =>
            RepositoryProvider.GetEntityRepository<StatistikaPorekloPosiljke>(PrimaryDbContext);
        public IRepository<StatistikaRazlogBrisanjaPosiljke> StatistikaRazlogBrisanjaPosiljke =>
            RepositoryProvider.GetEntityRepository<StatistikaRazlogBrisanjaPosiljke>(PrimaryDbContext);

        public IRepository<KalendarPlaner> KalendarPlaner =>
            RepositoryProvider.GetEntityRepository<KalendarPlaner>(PrimaryDbContext);

       
        public IPrijavaRepository PrijavaReklamacijaZalba =>
            RepositoryProvider.GetRepository<IPrijavaRepository>(
                dbContext => new PrijavaRepository(PrimaryDbContext));
        public IRepository<PrijavaReklamacijaZalbaLog> PrijavaReklamacijaZalbaLog =>
            RepositoryProvider.GetEntityRepository<PrijavaReklamacijaZalbaLog>(PrimaryDbContext);
        public IRepository<PrijavaTip> PrijavaTip =>
            RepositoryProvider.GetEntityRepository<PrijavaTip>(PrimaryDbContext);
        public IRepository<PrijavaPodTip> PrijavaPodTip =>
            RepositoryProvider.GetEntityRepository<PrijavaPodTip>(PrimaryDbContext);
        public IRepository<PrijavaNacin> PrijavaNacin =>
            RepositoryProvider.GetEntityRepository<PrijavaNacin>(PrimaryDbContext);
        public IRepository<PrijavaNapomena> PrijavaNapomena =>
            RepositoryProvider.GetEntityRepository<PrijavaNapomena>(PrimaryDbContext);
        public IRepository<PrijavaStatus> PrijavaStatus =>
            RepositoryProvider.GetEntityRepository<PrijavaStatus>(PrimaryDbContext);

        public IRepository<Ticket> Ticket =>
            RepositoryProvider.GetEntityRepository<Ticket>(PrimaryDbContext);
        public IRepository<TicketKategorija> TicketKategorija =>
            RepositoryProvider.GetEntityRepository<TicketKategorija>(PrimaryDbContext);
        public IRepository<TicketPost> TicketPost =>
            RepositoryProvider.GetEntityRepository<TicketPost>(PrimaryDbContext);
        public IRepository<TicketPrimalac> TicketPrimalac =>
            RepositoryProvider.GetEntityRepository<TicketPrimalac>(PrimaryDbContext);
        public IRepository<TicketPrimalacTip> TicketPrimalacTip =>
            RepositoryProvider.GetEntityRepository<TicketPrimalacTip>(PrimaryDbContext);
        public IRepository<TicketStatus> TicketStatus =>
            RepositoryProvider.GetEntityRepository<TicketStatus>(PrimaryDbContext);

        public IRepository<PravniPostupak> PravniPostupak =>
            RepositoryProvider.GetEntityRepository<PravniPostupak>(PrimaryDbContext);
        public IRepository<PPvrsta> PPvrsta =>
            RepositoryProvider.GetEntityRepository<PPvrsta>(PrimaryDbContext);
        public IRepository<PPoblast> PPoblast =>
            RepositoryProvider.GetEntityRepository<PPoblast>(PrimaryDbContext);

        public IUowCommandResult SubmitChanges() =>
            UowCommandResultFactory.Invoke(SaveChanges);

        private int SaveChanges()
        {
            var submitChanges = 0;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                submitChanges = DbContexts.ToList()
                    .Sum(dbcontext => dbcontext.SaveChanges());

                transactionScope.Complete();
            }

            return submitChanges;
        }

        private void ConfigureDbContexts()
        {
            if (DbContexts != null)
            {
                foreach (var dbContext in DbContexts)
                { dbContext.Configuration.ProxyCreationEnabled = false; }
            }
        }

        public void Dispose()
        {
            if (DbContexts != null)
            {
                foreach (var dbContext in DbContexts)
                { dbContext.Dispose(); }
            }
        }

        private IUowCommandResultFactory UowCommandResultFactory { get; }
        private static ISqlLogger SqlLogger { get; }
        private IList<DbContext> DbContexts { get; set; }
        private DbContext PrimaryDbContext { get; }
        private IRepositoryProvider RepositoryProvider { get; }

        
    }
}
