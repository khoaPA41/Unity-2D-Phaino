using UnityEngine;

[CreateAssetMenu]
public class AddStars : CardEffect
{
    [SerializeField] private int starsToAdd = 2;
    public override void Resolve(CardEffectContext context)
    {
        var target = context.TargetCard;
        target.BattleStar = Mathf.Min(target.BattleStar + starsToAdd, 12);
    }
}
