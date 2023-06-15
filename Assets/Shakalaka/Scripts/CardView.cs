using TMPro;
using UnityEngine;

namespace Shakalaka
{
    public class CardView : MonoBehaviour
    {
        public int CardType { get; private set; }
        
        [SerializeField] private TMP_Text cardTypeText;
        
        public void SetType(int cardType)
        {
            Debug.Log($"Setting cardType as {cardType}");
            CardType = cardType;
            cardTypeText.text = cardType.ToString();
        }
    }
}