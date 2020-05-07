using System;
using System.Collections.Generic;
using System.Text;

namespace EbayPriceFinder.Model
{
    public class ItemsToLookFor
    {
        public int ItemsToLookForId { get; set; }
        public string ItemNumber { get; set; }        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
