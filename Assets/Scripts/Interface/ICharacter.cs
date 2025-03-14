namespace Interface
{
    public interface ICharacter
    {
        bool CanGive();
        
        bool CanCarry();
        
        void TakeFruits();
        
        int RemoveFruits();
    }
}