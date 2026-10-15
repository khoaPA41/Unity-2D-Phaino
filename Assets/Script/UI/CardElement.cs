using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardElement : MonoBehaviour
{
    public Image CardDetailImage;
    public Image CardElementalImage;
    public Image CardTypeImage;
    public TextMeshProUGUI CardStar;

    public void Setup(Sprite detail, Sprite elemental, Sprite type, string start)
    {
        CardDetailImage.sprite = detail;
        CardElementalImage.sprite = elemental;
        CardTypeImage.sprite = type;
        CardStar.SetText(start);
    }
}
