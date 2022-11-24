using CMASTER.POS.Business.Interfaces;

namespace CMASTER.POS.Business
{
    public class Purchase : IPurchase
    {
        #region Fields

        private IEnumerable<decimal> _currencyDenominations;

        #endregion

        #region Constructors

        public Purchase(IEnumerable<decimal> CurrencyDenominations)
        {
            _currencyDenominations = CurrencyDenominations;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the amount of change (values and quantities) that needs to be returned to the customer
        /// </summary>
        /// <param name="providedCash">IEnumerable elements of object type Cash. The bills and coins provided by the customer</param>
        /// <param name="totalPrice">The total price of the item(s) being purchased</param>
        /// <returns>List of Cash objects. Only the required bills or coins will be returned</returns>
        /// <exception cref="Exception">Exception will be thrown if total price is higher than the provided cash or if other errors occurs</exception>
        public IEnumerable<ICash> CalculateChange(IEnumerable<ICash> providedCash, decimal totalPrice)
        {
            //Calculating the total cash that the customer provided
            decimal ProvidedCash = providedCash.Sum(c => c.Value * c.Quantity);

            if (totalPrice > ProvidedCash)
                throw new Exception($"The provided cash ${ProvidedCash} is not enough to pay the total purchase price of ${totalPrice}.");

            try
            {
                List<ICash> ChangeCashList = new List<ICash>();
                decimal Change = ProvidedCash - totalPrice;

                //Cycle to determine minimum number of bills and coins to return to the customer
                while (Change > 0)
                {
                    //Find the largest bill/coin denomination that does not surpass the change
                    ICash FoundCash = new Cash(_currencyDenominations.FirstOrDefault(c => Change >= c), 1);

                    //If the cash denomination is already in the list, just add 1 more to Quantity
                    if (ChangeCashList.Exists(c => c.Value == FoundCash.Value))
                        ChangeCashList.Find(c => c.Value == FoundCash.Value).Quantity++;
                    else
                        ChangeCashList.Add(FoundCash);

                    //Subtract the bill/coin value to the remaining change
                    Change -= FoundCash.Value;
                }

                return ChangeCashList;
            }
            catch (Exception ex)
            {
                throw new Exception($"The was a problem when trying to calculate the change. The returned message is: {ex.Message}");
            }
        }

        #endregion
    }
}
