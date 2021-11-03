using AspNet.DAL.EF.Models.Security;
using Bex.DAL.EF.Common;
using Bex.DAL.EF.Contexts;
using Bex.DAL.EF.Models;
using Bex.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Bex.DAL.EF.Contexts.Main
{
    public class BexMainDbContext : BaseDbContext
    {
        static BexMainDbContext()
        {
            //Database.SetInitializer<ClubDbContext>(null); // has same effect
            // Database.SetInitializer<BexMainDbContext>(new NullDatabaseInitializer<BexMainDbContext>());
            Database.SetInitializer(
                 new NullDatabaseInitializer<BexMainDbContext>());
        }

        public BexMainDbContext() : this(new SaveChangesResolver())
        { }
        public BexMainDbContext(ISaveChangesResolver saveChangesResolver)
            : base(saveChangesResolver)
        { }



        //public static BexMainDbContext Create()
        //{ return new BexMainDbContext(); }



        //public BexMainDbContext() : base("name=BexMainDbContext")
        //{
        //}
        public DbSet<Novosti> Novosti { get; set; }
        public DbSet<PAK> PAKs { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Reon> Reons { get; set; }
        public DbSet<Reoncic> Reoncics { get; set; }
       
        public DbSet<Street> Streets { get; set; }

        public DbSet<StatKasnjenja> StatKasnjenja { get; set; }

        public DbSet<Zip> Zips { get; set; }
        public System.Data.Entity.DbSet<Opstina> Opstina { get; set; }
        public System.Data.Entity.DbSet<ReonTip> ReonTip { get; set; }

        public DbSet<DimenzijeNav> DimenzijeNav { get; set; }

        public DbSet<Sektor> Sektors { get; set; }

        public System.Data.Entity.DbSet<Kontakt> Kontakt { get; set; }

        public System.Data.Entity.DbSet<KontaktFizickoLice> KontaktFizickoLice { get; set; }

        public System.Data.Entity.DbSet<KontaktPravnoLice> KontaktPravnaLica { get; set; }

        public System.Data.Entity.DbSet<Zaposleni> Zaposleni { get; set; }
        public System.Data.Entity.DbSet<EvidencijaVozaca> EvidencijaVozaca { get; set; }

        public System.Data.Entity.DbSet<KontaktAdresa> KontaktAdresa { get; set; }
        public System.Data.Entity.DbSet<KontaktEmail> KontaktEmail { get; set; }
        public System.Data.Entity.DbSet<KontaktTelefon> KontaktTelefon { get; set; }
        public System.Data.Entity.DbSet<KontaktZiroRacun> KontaktZiroRacun { get; set; }
        public System.Data.Entity.DbSet<KontaktDelatnost> KontaktDelatnost { get; set; }

        public System.Data.Entity.DbSet<KontaktUloga> KontaktUloga { get; set; }
        public System.Data.Entity.DbSet<KontaktUlogaTip> KontaktUlogaTip { get; set; }
        public System.Data.Entity.DbSet<ZaposleniZanimanje> ZaposleniZanimanje { get; set; }

        public System.Data.Entity.DbSet<ZaposleniNapomena> ZaposleniNapomena { get; set; }

        public System.Data.Entity.DbSet<ZaposleniDan> ZaposleniDan { get; set; }
        public System.Data.Entity.DbSet<ZaposleniDanStatus> ZaposleniDanStatus { get; set; }

        public System.Data.Entity.DbSet<PutniNalog> PutniNalog { get; set; }
        public System.Data.Entity.DbSet<PutniNalogTip> PutniNalogTip { get; set; }
        public System.Data.Entity.DbSet<VozniPark> VozniPark { get; set; }
        public System.Data.Entity.DbSet<Gorivo> Gorivo { get; set; }

        public System.Data.Entity.DbSet<GorivoKartica> GorivoKartica { get; set; }
        public System.Data.Entity.DbSet<GorivoPumpa> GorivoPumpa { get; set; }
        public System.Data.Entity.DbSet<GorivoTocenje> GorivoTocenje { get; set; }
        public System.Data.Entity.DbSet<VozniParkKarakteristike> VozniParkKarakteristike { get; set; }
        public System.Data.Entity.DbSet<VozniParkKaroserija> VozniParkKaroserija { get; set; }
        public System.Data.Entity.DbSet<VozniParkPodKaroserija> VozniParkPodKaroserija { get; set; }
        public System.Data.Entity.DbSet<VozniParkBoja> VozniParkBoja { get; set; }
        public System.Data.Entity.DbSet<VozniParkMenjac> VozniParkMenjac { get; set; }
        public System.Data.Entity.DbSet<VozniParkKategorija> VozniParkKategorija { get; set; }
        public System.Data.Entity.DbSet<VozniParkModel> VozniParkModel { get; set; }
        public System.Data.Entity.DbSet<VozniParkMarka> VozniParkMarka { get; set; }
        public System.Data.Entity.DbSet<VozniParkStatus> VozniParkStatus { get; set; }
        public System.Data.Entity.DbSet<VozniParkTip> VozniParkTip { get; set; }
        public System.Data.Entity.DbSet<VozniParkAlarm> VozniParkAlarm { get; set; }
        public System.Data.Entity.DbSet<VpAlarmGrupa> VpAlarmGrupa { get; set; }
        public System.Data.Entity.DbSet<VpAlarmTip> VpAlarmTip { get; set; }
        public System.Data.Entity.DbSet<VpTrosak> VpTrosak { get; set; }
        public System.Data.Entity.DbSet<VpPopravke> VpPopravke { get; set; }
        public System.Data.Entity.DbSet<VpPopravkeTip> VpPopravkeTip { get; set; }

        public System.Data.Entity.DbSet<VozniParkDnevnik> VozniParkDnevnik { get; set; }
        public System.Data.Entity.DbSet<VozniParkDnevnikTip> VozniParkDnevnikTip { get; set; }
        public System.Data.Entity.DbSet<VpOpremaGrupa> VpOpremaGrupa { get; set; }
        public System.Data.Entity.DbSet<VpOpremaTip> VpOpremaTip { get; set; }
        public System.Data.Entity.DbSet<VpOpremaPodTip> VpOpremaPodTip { get; set; }
        public System.Data.Entity.DbSet<VpOprema> VpOprema { get; set; }
        public System.Data.Entity.DbSet<KurirZaduzenje> KurirZaduzenje { get; set; }

        public System.Data.Entity.DbSet<KurirRazduzenjeSpecifikacija> KurirRazduzenjeSpecifikacija { get; set; }

        public System.Data.Entity.DbSet<KurirRazduzenje> KurirRazduzenje { get; set; }

        public System.Data.Entity.DbSet<NapomenaPosiljka> NapomenaPosiljka { get; set; }

        public System.Data.Entity.DbSet<NapomenaPosiljkaPodTip> NapomenaPosiljkaPodTip { get; set; }


        public System.Data.Entity.DbSet<Posiljka> Posiljka { get; set; }
        public System.Data.Entity.DbSet<PosiljkaStatus> PosiljkaStatus { get; set; }
        public System.Data.Entity.DbSet<PosiljkaVrsta> PosiljkaVrsta { get; set; }
        public System.Data.Entity.DbSet<PosiljkaKategorija> PosiljkaKategorija { get; set; }
        public System.Data.Entity.DbSet<PosiljkaSadrzaj> PosiljkaSadrzaj { get; set; }
        public System.Data.Entity.DbSet<PosiljkaZadatak> PosiljkaZadatak { get; set; }
        public System.Data.Entity.DbSet<PosiljkaUsluga> PosiljkaUsluga { get; set; }
        public System.Data.Entity.DbSet<PosiljkaUslugaTip> PosiljkaUslugaTip { get; set; }
        public System.Data.Entity.DbSet<PosiljkaPlacanje> PosiljkaPlacanje { get; set; }
        public System.Data.Entity.DbSet<PosiljkaPlacanjeTip> PosiljkaPlacanjeTip { get; set; }
        public System.Data.Entity.DbSet<PosiljkaNapomena> PosiljkaNapomena { get; set; }
        public System.Data.Entity.DbSet<PosiljkaNapomenaTip> PosiljkaNapomenaTip { get; set; }
        public System.Data.Entity.DbSet<PosiljkaObrisana> PosiljkaObrisana { get; set; }
        public System.Data.Entity.DbSet<Paket> Paket { get; set; }
        public System.Data.Entity.DbSet<PaketTip> PaketTip { get; set; }
        public System.Data.Entity.DbSet<PaketZadatak> PaketZadatak { get; set; }
        public System.Data.Entity.DbSet<Zona> Zona { get; set; }
        public System.Data.Entity.DbSet<ZonaTip> ZonaTip { get; set; }
        public System.Data.Entity.DbSet<ZonaPodTip> ZonaPodTip { get; set; }
        public System.Data.Entity.DbSet<Sken> Sken { get; set; }
        public System.Data.Entity.DbSet<SkenStart> SkenStart { get; set; }
        public System.Data.Entity.DbSet<SkenPracenje> SkenPracenje { get; set; }
        public System.Data.Entity.DbSet<SkenRead> SkenRead { get; set; }
        public System.Data.Entity.DbSet<SkenTip> SkenTip { get; set; }
        public System.Data.Entity.DbSet<PosiljkaOtkupZbirni> PosiljkaOtkupZbirni { get; set; }
        public System.Data.Entity.DbSet<PosiljkaOtkupZbirniStavka> PosiljkaOtkupZbirniStavka { get; set; }
        public System.Data.Entity.DbSet<BankaPazara> BankaPazara { get; set; }
        public System.Data.Entity.DbSet<BankaPazaraSpecifikacija> BankaPazaraSpecifikacija { get; set; }
        public System.Data.Entity.DbSet<Banke> Banke{ get; set; }
        public System.Data.Entity.DbSet<Cekovi> Cekovi { get; set; }
        public System.Data.Entity.DbSet<CekoviProvizija> CekoviProvizija { get; set; }
        public System.Data.Entity.DbSet<Ugovor> Ugovor { get; set; }
        public System.Data.Entity.DbSet<UgovorFakturisanje> UgovorFakturisanje { get; set; }
        public System.Data.Entity.DbSet<UgovorKontaktBex> UgovorKontaktBex { get; set; }
        public System.Data.Entity.DbSet<UgovorNacinNaplate> UgovorNacinNaplate { get; set; }
        public System.Data.Entity.DbSet<UgovorNapomena> UgovorNapomena { get; set; }
        public System.Data.Entity.DbSet<UgovorNapomenaTip> UgovorNapomenaTip { get; set; }
        public System.Data.Entity.DbSet<UgovorObracunCena> UgovorObracunCena { get; set; }
        public System.Data.Entity.DbSet<UgovorPovlascenaCenaTip> UgovorPovlascenaCenaTip { get; set; }
        public System.Data.Entity.DbSet<UgovorVip> UgovorVip { get; set; }
        public System.Data.Entity.DbSet<Cenovnik> Cenovnik { get; set; }
        public System.Data.Entity.DbSet<StatistikaSat> StatistikaSat { get; set; }
        public System.Data.Entity.DbSet<StatistikaDan> StatistikaDan { get; set; }
        public System.Data.Entity.DbSet<StatistikaPorekloPosiljke> StatistikaPorekloPosiljke { get; set; }
        public System.Data.Entity.DbSet<StatistikaRazlogBrisanjaPosiljke> StatistikaRazlogBrisanjaPosiljke { get; set; }

        public System.Data.Entity.DbSet<KorisniciPrograma> KorisniciPrograma { get; set; }
        public System.Data.Entity.DbSet<KorisniciProgramaClaims> KorisniciProgramaClaims { get; set; }

        public System.Data.Entity.DbSet<KorisniciProgramaClaimsRoles> KorisniciProgramaClaimsRoles { get; set; }

        public System.Data.Entity.DbSet<KalendarPlaner> KalendarPlaner { get; set; }

        public System.Data.Entity.DbSet<PrijavaReklamacijaZalba> PrijavaReklamacijaZalba { get; set; }
        public System.Data.Entity.DbSet<PrijavaReklamacijaZalbaLog> PrijavaReklamacijaZalbaLog { get; set; }
        public System.Data.Entity.DbSet<PrijavaTip> PrijavaTip { get; set; }
        public System.Data.Entity.DbSet<PrijavaPodTip> PrijavaPodTip { get; set; }
        public System.Data.Entity.DbSet<PrijavaNacin> PrijavaNacin { get; set; }
        public System.Data.Entity.DbSet<PrijavaNapomena> PrijavaNapomena { get; set; }
        public System.Data.Entity.DbSet<PrijavaStatus> PrijavaStatus { get; set; }

        public System.Data.Entity.DbSet<Ticket> Ticket { get; set; }
        public System.Data.Entity.DbSet<TicketKategorija> TicketKategorija { get; set; }
        public System.Data.Entity.DbSet<TicketPost> TicketPost { get; set; }
        public System.Data.Entity.DbSet<TicketPrimalac> TicketPrimalac { get; set; }
        public System.Data.Entity.DbSet<TicketPrimalacTip> TicketPrimalacTip { get; set; }
        public System.Data.Entity.DbSet<TicketStatus> TicketStatus { get; set; }
        public System.Data.Entity.DbSet<Trebovanje> Trebovanje { get; set; }
        public System.Data.Entity.DbSet<TrebovanjeStavke> TrebovanjeStavke { get; set; }
        public System.Data.Entity.DbSet<Artikli> Artikli { get; set; }
        public System.Data.Entity.DbSet<ArtikliGrupa> ArtikliGrupa { get; set; }
        public System.Data.Entity.DbSet<ArtikliVrsta> ArtikliVrsta { get; set; }
        public System.Data.Entity.DbSet<MagacinSpisak> MagacinSpisak { get; set; }
        public System.Data.Entity.DbSet<Lager> Lager { get; set; }
        public System.Data.Entity.DbSet<TPtermini> TPtermini { get; set; }
        public System.Data.Entity.DbSet<TPstatusTermina> TPstatusTermina { get; set; }
        public System.Data.Entity.DbSet<TPlokacije> TPlokacije { get; set; }
        public System.Data.Entity.DbSet<TPocitavanja> TPocitavanja { get; set; }
        public System.Data.Entity.DbSet<StetaTip> StetaTip { get; set; }
        public System.Data.Entity.DbSet<StetaKategorija> StetaKategorija { get; set; }
        public System.Data.Entity.DbSet<VozniParkSteta> Steta { get; set; }

        public System.Data.Entity.DbSet<Gallery> Gallery { get; set; }
        public System.Data.Entity.DbSet<WebFiles> WebFiles { get; set; }
        public System.Data.Entity.DbSet<WebFilesTip> WebFilesTip { get; set; }

        public System.Data.Entity.DbSet<Firma> Firma { get; set; }
        public System.Data.Entity.DbSet<FirmaVP> FirmaVP { get; set; }

        public System.Data.Entity.DbSet<PravniPostupak> PravniPostupak { get; set; }
        public System.Data.Entity.DbSet<PPvrsta> PPvrsta { get; set; }
        public System.Data.Entity.DbSet<PPoblast> PPoblast { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new NovostiConfiguration());
            modelBuilder.Configurations.Add(new PakConfiguration());
            modelBuilder.Configurations.Add(new PlaceConfiguration());
            modelBuilder.Configurations.Add(new RegionConfiguration());
            modelBuilder.Configurations.Add(new ReoncicConfiguration());
            modelBuilder.Configurations.Add(new ReonConfiguration());
            modelBuilder.Configurations.Add(new OpstinaConfiguration());
            modelBuilder.Configurations.Add(new ReonTipConfiguration());
            modelBuilder.Configurations.Add(new StreetConfiguration());
            modelBuilder.Configurations.Add(new ZipConfiguration());
            modelBuilder.Configurations.Add(new DimenzijeNavConfiguration());

            modelBuilder.Configurations.Add(new KontaktConfiguration());
            modelBuilder.Configurations.Add(new KontaktPravnoLiceConfiguration());
            modelBuilder.Configurations.Add(new KontaktFizickoLiceConfiguration());
            modelBuilder.Configurations.Add(new KontaktUlogaConfiguration());
            modelBuilder.Configurations.Add(new KontaktUlogaTipConfiguration());
            modelBuilder.Configurations.Add(new KontaktAdresaConfiguration());
            modelBuilder.Configurations.Add(new KontaktZiroRacunConfiguration());
            modelBuilder.Configurations.Add(new KontaktEmailConfiguration());
            modelBuilder.Configurations.Add(new KontaktTelefonConfiguration());
            modelBuilder.Configurations.Add(new KontaktDelatnostConfiguration());


            modelBuilder.Configurations.Add(new SektorConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniConfiguration());
            modelBuilder.Configurations.Add(new EvidencijaVozacaConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniZanimanjeConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniRazlogOtkazaConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniStatusConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniStrucnaSpremaConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniRadnoMestoConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniOsnovOsiguranjaConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniProgramZaposlenjaConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniSlavaConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniPlataConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniNapomenaConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniDanConfiguration());
            modelBuilder.Configurations.Add(new ZaposleniDanStatusConfiguration());
            modelBuilder.Configurations.Add(new KorisniciProgramaConfiguration());
            modelBuilder.Configurations.Add(new KorisniciProgramaClaimsConfiguration());
            modelBuilder.Configurations.Add(new KorisniciProgramaClaimsRolesConfiguration());

            modelBuilder.Configurations.Add(new PutniNalogConfiguration());
            modelBuilder.Configurations.Add(new PutniNalogTipConfiguration());
            modelBuilder.Configurations.Add(new VozniParkConfiguration());
            //modelBuilder.Ignore<VozniPark>();
            modelBuilder.Configurations.Add(new GorivoConfiguration());
            modelBuilder.Configurations.Add(new GorivoKarticaConfiguration());
            modelBuilder.Configurations.Add(new GorivoPumpaConfiguration());
            modelBuilder.Configurations.Add(new GorivoTocenjeConfiguration());
            modelBuilder.Configurations.Add(new VozniParkTipConfiguration());
            modelBuilder.Configurations.Add(new VozniParkMenjacConfiguration());
            modelBuilder.Configurations.Add(new VozniParkKategorijaConfiguration());
            modelBuilder.Configurations.Add(new VozniParkModelConfiguration());
            modelBuilder.Configurations.Add(new VozniParkMarkaConfiguration());
            modelBuilder.Configurations.Add(new VozniParkBojaConfiguration());
            modelBuilder.Configurations.Add(new VozniParkKarakteristikeConfiguration());
            modelBuilder.Configurations.Add(new VozniParkKaroserijaConfiguration());
            modelBuilder.Configurations.Add(new VozniParkPodKaroserijaConfiguration());
            modelBuilder.Configurations.Add(new VozniParkStatusConfiguration());
            modelBuilder.Configurations.Add(new VozniParkAlarmConfiguration());
            modelBuilder.Configurations.Add(new VpAlarmGrupaConfiguration());
            modelBuilder.Configurations.Add(new VpAlarmTipConfiguration());
            modelBuilder.Configurations.Add(new VpTrosakConfiguration());
            modelBuilder.Configurations.Add(new VpPopravkeConfiguration());
            modelBuilder.Configurations.Add(new VpPopravkeTipConfiguration());
            modelBuilder.Configurations.Add(new VozniParkDnevnikConfiguration());
            modelBuilder.Configurations.Add(new VozniParkDnevnikTipConfiguration());
            modelBuilder.Configurations.Add(new VpOpremaGrupaConfiguration());
            modelBuilder.Configurations.Add(new VpOpremaTipConfiguration());
            modelBuilder.Configurations.Add(new VpOpremaPodTipConfiguration());
            modelBuilder.Configurations.Add(new VpOpremaConfiguration());

            modelBuilder.Configurations.Add(new KurirZaduzenjeConfiguration());
            modelBuilder.Configurations.Add(new KurirRazduzenjeConfiguration());
            modelBuilder.Configurations.Add(new KurirRazduzenjeSpecifikacijaConfiguration());
            modelBuilder.Configurations.Add(new NapomenaPosiljkaConfiguration());
            modelBuilder.Configurations.Add(new NapomenaPosiljkaPodTipConfiguration());

            modelBuilder.Configurations.Add(new PosiljkaConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaStatusConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaVrstaConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaKategorijaConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaSadrzajConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaZadatakConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaUslugaConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaUslugaTipConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaPlacanjeConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaPlacanjeTipConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaNapomenaConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaNapomenaTipConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaObrisanaConfiguration());
            modelBuilder.Configurations.Add(new PaketConfiguration());
            modelBuilder.Configurations.Add(new PaketTipConfiguration());
            modelBuilder.Configurations.Add(new PaketZadatakConfiguration());
            modelBuilder.Configurations.Add(new ZonaConfiguration());
            modelBuilder.Configurations.Add(new ZonaTipConfiguration());
            modelBuilder.Configurations.Add(new ZonaPodTipConfiguration());


            modelBuilder.Configurations.Add(new SkenConfiguration());
            modelBuilder.Configurations.Add(new SkenStartConfiguration());
            modelBuilder.Configurations.Add(new SkenPracenjeConfiguration());
            modelBuilder.Configurations.Add(new SkenReadConfiguration());
            modelBuilder.Configurations.Add(new SkenTipConfiguration());

            modelBuilder.Configurations.Add(new PosiljkaOtkupZbirniConfiguration());
            modelBuilder.Configurations.Add(new PosiljkaOtkupZbirniStavkaConfiguration());

            modelBuilder.Configurations.Add(new BankaPazaraConfiguration());
            modelBuilder.Configurations.Add(new BankaPazaraSpecifikacijaConfiguration());
            modelBuilder.Configurations.Add(new BankeConfiguration());
            modelBuilder.Configurations.Add(new CekoviConfiguration());
            modelBuilder.Configurations.Add(new CekoviProvizijaConfiguration());


            modelBuilder.Configurations.Add(new FirmaConfiguration());
            modelBuilder.Configurations.Add(new FirmaVPConfiguration());

            modelBuilder.Configurations.Add(new StatKasnjenjaConfiguration());

            modelBuilder.Configurations.Add(new UgovorConfiguration());
            modelBuilder.Configurations.Add(new UgovorFakturisanjeConfiguration());
            modelBuilder.Configurations.Add(new UgovorKontaktBexConfiguration());
            modelBuilder.Configurations.Add(new UgovorNacinNaplateConfiguration());
            modelBuilder.Configurations.Add(new UgovorNapomenaConfiguration());
            modelBuilder.Configurations.Add(new UgovorNapomenaTipConfiguration());
            modelBuilder.Configurations.Add(new UgovorObracunCenaConfiguration());
            modelBuilder.Configurations.Add(new UgovorPovlascenaCenaTipConfiguration());
            modelBuilder.Configurations.Add(new UgovorVipConfiguration());

            modelBuilder.Configurations.Add(new KovertaZaDokumentConfiguration());
            modelBuilder.Configurations.Add(new KovertaZaOtkupninuConfiguration());
            modelBuilder.Configurations.Add(new KurirskaListaDostavaConfiguration());
            modelBuilder.Configurations.Add(new SpisakDostavaConfiguration());
            modelBuilder.Configurations.Add(new SpisakOtkupninaConfiguration());

            modelBuilder.Configurations.Add(new CenovnikConfiguration());
            modelBuilder.Configurations.Add(new CeneConfiguration());

            modelBuilder.Configurations.Add(new UgovorRptConfiguration());
            modelBuilder.Configurations.Add(new UgovorRptCenovnik1Configuration());
            modelBuilder.Configurations.Add(new UgovorRptCenovnik2Configuration());

            modelBuilder.Configurations.Add(new StatistikaDanConfiguration());
            modelBuilder.Configurations.Add(new StatistikaSatConfiguration());
            modelBuilder.Configurations.Add(new StatistikaPorekloPosiljkeConfiguration());
            modelBuilder.Configurations.Add(new StatistikaRazlogBrisanjaPosiljkeConfiguration());

            modelBuilder.Configurations.Add(new KalendarPlanerConfiguration());

            modelBuilder.Configurations.Add(new PrijavaReklamacijaZalbaConfiguration());
            modelBuilder.Configurations.Add(new PrijavaReklamacijaZalbaLogConfiguration());
            modelBuilder.Configurations.Add(new PrijavaTipConfiguration());
            modelBuilder.Configurations.Add(new PrijavaPodTipConfiguration());
            modelBuilder.Configurations.Add(new PrijavaNacinConfiguration());
            modelBuilder.Configurations.Add(new PrijavaNapomenaConfiguration());
            modelBuilder.Configurations.Add(new PrijavaStatusConfiguration());

            modelBuilder.Configurations.Add(new TrebovanjeConfiguration());
            modelBuilder.Configurations.Add(new TrebovanjeStavkeConfiguration());
            modelBuilder.Configurations.Add(new ArtikliConfiguration());
            modelBuilder.Configurations.Add(new ArtikliGrupaConfiguration());
            modelBuilder.Configurations.Add(new ArtikliVrstaConfiguration());
            modelBuilder.Configurations.Add(new MagacinSpisakConfiguration());
            modelBuilder.Configurations.Add(new LagerConfiguration());
            modelBuilder.Configurations.Add(new TPterminiConfiguration());
            modelBuilder.Configurations.Add(new TPlokacijeConfiguration());
            modelBuilder.Configurations.Add(new TPstatusTerminaConfiguration());
            modelBuilder.Configurations.Add(new TPocitavanjaConfiguration());
            modelBuilder.Configurations.Add(new StetaTipConfiguration());
            modelBuilder.Configurations.Add(new StetaKategorijaConfiguration());
            modelBuilder.Configurations.Add(new StetaConfiguration());

            modelBuilder.Configurations.Add(new GalleryConfiguration());
            modelBuilder.Configurations.Add(new WebFilesConfiguration());
            modelBuilder.Configurations.Add(new WebFilesTipConfiguration());

            modelBuilder.Configurations.Add(new TicketConfiguration());
            modelBuilder.Configurations.Add(new TicketKategorijaConfiguration());
            modelBuilder.Configurations.Add(new TicketPostConfiguration());
            modelBuilder.Configurations.Add(new TicketPrimalacConfiguration());
            modelBuilder.Configurations.Add(new TicketPrimalacTipConfiguration());
            modelBuilder.Configurations.Add(new TicketStatusConfiguration());

            modelBuilder.Configurations.Add(new PravniPostupakConfiguration());
            modelBuilder.Configurations.Add(new PPoblastConfiguration());
            modelBuilder.Configurations.Add(new PPvrstaConfiguration());


            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            //modelBuilder.Entity<SkenStart>().Ignore(i => i.User);
            //modelBuilder.Entity<Region>().Ignore(i => i.User);
            //modelBuilder.Entity<PosiljkaOtkupZbirni>().Ignore(i => i.User);
            //modelBuilder.Entity<PosiljkaObrisana>().Ignore(i => i.UserObrisao);
            //modelBuilder.Entity<SkenRead>().Ignore(i => i.User);
            //modelBuilder.Entity<KurirZaduzenje>().Ignore(i => i.User);
            //modelBuilder.Entity<PaketZadatak>().Ignore(i => i.User);
            //modelBuilder.Entity<NapomenaPosiljka>().Ignore(i => i.User);
            //modelBuilder.Entity<Posiljka>().Ignore(i => i.UserUneo);
            //modelBuilder.Entity<PosiljkaZadatak>().Ignore(i => i.User);
            //modelBuilder.Entity<Kontakt>().Ignore(i => i.User);
            //modelBuilder.Entity<KurirRazduzenje>().Ignore(i => i.UserUnos);
            //modelBuilder.Entity<KurirRazduzenje>().Ignore(i => i.UserKurir);
            //modelBuilder.Entity<KurirRazduzenjeSpecifikacija>().Ignore(i => i.User);
        }
        public override int SaveChanges()
        {
            ShouldValidateOnSaveChanges = true;

            return base.SaveChanges();
        }
    }
}
