using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public int numberOfCash = 0;
        
        public event Action<int> OnCashChanged;

        public void AddMoney(int amount)
        {
            numberOfCash += amount;
            OnCashChanged?.Invoke(numberOfCash);
        }
        
        public void ChangeCash(int amount)
        {
            numberOfCash = amount;
            OnCashChanged?.Invoke(numberOfCash);
        }
    }
}
