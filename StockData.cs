using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace Telegram_Bot
{
  public  class StockData : IAlertable
    {
       
        //samlpe for next extensions
        public async Task<int> GetStockData(string symbol, DateTime startDate, DateTime enddate)
        {
            try
            {
                var historic_data = await Yahoo.GetHistoricalAsync(symbol, startDate, enddate);
                var security = await Yahoo.Symbols(symbol).Fields(Field.LongName).QueryAsync();
                var ticker = security[symbol];
                var companyName = ticker[Field.LongName];
                
                for (int i = 0; i < historic_data.Count; i++)
                {
                    Console.WriteLine(companyName + "Closing price on"+ historic_data.ElementAt(i).DateTime.Month + "/"
                        + historic_data.ElementAt(i).DateTime.Day
                        + "/"+ historic_data.ElementAt(i).DateTime.Year
                        + ": $"+Math.Round( historic_data.ElementAt(i).Close,2));
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
            return 1; 
        }


        public async Task<int> CheckPredict(string symbol, int userPredictPrice)
        {
            //TODO: Get stock data- get current price. 
            //if current price = stockPrice = RETURN TRUE
            // if date now = predict date RETURN FALSE
            //enddate- now
            //startdate- few time ago 

            try
            {
                var startDate = DateTime.Today.AddMonths(-1);
                DateTime endDate = DateTime.Today;
                var historic_data = await Yahoo.GetHistoricalAsync(symbol, startDate, endDate);
                var security = await Yahoo.Symbols(symbol).Fields(Field.LongName).QueryAsync();
                var ticker = security[symbol];
                var companyName = ticker[Field.LongName];
                var rounded = Math.Round(historic_data.ElementAt(0).Close,0);
                
                if (userPredictPrice >= rounded)
                {
                    //Console.WriteLine("INTO IF" + " " + Alert());
                    
                    //TODO: maybe here I should add stock data with current price
                    return 1;
                }
                else
                {
                    Console.WriteLine("outside if");
                    return 0;
                }
            }
            catch (Exception)
            {

                throw ;
            }  
        }

        public string Alert()
        {
          return  "Success" ;
        }
    }
}
