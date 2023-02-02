using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using YahooFinanceApi;

namespace Telegram_Bot
{
    class Program
    {
        private static string token { get; set; } = ""; //insert TELEGRAM TOKEN 
        private static TelegramBotClient Bot;
        static List<Stock> StockList = new List<Stock>();

 
        static void Main(string[] args)
        {
           
            Bot = new TelegramBotClient(token);
            Bot.OnMessage += BotOnMessageReceived;
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            string messageText = message.Text.ToString();
            string userName = $"{message.From.FirstName} {message.From.LastName}";
            Console.WriteLine($"{userName} send message '{message.Text}'");

            if (message.Type != MessageType.Text)
            {
                return;
            }


            #region Commands
            switch (message.Text)
            {
                case "/start":
                    string intro =
 @"Command list:
/help - F.A.Q.
/getlist - Get watchlist of your shares
/clearlist - Clear watchlist
/checkpredict - Check all your predicts";
                    await Bot.SendTextMessageAsync(message.From.Id, intro);
                    break;

                case "/help":
                    string faq = "Send your stock ticker to bot and price, which should be reached." +
                        " If this stock will get this price you will get an alert message." +
                        " Example: AAPL 146 \n(Where AAPL is a stock ticker and 146 is a wish price.)";
                    await Bot.SendTextMessageAsync(message.From.Id, faq);
                    break;

                case "/getlist":
                    bool isEmpty = !StockList.Any();
                    if (isEmpty)
                    {
                        await Bot.SendTextMessageAsync(message.From.Id, "Watchlist is empty!");
                    }

                    foreach (var Stock in StockList)
                    {
                        string stockTicker = Stock.Ticker;
                        string stockPrice = Stock.Price.ToString();
                        var stockresult = stockTicker + " " + stockPrice;
                        await Bot.SendTextMessageAsync(message.From.Id, stockresult);
                    }
                    break;

                case "/checkpredict":
                    if (StockList.Any())
                    {
                        Clockwork Alerter = new Clockwork();
                        foreach (var Stock in StockList)
                        {
                            var checkRes = await Alerter.DingDing(Stock);
                            await Bot.SendTextMessageAsync(message.From.Id, checkRes);
                        }
                    }
                    else
                    {
                        goto case "/getlist";
                    }

                    break;

                case "/clearlist":
                    StockList.Clear();
                    await Bot.SendTextMessageAsync(message.From.Id, "Watchlist has been cleared!");
                    break;

            }
            #endregion

            //if sender sends a message with letter, space and number it should be a message with ticker and wish price
            if (messageText.Any(char.IsDigit) && messageText.Any(char.IsWhiteSpace) && messageText.Any(char.IsLetter))
            {
                Stock StockOne = new Stock();
                var FilledStock = StockOne.ParseString(messageText);

                string res = FilledStock.Ticker + "- stock ticker" + "\n" + FilledStock.Price + "-stock price";
                try
                {
                    await Bot.SendTextMessageAsync(message.From.Id, res);
                }
                catch (Exception ex)
                {
                    ex = new Exception("Incorrect format! Send your message in follow format: AAAA 10"
                 + "\n"
                 + "Where 'AAAA' is a ticker and 10 is a price");
                    await Bot.SendTextMessageAsync(message.From.Id, ex.Message.ToString());
                }

                StockList.Add(FilledStock);
            }
            else if (messageText.Any(char.IsDigit) && messageText.Any(char.IsLetter))
            {
                var ex = new Exception("Incorrect format! \n Send your message in follow format: \n AAAA 10"
                + "\n"
                + "Where 'AAAA' is a ticker and 10 is a price");
                await Bot.SendTextMessageAsync(message.From.Id, ex.Message.ToString());

            }

        }


    }
}
