using EbayPriceFinder.Services.ItemFinder;
using EbayPriceFinder.Services.PartNum;
using EbayPriceFinder.Services.PriceFinder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EbayPriceFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is where I register all the services for the application on startup
            var serviceProvider = new ServiceCollection()
                
                .AddTransient<IPartNumberService, PartNumberService>()
                .AddTransient<IItemFoundService, ItemFoundService>()
                .AddTransient<IPriceFinderService, PriceFinderService>()

                .BuildServiceProvider();

            //I am now grabbing one of the services i registered and running it's "get Price" method
            var m_PriceFinderService = serviceProvider.GetService<IPriceFinderService>();
            try
            {
                var itemsAndPrices = m_PriceFinderService.GetPrices();

                //You can set a breakpoint here to view all the items and the prices found
                var breakpoint = itemsAndPrices;
            }
            catch (Exception ex)
            {
                //I usually set a break point here to catch any exceptions that might get thrown
                var setABreakpointHere = 5;
            }                       
            Console.WriteLine("Finished finding and updating prices.");

        }
    }
}
