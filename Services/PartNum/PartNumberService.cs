using EbayPriceFinder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EbayPriceFinder.Services.PartNum
{
    public class PartNumberService : IPartNumberService
    {
        public List<ItemsToLookFor> GetPartNumbers()
        {
            //Usually I would have this service call a repository to get part numbers from a table
            //But right now I am just hard coding the service to return a list of objects

            var retValue = new List<ItemsToLookFor>() 
            {
                new ItemsToLookFor()
                { 
                    ItemsToLookForId = 1,
                    ItemNumber = "BasketBall",
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                },
                new ItemsToLookFor()
                {
                    ItemsToLookForId = 2,
                    ItemNumber = "nintendo switch",
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                }
            };

            return retValue;
        }
    }
}
