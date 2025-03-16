using TMPro;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// Other Mono attach to Bot game object for displaying visual
    /// </summary>
    public class BotStateDisplayer : MonoBehaviour
    {
        [SerializeField] private Transform iconTransform;
        [SerializeField] private Transform cameraTransform;
        
        [SerializeField] private TMP_Text fruitTxt;
        
        [SerializeField] private GameObject textObject;
        [SerializeField] private GameObject cashObject;
        [SerializeField] private GameObject imojiObject;

        public void DisplayText(int fruit, int capacity)
        {
            fruitTxt.text = $"{fruit}/{capacity}";
        }
        
        public void ShowText()
        {
            textObject.SetActive(true);
            cashObject.SetActive(false);
            imojiObject.SetActive(false);
        }

        public void ShowCashier()
        {
            textObject.SetActive(false);
            cashObject.SetActive(true);
            imojiObject.SetActive(false);
        }
        
        public void ShowEmoji()
        {
            textObject.SetActive(false);
            cashObject.SetActive(false);
            imojiObject.SetActive(true);
        }

        private void Update()
        {
            iconTransform.LookAt(cameraTransform);
        }
    }
}
