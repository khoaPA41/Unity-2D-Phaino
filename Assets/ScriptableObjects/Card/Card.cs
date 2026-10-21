using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject
{
    public CardInformation CardInformation;
    public CardNameId CardNameId;
    public CardType CardType;
    public CardElemental CardElemental;
    public Sprite CardDetailImage;
    public Sprite CardElementalImage;
    public Sprite CardTypeImage;
    [Range(1, 12)] public int CardStar;
    public CardEffect Effect;
}
