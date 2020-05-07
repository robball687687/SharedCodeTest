using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EbayPriceFinder.Model;

namespace EbayPriceFinder.Services.PriceFinder
{
    public interface IPriceFinderService
    {
        Task<List<ItemFound>> GetPrices();
    }
}
