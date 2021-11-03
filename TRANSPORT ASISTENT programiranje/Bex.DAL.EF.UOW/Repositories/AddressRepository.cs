using Bex.Common;
using Bex.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Bex.ViewModels;

namespace Bex.DAL.EF.UOW
{
    public class AddressRepository : BaseRepository<PAK>, IAddressRepository
    {
        public AddressRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public AddressRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }

        public IEnumerable<PAK> GetAddressAutocompliteData(string searchQuery)
        {
            var addressData = DataSet.Include(u => u.Street).Include(m => m.Place)
                                     .Where(x => (x.Street.StreetName.ToUpper() + " - " + x.Place.PlaceName.ToUpper()).Contains(searchQuery.ToUpper()));

            return addressData;
        }



        public IEnumerable<PAK> GetSearchAddressData(string searchTerms)
        {
            var addressData = DataSet.Include(u => u.Street)
                                            .Include(r => r.Reon)
                                            .Include(re => re.Reoncic)
                                            .Include(m => m.Place)
                                            .Include(reg => reg.Region).AsQueryable();

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            int searchColumnPak = 0;
            string searchColumnUlica = "";
            string searchColumnMesta = "";
            string searchColumnReoncic = "";
            string searchColumnReon = "";
            string searchColumnRegion = "";
            int searchColumnOdBroja = 0;
            int searchColumnDoBroja = 0;

            //var addressData = DataSet.AsQueryable();

            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {

                    if (searchColumn.Equals("Pak"))
                    {
                        searchColumnPak = int.Parse(searchTxt);
                        addressData = DataSet.AsQueryable().Where(k => k.Id == searchColumnPak);

                    }
                    else if (searchColumn.Equals("NazivUlice"))
                    {
                        searchColumnUlica = searchTxt;
                        addressData = addressData.Where(k => k.Street.StreetName.ToUpper().Contains(searchColumnUlica.ToUpper()));
                    }
                    else if (searchColumn.Equals("OznReona"))
                    {
                        searchColumnReon = searchTxt;
                        addressData = addressData.Where(k => k.Reon.OznReona.ToUpper().Contains(searchColumnReon.ToUpper()));
                    }
                    else if (searchColumn.Equals("NazivMesta"))
                    {
                        searchColumnMesta = searchTxt;
                        addressData = addressData.Where(k => k.Place.PlaceName.ToUpper().Contains(searchColumnMesta.ToUpper()));
                    }
                    else if (searchColumn.Equals("NazivReoncica"))
                    {
                        searchColumnReoncic = searchTxt;
                        addressData = addressData.Where(k => k.Reoncic.NazivReoncica.ToUpper().Contains(searchColumnReoncic.ToUpper()));
                    }
                    else if (searchColumn.Equals("NazivSkraceniRegiona"))
                    {
                        searchColumnRegion = searchTxt;
                        addressData = addressData.Where(k => k.Region.NazivSkraceni.ToUpper().Contains(searchColumnRegion.ToUpper()));
                    }
                    else if (searchColumn.Equals("OdBroja") || searchColumn.Equals("DoBroja"))
                    {
                        searchColumnOdBroja = int.Parse(searchTxt);
                        searchColumnDoBroja = int.Parse(searchTxt);

                        addressData = addressData.Where(u => u.OdBroja <= searchColumnOdBroja && u.DoBroja >= searchColumnDoBroja);
                    }
                }

            }



            return addressData;
        }

        public int GetTotalAddress()
        {
            var addressQuery = DataSet.AsQueryable();

            return addressQuery.Count();
        }



        public int GetPakId(string adresaTxt, string brojTxt)
        {
            int pakId;
            PAK pakResult;

            string[] adresaArray = adresaTxt.Split('-');
            string ulica = adresaArray[0].TrimEnd();
            string mesto = adresaArray[1].TrimStart();

            int broj = 0; //ovde je potrebno jos provera da li je to broj, bb ili 0 i slicno
            if (brojTxt.Contains("/"))
            {
                string[] brojArray = brojTxt.Split('/');
                broj = int.Parse(brojArray[0]);
            }
            else
            {
                broj = int.Parse(brojTxt);
            }

            var addressData = DataSet.Include(u => u.Street).Include(m => m.Place).AsQueryable();
            addressData = addressData.Where(x => x.Place.PlaceName.ToUpper().Equals(mesto.ToUpper()))
                        .Where(x => x.Street.StreetName.ToUpper().Equals(ulica.ToUpper()));

            if (broj > 0)
            {
                addressData = addressData.Where(u => u.OdBroja <= broj && u.DoBroja >= broj);
            }
                        

           
            if (broj % 2 == 0)
            {
                pakResult = addressData.Where(x => x.Parnost == 2 || x.Parnost == 4 || x.Parnost == 7).SingleOrDefault();
            }
            else
            {
                pakResult = addressData.Where(x => x.Parnost == 0 || x.Parnost == 1 || x.Parnost == 3 || x.Parnost == 7).SingleOrDefault();
            }

            pakId = pakResult.Id;

            return pakId;
        }

        //public IEnumerable<AddressViewModel> GetAddress()
        //{
        //    IEnumerable<PAK> paks = DataSet.Include(u => u.Street)
        //                                                             .Include(r => r.Reon)
        //                                                             .Include(re => re.Reoncic)
        //                                                             .Include(m => m.Place)
        //                                                             .Include(reg => reg.Region).ToList();

        //    if (paks != null)
        //    {
        //        List<AddressViewModel> adreseDisplay = new List<AddressViewModel>();
        //        foreach (var x in paks)
        //        {
        //            var addressDisplay = new AddressViewModel()
        //            {
        //                Pak = x.Id,
        //                NazivUlice = x.Street.StreetName,
        //                OznReona = x.Reon.OznReona,
        //                NazivMesta = x.Place.PlaceName,
        //                NazivReoncica = x.Reoncic.NazivReoncica,
        //                NazivSkraceniRegiona = x.Region.NazivSkraceni,
        //                OdBroja = x.OdBroja,
        //                DoBroja = x.DoBroja

        //            };
        //            adreseDisplay.Add(addressDisplay);
        //        }
        //        return adreseDisplay;
        //    }
        //    return null;
        //}

        //public IEnumerable<AddressViewModel> GetAddress(string searchPak, string searchUlica, string searchMesta, string searchReoncic, string searchReon, string searchRegion, int? searchBroj)
        //{
        //    IEnumerable<PAK> paks = DataSet.Include(u => u.Street)
        //                                    .Include(r => r.Reon)
        //                                    .Include(re => re.Reoncic)
        //                                    .Include(m => m.Place)
        //                                    .Include(reg => reg.Region)
        //                                    .Where(u => u.Street.StreetName.ToUpper().Contains(searchUlica.ToUpper())
        //                                      && u.Place.PlaceName.ToUpper().Contains(searchMesta.ToUpper())
        //                                      && (u.OdBroja <= searchBroj
        //                                      && u.DoBroja >= searchBroj)).ToList();

        //    if (paks != null)
        //    {
        //        List<AddressViewModel> adreseDisplay = new List<AddressViewModel>();
        //        foreach (var x in paks)
        //        {
        //            var addressDisplay = new AddressViewModel()
        //            {
        //                Pak = x.Id,
        //                NazivUlice = x.Street.StreetName,
        //                OznReona = x.Reon.OznReona,
        //                NazivMesta = x.Place.PlaceName,
        //                NazivReoncica = x.Reoncic.NazivReoncica,
        //                NazivSkraceniRegiona = x.Region.NazivSkraceni,
        //                OdBroja = x.OdBroja,
        //                DoBroja = x.DoBroja

        //            };
        //            adreseDisplay.Add(addressDisplay);
        //        }
        //        return adreseDisplay;
        //    }
        //    return null;
        //}

        //AddressEditViewModel
        public PAK GetEditAddress(int? pakId)
        {
            var pak = DataSet.Include(u => u.Street).Include(m => m.Place).Where(x => x.Id == pakId).SingleOrDefault();

            return pak;
            
        }

        



        private IUowCommandResultFactory UowCommandResultFactory { get; }

       
    }
}
