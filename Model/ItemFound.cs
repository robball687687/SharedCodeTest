using System;
using System.Collections.Generic;
using System.Text;

namespace EbayPriceFinder.Model
{
    public class ItemFound
    {
        public int ItemFoundId { get; set; }
        public string ItemNumber { get; set; }
        public List<string> AllPrices { get; set; }
        public DateTime Created { get; set; }
    }
}
