using EbayPriceFinder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EbayPriceFinder.Services.ItemFinder
{
    public interface IItemFoundService
    {
        ItemFound CreateByPN(ItemFound record);
    }
}
