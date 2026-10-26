using UnityEngine;

[CreateAssetMenu]
public class Countermeasure : CardEffect
{
    public override bool Resolve(CardEffectContext ctx)
    {
        ctx.AttackContext.IsCancelled = true;
        Debug.Log("Block");
        return true;
    }

    public override void RemoveEffect(Modifier modifier, CardInstance target)
    {
    }
}
