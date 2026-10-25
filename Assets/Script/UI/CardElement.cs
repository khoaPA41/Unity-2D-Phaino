using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardElement : MonoBehaviour
{
    [SerializeField] private GameObject FaceUpVisual;
    [SerializeField] private GameObject FaceDownVisual;

    public int CardIndex;
    public Image CardDetailImage;
    public Image CardElementalImage;
    public Image CardTypeImage;
    public TextMeshProUGUI CardStar;
    public bool IsAttackStance;

    public void Setup(int cardIndex, Sprite detail, Sprite elemental, Sprite type, string start, bool isAttackStance)
    {
        CardIndex = cardIndex;
        CardDetailImage.sprite = detail;
        CardElementalImage.sprite = elemental;
        CardTypeImage.sprite = type;
        CardStar.SetText(start);
        IsAttackStance = isAttackStance;
    }

    public void SetFaceDown(bool isFaceDown)
    {
        FaceUpVisual.SetActive(!isFaceDown);
        FaceDownVisual.SetActive(isFaceDown);
    }
}
