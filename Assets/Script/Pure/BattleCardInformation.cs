using System;
using System.Collections.Generic;
using Raziel.Battle;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[Serializable]
public class BoardSlot // Class use for slot on board
{
    public Character Character;
    public CardInstance CardInstance;
    public bool AcceptSoul; // CardType
}

[Serializable]
public class CardInstance // Class keep instance of card in battle
{
    public Card CardReference;
    public int OriginalStar;
    public int BattleStar;
    public bool IsAttackStance; // Card State
    public int DefTurnsElapsed;
    public int DefTurnLimit;
    public bool HasUsedDefense;
    public bool HasUsedEffect;


    // Add Star Effect
    public const int MaxStar = 12;
    public List<StarModifier> StarModifiers = new();

    public int AddStars(CardInstance source, int amount, Character sourceOnwer, int remainingTurns)
    {
        int before = BattleStar;
        BattleStar = Mathf.Clamp(BattleStar + amount, 1, MaxStar);

        int applied = BattleStar - before;
        if (applied > 0) StarModifiers.Add(new StarModifier(amount, source, sourceOnwer, remainingTurns));
        return applied;
    }

    public bool TickModifiers(Character turnOwner)
    {
        bool changed = false;

        for (int i = StarModifiers.Count - 1; i >= 0; i--)
        {
            var modifier = StarModifiers[i];
            if (modifier.RemainingTurns < 0 || modifier.SourceOnwer != turnOwner) continue;

            modifier.RemainingTurns--;

            if (modifier.RemainingTurns > 0) continue;
            RemoveModifierAt(i);
            changed = true;
        }
        return changed;
    }

    public bool RemoveStarFrom(CardInstance source)
    {
        bool changed = false;

        for (int i = StarModifiers.Count - 1; i >= 0; i--)
        {
            if (StarModifiers[i].SourceCard != source) continue;

            RemoveModifierAt(i);
            changed = true;
        }
        return changed;
    }

    public void RemoveModifierAt(int index)
    {
        BattleStar = Mathf.Max(BattleStar - StarModifiers[index].Amount, 1);
        StarModifiers.RemoveAt(index);
    }

}

[Serializable]
public sealed class CardEffectContext
{
    public CardInstance SourceCard;
    public DuelInformation Owner;
    public IReadOnlyList<BoardSlot> Targets;
}

[Serializable]
public sealed class StarModifier
{
    public int Amount;
    public CardInstance SourceCard;
    public Character SourceOnwer;
    public int RemainingTurns;

    public StarModifier(int amount, CardInstance sourceCard, Character sourceOnwer, int remainingTurns)
    {
        Amount = amount;
        SourceCard = sourceCard;
        SourceOnwer = sourceOnwer;
        RemainingTurns = remainingTurns;
    }
}


