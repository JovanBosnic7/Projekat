using Bex.Models;
using Bex.ViewModels;
using System;
using System.Collections.Generic;


namespace Bex.Common
{
    public interface IAddressRepository : IRepository<PAK>
    {
        //IUowCommandResult GetAllAddress();
        //IEnumerable<AddressViewModel> GetAddress();
        //IEnumerable<AddressViewModel> GetAddress(string searchPak, string searchUlica, string searchMesta, string searchReoncic, string searchReon, string searchRegion, int? searchBroj);

        IEnumerable<PAK> GetSearchAddressData(string searchTerms);

        IEnumerable<PAK> GetAddressAutocompliteData(string searchQuery);
        PAK GetEditAddress(int? id);

        int GetTotalAddress();

        int GetPakId(string adresaTxt, string brojTxt);
    }
}
