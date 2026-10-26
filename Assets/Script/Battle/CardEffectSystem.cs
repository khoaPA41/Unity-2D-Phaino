using System;
using System.Collections.Generic;
using Raziel.Battle;

public class CardEffectSystem
{
    private readonly BattleBoard _battleBoard;
    public CardEffectSystem(BattleBoard battleBoard)
    {
        _battleBoard = battleBoard;
    }

    public bool IsNeedTarget(CardInstance source) // Check this card need target or not
    {
        var effect = source?.CardReference?.Effect;
        return effect != null && effect.TargetRule != TargetRule.None;
    }

    public List<BoardSlot> GetAllValidTarget(CardInstance source, DuelInformation owner) // Return all targets valid
    {
        var result = new List<BoardSlot>();
        var effect = source?.CardReference?.Effect;

        if (effect == null) return result;

        var enemy = owner.character == Character.Player ? Character.Enemy : Character.Player; // Get enemy by character

        switch (effect.TargetRule)
        {
            case TargetRule.OwnSoul:
                AddSoul(result, owner.character);
                break;
            case TargetRule.EnemySoul:
                AddSoul(result, enemy);
                break;
            case TargetRule.AnySoul:
                AddSoul(result, owner.character);
                AddSoul(result, enemy);
                break;
        }
        return result;
    }

    public bool TryActiveEffect(CardInstance source, DuelInformation owner, IReadOnlyList<BoardSlot> targets, AttackContext attackContext = null)
    {
        var effect = source?.CardReference?.Effect;
        if (effect == null || source.HasUsedEffect) return false;

        targets ??= Array.Empty<BoardSlot>();

        if (!AreTargetValid(source, owner, targets)) return false;


        var context = new CardEffectContext
        {
            SourceCard = source,
            Owner = owner,
            BattleBoard = _battleBoard,
            Targets = targets,
            AttackContext = attackContext
        };

        if (!effect.CanActivate(context)) return false;
        if (!effect.Resolve(context)) return false;

        source.HasUsedEffect = true;

        foreach (var target in targets)
        {
            _battleBoard.NotiySlotChanged(target);
        }

        if (effect.EffectDuration != EffectDuration.ByTurn)
        {
            var sourceSlot = _battleBoard.FindSlot(source);
            if (sourceSlot != null) _battleBoard.CardDeath(sourceSlot);
        }
        else
        {
            source.EffectRemaining = effect.ModifierTurn;
        }

        return true;
    }

    private bool AreTargetValid(CardInstance source, DuelInformation owner, IReadOnlyList<BoardSlot> targets) // Check current target with targets valid list
    {
        if (!IsNeedTarget(source)) return targets.Count == 0;
        if (targets.Count == 0) return false;

        var valid = GetAllValidTarget(source, owner);

        foreach (var target in targets)
        {
            if (!valid.Contains(target)) return false;
        }
        return true;
    }

    public void AddSoul(List<BoardSlot> boardSlots, Character character)
    {
        foreach (var soulSlot in _battleBoard.GetSlotByType(character, CardType.Soul))
        {
            if (soulSlot?.CardInstance?.CardReference != null) boardSlots.Add(soulSlot);
        }
    }

    public bool TrySummon(CardInstance source, DuelInformation owner, List<BoardSlot> targets) // Try Summon Effect Card
    {
        var effect = source?.CardReference?.Effect;
        if (effect == null || source.HasUsedEffect) return false;

        var context = new CardEffectContext
        {
            SourceCard = source,
            Owner = owner,
            BattleBoard = _battleBoard,
            Targets = targets
        };

        if (!effect.CanActivate(context)) return false;

        return true;
    }

    public void TickDuration(Character turnOwner) // Subtract turn if card effect is has Turn
    {
        var slots = _battleBoard.GetSlotByType(turnOwner, CardType.Mighty);

        foreach (var slot in slots)
        {
            var card = slot?.CardInstance;
            var effect = card?.CardReference?.Effect;

            if (effect == null || !card.HasUsedEffect) continue;
            if (effect.EffectDuration != EffectDuration.ByTurn) continue;


            if (card.EffectRemaining > 0) card.EffectRemaining--;
            if (card.EffectRemaining <= 0)
            {
                _battleBoard.CardDeath(slot);
            }
        }
    }

    public void RemoveSourceEffect(CardInstance source)
    {
        var effect = source?.CardReference?.Effect;
        if (effect == null || !source.HasUsedEffect) return;
        if (effect.EffectDuration != EffectDuration.ByTurn) return;

        source.EffectRemaining = 0;

        foreach (var side in new[] { Character.Player, Character.Enemy })
        {
            var slots = _battleBoard.GetSlotByType(side, CardType.Soul);

            foreach (var slot in slots)
            {
                var card = slot?.CardInstance;

                if (card?.CardReference == null) continue;

                if (!card.RemoveModifierFromSource(source)) continue;

                if (card.BattleStar <= 0)
                {
                    _battleBoard.CardDeath(slot);
                }
                else
                {
                    _battleBoard.NotiySlotChanged(slot);
                }
            }
        }
    }
}
