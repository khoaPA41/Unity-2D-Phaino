using UnityEngine;

[CreateAssetMenu]
public class Countermeasure : CardEffect
{
    public override bool Resolve(CardEffectContext ctx)
    {
        // ctx.BattleBoard.CancelAttack();
        // ctx.BattleBoard.FinishUseRupture();
        ctx.AttackContext.IsCancelled = true;
        Debug.Log("Block");

        return true;
    }

    public override void RemoveEffect(Modifier modifier, CardInstance target)
    {
    }
}
