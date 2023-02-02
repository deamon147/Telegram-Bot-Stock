using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot
{
    public class Stock
    {
        private string ticker;
        private decimal price;


        /// Current Stock ticker
        public string Ticker
        {
            get
            {
                return ticker;
            }
            set
            {
                // check Is ticker contains a number in itself 
                if (value.Length < 5 && value.Any(char.IsDigit) == false)
                {
                    ticker = value.ToUpper();
                }
            }
        }

  
        /// Current Stock price
        public int Price
        {
            get
            {
                return (int)price;
            }
            set
            {
                if (value > 0)
                {
                    price = value;
                }
            }
        }

        /// Parse message from user in correct data and converting this data to stock
        /// <param name="sender"></param>
        /// <param name="TickerAndPrice"></param>
        /// <returns></returns>
        public Stock ParseString(string TickerAndPrice)
        {
            Stock CurrentStock = new Stock();
            try
            {
                string[] subs = new string[1];
                subs = TickerAndPrice.Trim().Split(' ');
                var first = subs[0].ToString();
                var second = subs[1];
                if (first.Any(char.IsDigit) == false)
                {
                    CurrentStock.Ticker = subs[0].ToString();
                }
                if (second.All(char.IsDigit) == true)
                {
                    CurrentStock.Price = Convert.ToInt32(second);
                }
                else
                {
                    var ex = new Exception("Incorrect format! Send your message in follow format: AAAA 10"
                   + "\n"
                   + "Where 'AAAA' is a ticker and 10 is a price");
                    throw ex;
                }

            }
            catch (Exception)
            {
                var ex = new Exception("Incorrect format! Send your message in follow format: AAAA 10"
                    + "\n"
                    + "Where 'AAAA' is a ticker and 10 is a price");
                throw ex;
            } 
            return CurrentStock;
        }
    }
}
