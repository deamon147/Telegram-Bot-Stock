using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot
{
  public class Clockwork : IAlertable
    {
        public string Alert()
        {
            return "Your stock has reached selected wish price!";
        }

        public async Task <string> DingDing(Stock Stock)
        { 
            StockData StockMethod = new StockData();
            Clockwork Clockwork = new Clockwork();
            var Checking = await StockMethod.CheckPredict(Stock.Ticker, Stock.Price);
            if (Checking==1)
            { 
               return  Clockwork.Alert()+ "\n"+ "This one:" + "  " + Stock.Ticker; 
            }
            else
            {
                return "Your predict was mistakable.";
            }
        }
         
      
    }
}
