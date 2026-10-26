using UnityEngine;

[CreateAssetMenu]
public class AddStars : CardEffect
{
    [SerializeField] private int starsToAdd = 2;

    public override bool CanActivate(CardEffectContext ctx)
    {
        var targets = ctx.Targets;
        var source = ctx.SourceCard;

        int soulValid = 0;

        if (source.CardReference.Effect.TargetRule == TargetRule.None) return true;

        foreach (var target in targets)
        {
            if (target.CardInstance.BattleStar >= target.CardInstance.MaxStar) continue;
            soulValid++;
        }
        return soulValid > 0;
    }

    public override bool Resolve(CardEffectContext ctx)
    {
        var targets = ctx.Targets;
        var source = ctx.SourceCard;

        foreach (var target in targets)
        {
            int battleStar = target.CardInstance.BattleStar;
            int before = battleStar;

            battleStar = Mathf.Clamp(battleStar + starsToAdd, 1, target.CardInstance.MaxStar); // Add to battle stars
            int applied = battleStar - before;
            if (applied > 0)
            {
                target.CardInstance.Modifiers.Add(new Modifier(source, ctx.Owner.character, applied));
                target.CardInstance.BattleStar = battleStar;
            }

        }
        return true;
    }

    public override void RemoveEffect(Modifier modifier, CardInstance target)
    {
        target.BattleStar = Mathf.Max(target.BattleStar - modifier.Amount, 0);
    }
}
