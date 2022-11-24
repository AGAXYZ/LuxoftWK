using CMASTER.POS.Business.Interfaces;

namespace CMASTER.POS.Business
{
    public class Cash : ICash
    {
        #region Fields

        private decimal _value;
        private int _quantity;

        #endregion

        #region Properties

        public decimal Value { get => _value; set => this._value = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Cash class constructor
        /// </summary>
        /// <param name="value">The value of the bill/coin</param>
        /// <param name="quantity">The quantity of the bill/coin</param>
        public Cash(decimal value, int quantity)
        {
            _value = value;
            _quantity = quantity;
        }

        #endregion
    }
}
