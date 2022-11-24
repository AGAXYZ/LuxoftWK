using CMASTER.POS.Business;
using CMASTER.POS.Business.Exceptions;
using CMASTER.POS.Business.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CMASTER.POS.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // == NOTE == It is required to add an appsettings.json file with the "currency" values (each denomination separated by pipe) to a new project that implements the Purchase rutine
                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                IEnumerable<decimal> currencyDenominations = config["currency"].Split("|").Select(decimal.Parse).OrderByDescending(c => c);

                //Type the required input information
                Console.WriteLine("Welcome to the CASH Masters Point Of Sale!");
                Console.Write("Please enter the total price of the items being purchased: ");
                decimal totalPrice = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Please enter the corresponding quantity of bills and coins provided by the customer: ");

                List<Cash> receivedCash = new List<Cash>();
                int quantity = 0;

                //Add every bill/coin denomination taken from the appsettings.json file
                foreach (decimal currency in currencyDenominations)
                {
                    Console.Write($"Value: {currency} - Quantity: ");
                    quantity = Convert.ToInt32(Console.ReadLine());
                    receivedCash.Add(new Cash(currency, quantity));
                }

                //Create instance of Purchase class and call the CalculateChange method
                IPurchase purchase = new Purchase(currencyDenominations);
                IEnumerable<ICash> change = purchase.CalculateChange(receivedCash, totalPrice);

                Console.WriteLine("=== Return to the customer the following bills and/or coins ===");

                foreach (ICash cash in change)
                    Console.WriteLine($"Value: {cash.Value} - Quantity: {cash.Quantity}");
            }
            catch (Exception ex)
            {
                //Explicitly adding the different exceptions
                switch (ex)
                {
                    case NotEnoughtCashException:
                        Console.WriteLine(ex.Message);
                        break;
                    case NoChangeException:
                        Console.WriteLine(ex.Message);
                        break;
                    default:
                        Console.WriteLine($"There was a problem during the process. The returned message is: {ex.Message}");
                        break;
                }
            }

            Console.Read();
        }
    }
}