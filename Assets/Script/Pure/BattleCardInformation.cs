using System;
using System.Collections.Generic;
using Raziel.Battle;

[Serializable]
public sealed class BoardSlot // Class use for slot on board
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
    public bool IsFaceDown;
    public int EffectRemaining;
    public bool IsAttacked;

    public CardInstance(Card cardReference, bool faceDownDefault)
    {
        CardReference = cardReference;
        IsFaceDown = faceDownDefault;

        if (CardReference.CardType == CardType.Soul)
        {
            OriginalStar = cardReference.CardStar;
            BattleStar = cardReference.CardStar;
            DefTurnLimit = Math.Min(cardReference.CardStar / 2, 5);
        }
    }

    // Add Star Effect
    public int MaxStar = 20;
    public List<Modifier> Modifiers = new();

    public bool RemoveModifierFromSource(CardInstance source)
    {
        if (source?.CardReference?.Effect == null) return false;
        bool changed = false;

        for (int i = Modifiers.Count - 1; i >= 0; i--)
        {
            var modifier = Modifiers[i];

            if (modifier.SourceCard != source) continue;

            modifier.SourceCard.CardReference.Effect.RemoveEffect(modifier, this);
            Modifiers.RemoveAt(i);
            changed = true;
        }
        return changed;
    }
}

[Serializable]
public sealed class CardEffectContext
{
    public CardInstance SourceCard;
    public DuelInformation Owner;
    public BattleBoard BattleBoard;
    public IReadOnlyList<BoardSlot> Targets;
    public AttackContext AttackContext;
}

[Serializable]
public sealed class Modifier
{
    [NonSerialized] public CardInstance SourceCard;
    public Character SourceOnwer;
    public int Amount; // if have
    public Modifier(CardInstance sourceCard, Character sourceOnwer, int amount)
    {
        SourceCard = sourceCard;
        SourceOnwer = sourceOnwer;
        Amount = amount;
    }
}

[Serializable]
public sealed class AttackContext
{
    public BoardSlot AttackerSlot;
    public BoardSlot TargeterSlot;
    public bool IsCancelled;
}