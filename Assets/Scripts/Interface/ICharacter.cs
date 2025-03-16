namespace Interface
{
    public interface ICharacter
    {
        bool CanGive();
        
        bool CanCarry();
        
        void TakeFruits(ITransfer transfer);
        
        ITransfer RemoveFruits();
        
        void TakeCash(ITransfer transfer);
    }
}