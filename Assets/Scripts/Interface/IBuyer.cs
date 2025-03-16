namespace Interface
{
    public interface IBuyer
    {
        bool CanBuy();
        
        void TakeCash(ITransfer transfer);

        void SpendCash();
    }
}