using Data;
using TMPro;
using UnityEngine;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TMP_Text cashTxt;
    
    private void Start()
    {
        playerData.OnCashChanged += UpdateCash;
    }

    private void OnDestroy()
    {
        playerData.OnCashChanged -= UpdateCash;
    }

    private void UpdateCash(int number)
    {
        cashTxt.text = number.ToString();
    }
}
