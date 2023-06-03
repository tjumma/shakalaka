using TMPro;
using UnityEngine;

namespace Shakalaka
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text cardTypeText;
        
        public void SetType(int cardType)
        {
            Debug.Log($"Setting cardType as {cardType}");
            cardTypeText.text = cardType.ToString();
        }
    }
}