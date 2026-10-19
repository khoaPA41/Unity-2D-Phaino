using System;
using Raziel.Battle;

[Serializable]
public readonly struct BattleResult
{
    public int AttackerStarAfter { get; }
    public int TargeterStarAfter { get; }

    public bool AttackerDies => AttackerStarAfter <= 0;
    public bool TargeterDies => TargeterStarAfter <= 0;

    public BattleResult(int attackerStartAfter, int targeterStarAfter)
    {
        AttackerStarAfter = attackerStartAfter - targeterStarAfter;
        TargeterStarAfter = targeterStarAfter - attackerStartAfter;
    }
}

public class BattleResolver
{
    // Attack
    public BattleResult Attack(CardInstance attacker, CardInstance targeter)
    {
        var attackerCardStar = attacker.BattleStar;
        var targeterCardStar = targeter.BattleStar;

        var result = new BattleResult(attackerCardStar, targeterCardStar);
        attacker.BattleStar = result.AttackerStarAfter;
        targeter.BattleStar = result.TargeterStarAfter;

        return result;
    }
}
