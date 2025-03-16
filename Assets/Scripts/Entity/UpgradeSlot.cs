using System;
using TMPro;
using UnityEngine;

namespace Entity
{
    public class UpgradeSlot : InteractBase
    {
        public Vector3 offsetVector = Vector3.zero;
        
        [SerializeField] private int upgradeCost = 30;
        [SerializeField] private GameObject upgradePrefab;
        [SerializeField] private TMP_Text numberTxt;

        private void Start()
        {
            UpdateText(upgradeCost);
        }

        protected override float GetCooldownTime()
        {
            return 0.1f;
        }

        protected override bool CanInteractWithPlayer()
        {
            return character.CanBuy();
        }

        protected override void InteractPlayer()
        {
            character.SpendCash();
            upgradeCost -= 1;
            UpdateText(upgradeCost);
            if (upgradeCost <= 0)
            {
                Instantiate(upgradePrefab, transform.position + offsetVector, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        private void UpdateText(int value)
        {
            numberTxt.text = value.ToString();
        }
    }
}
