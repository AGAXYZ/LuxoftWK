namespace CMASTER.POS.Business.Interfaces
{
    public interface IPurchase
    {
        IEnumerable<ICash> CalculateChange(IEnumerable<ICash> providedCash, decimal totalPrice);
    }
}