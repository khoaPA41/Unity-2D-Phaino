using System;
using System.Collections.Generic;
using Raziel.Battle;

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
    public bool IsFaceDown;

    public CardInstance(Card cardReference, bool faceDownDefault)
    {
        CardReference = cardReference;
        IsFaceDown = faceDownDefault;

        if (CardReference.CardType == CardType.Soul)
        {
            OriginalStar = cardReference.CardStar;
            BattleStar = cardReference.CardStar;
            DefTurnLimit = cardReference.CardStar / 2;
        }
    }

    // Add Star Effect
    public int MaxStar = 20;
    public List<Modifier> Modifiers = new();

    public bool TickModifiers(Character turnOwner)
    {
        bool changed = false;

        for (int i = Modifiers.Count - 1; i >= 0; i--)
        {
            var modifier = Modifiers[i];
            if (modifier.RemainingTurns < 0 || modifier.SourceOnwer != turnOwner) continue;

            modifier.RemainingTurns--;

            if (modifier.RemainingTurns > 0) continue;

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
    public int RemainingTurns;
    public int Amount; // if have
    public Modifier(CardInstance sourceCard, Character sourceOnwer, int remainingTurns, int amount)
    {
        SourceCard = sourceCard;
        SourceOnwer = sourceOnwer;
        RemainingTurns = remainingTurns;
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