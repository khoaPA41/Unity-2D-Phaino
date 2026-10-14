using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public CardInformation CardInformation;
    public CardNameId CardNameId;
    public CardType CardType;
    public CardElemental CardElemental;
    public Sprite CardDetailImage;
    public Sprite CardElementalImage;
    public Sprite CardTypeImage;
    public string CardStar;
}
