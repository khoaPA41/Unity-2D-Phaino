using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public CardNameId CardNameId;
    public CardType CardType;
    public Sprite CardDetailImage;
    public Sprite CardElementalImage;
    public Sprite CardTypeImage;
    public string CardStar;
}
