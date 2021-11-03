namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class UgovorKontaktBex

    {
       
        public int Id { get; set; }
        public int SubId { get; set; }
        public int? UgovorId { get; set; }
        public int? nSubId { get; set; }
        public int? tSubId { get; set; }
        public int? cSubId { get; set; }
        public int? KategorijaId { get; set; }
        public int? KategorijaSpecijalnaID { get; set; }
        public int? KategorijaZaVrstuPoFakturiId { get; set; }
        public int? VrstaId { get; set; }
        public int? SadrzajId { get; set; }
        public bool OtkupUvekNaRacun { get; set; }
        public bool Otpremnica { get; set; }
        public bool LicnoUrucenje { get; set; }
        public bool Povratnica { get; set; }
        public bool PlacenOdgovor { get; set; }
        public int? NajavaMinuta { get; set; }
        public int? NajavaMinutaDostava { get; set; }
        public TimeSpan VremePreuzimanjaOd { get; set; }
        public TimeSpan VremePreuzimanjaDo { get; set; }
        public TimeSpan VremeDostaveOd { get; set; }
        public TimeSpan VremeDostaveDo { get; set; }
        public int? NacinPlacanjaId { get; set; }
        public int? PlatilacUgovorId { get; set; }
        public bool PrimalacJeFizickoLice { get; set; }
        public string SkracenoStart { get; set; }
        public int? SkracenoEnd { get; set; }
        public bool PrimaCek { get; set; }
        public string Napmena { get; set; }
        public string NapomenaZaAdresnicu { get; set; }
        public string NapomenaZaPreuzimanje { get; set; }
        public string NapomenaZaDostavu { get; set; }
        public bool BexImportuje { get; set; }
        public bool AutomatskaValidacijaImport { get; set; }
        public int FormatDatumaImportId { get; set; }
        public bool ZabranjenoBrisanjePosiljki { get; set; }
        public int? VrednostMaxPosiljke { get; set; }
        public bool AdresnicaSakrijKesPosiljalac { get; set; }
        public bool BezPrikazaUkupnogBrojaPaketa { get; set; }
        public bool NeSlatiSMSnajavaDostave { get; set; }

        public virtual Ugovor Ugovor { get; set; }
    }
}
