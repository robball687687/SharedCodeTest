using EbayPriceFinder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EbayPriceFinder.Services.PartNum
{
    public interface IPartNumberService
    {       
        List<ItemsToLookFor> GetPartNumbers();
    }
}
