using EbayPriceFinder.Model;
using EbayPriceFinder.Services.ItemFinder;
using EbayPriceFinder.Services.PartNum;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPriceFinder.Services.PriceFinder
{
    public class PriceFinderService : IPriceFinderService
    {        
        private readonly IItemFoundService m_ItemFoundService;
        private readonly IPartNumberService m_IPartNumberService;
        public PriceFinderService(IItemFoundService ItemFoundService,
            IPartNumberService partNumberService)
        {           
            //I am initializing all of the services this class will need in the constructor
            //These are all getting set from the services I registered on startup in program.cs
            m_ItemFoundService = ItemFoundService;
            m_IPartNumberService = partNumberService;
        }

        public async Task<List<ItemFound>> GetPrices()
        {           
            try
            {
                //I am getting all the part numbers i want to look for from this service call
                var allPartsToSearchFor = m_IPartNumberService.GetPartNumbers();

                //This is where i setup the chrome driver exe
                //You need to change this path to match on your local machine for this application to work
                System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"C:\Users\Rob\source\repos\EbayPriceFinder\bin\Debug\netcoreapp3.1\chromedriver.exe");

                //This is creating the web driver
                //You need to change this path to match on your local machine for this application to work
                IWebDriver driver = new ChromeDriver(@"C:\Users\Rob\source\repos\EbayPriceFinder\bin\Debug\netcoreapp3.1");

                //Now we are searching for the parts
                return await SearchForPricesOnEbay(driver, allPartsToSearchFor);
            }
            catch (Exception ex)
            {
                var test = ex;
            }

            return new List<ItemFound>();
        }
               
        public async Task<List<ItemFound>> SearchForPricesOnEbay(IWebDriver driver, List<ItemsToLookFor> allPartsToSearchFor)
        {
            //Initializing variables
            var siteURL = $"";
            int count = 0;
            List<ItemFound> newEbayPrices = new List<ItemFound>();

            foreach (var key in allPartsToSearchFor.Skip(0))
            {
                //We are going to loop through every part number
                var tmpNumber = key.ItemNumber;                  
                count = count + 1;                    
                Console.WriteLine($"{count} out of {allPartsToSearchFor.Count}");

                try
                {
                    //we are going to search ebay for the part number we are looking for
                    siteURL = $"https://www.ebay.com/sch/i.html?_from=R40&_trksid=p2380057.m570.l1312.R1.TR0.TRC0.A0.H0.TRS5&_nkw={tmpNumber}&_sacat=0";

                    //this moves the chrome browser to the ebay site
                    driver.Navigate().GoToUrl($"{siteURL}{tmpNumber}");
                    
                    List<string> tmpPrices = new List<string>();

                    //We are going to get the first ten items listed on ebay
                    for (int i = 1; i <= 10; i++)
                    {
                        //This code find the Xpath to the Title of the ebay item
                        var tmpHeaderXpath = $"//*[@id='srp-river-results']/ul/li[{i}]/div/div[2]/a/h3";

                        //We need to make sure we can find the item on the page otherwise an exception is thrown
                        if (isElementPresent(driver, tmpHeaderXpath))
                        {
                            //The header is found on the page now we check the title of the ebay item
                            var tmpHeaderSpan = driver.FindElement(By.XPath(tmpHeaderXpath));

                            var tmpHeaderText = tmpHeaderSpan.Text;

                            //If the title of the ebay item contains the part number we are looking for this is a valid item we want to track
                            if (CheckIfHeaderContainsNumber(tmpHeaderText, tmpNumber))
                            {
                                //now we want to check the first 5 rows within the item for the price
                                for (int x = 1; x <= 5; x++)
                                {                                    
                                    var tmpXpath = $"//*[@id='srp-river-results']/ul/li[{i}]/div/div[2]/div[{x}]/div[1]/span";

                                    //we want to make to the element on the xpath exists
                                    if (isElementPresent(driver, tmpXpath))
                                    {
                                        //if the element exists we then see if it's a price
                                        var tmpSpan = driver.FindElement(By.XPath(tmpXpath));

                                        var tmpText = tmpSpan.Text;

                                        //we are seeing if this element is a price
                                        if (tmpText.Contains("$"))
                                        {
                                            //This element is a price so we want to track this price
                                            tmpPrices.Add(tmpText);

                                            Console.WriteLine($"{count} out of {allPartsToSearchFor.Count} adding price {tmpText}");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //We now could have a list of prices for the tracked item
                    if (tmpPrices.Count > 0)
                    {
                        ItemFound tmpPrice = new ItemFound()
                        {
                            ItemNumber = tmpNumber,
                            AllPrices = tmpPrices,
                            Created = DateTime.Now
                        };

                        // this service would create a object for all the tracked prices and then add them to a repository
                        //m_ItemFoundService.CreateByPN(tmpPrice);

                        // we are just adding the prices to a list now to return some data to the user
                        newEbayPrices.Add(tmpPrice);
                    }
                }
                catch (Exception ex)
                {
                    var exception = ex;
                }
            }            

            return newEbayPrices;
        }

        public bool CheckIfHeaderContainsNumber(string header, string number)
        {
            //this method checks to see if the header contains the item number
            // we try to alter the part number a bit incase it has different characters in it
            var compareHeader = header.ToUpper();
            var compareNumber = number.ToUpper();
            
            if (compareHeader.Contains(compareNumber))
            {
                return true;
            }

            compareHeader = compareHeader.Replace(" ", "");
            compareNumber = compareNumber.Replace(" ", "");

            if (compareHeader.Contains(compareNumber))
            {
                return true;
            }

            compareHeader = compareHeader.Replace("-", "");
            compareNumber = compareNumber.Replace("-", "");

            if (compareHeader.Contains(compareNumber))
            {
                return true;
            }

            return false;
        }
                
        public bool isElementPresent(IWebDriver driver, string key)
        {
            // this checks if the xpath is on the page or not
            try
            {
                driver.FindElement(By.XPath(key));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
