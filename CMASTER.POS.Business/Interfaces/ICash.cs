namespace CMASTER.POS.Business.Interfaces
{
    public interface ICash
    {
        int Quantity { get; set; }
        decimal Value { get; set; }
    }
}