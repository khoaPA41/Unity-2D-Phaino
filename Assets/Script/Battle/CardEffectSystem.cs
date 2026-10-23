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


    public List<BoardSlot> GetAllValidTarget(CardInstance source, DuelInformation owner)
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

    public bool TryActiveEffect(CardInstance source, DuelInformation owner, IReadOnlyList<BoardSlot> targets)
    {
        var effect = source?.CardReference?.Effect;
        if (effect == null || source.HasUsedEffect) return false;

        targets ??= Array.Empty<BoardSlot>();

        if (!AreTargetValid(source, owner, targets)) return false;


        var context = new CardEffectContext
        {
            SourceCard = source,
            Owner = owner,
            Targets = targets
        };

        if (!effect.CanActivate(context)) return false;
        if (!effect.Resolve(context)) return false;

        source.HasUsedEffect = true;

        foreach (var target in targets)
        {
            _battleBoard.NotiySlotChanged(target);
        }

        if (effect.EffectDuration != EffectDuration.StillExists)
        {
            var sourceSlot = _battleBoard.FindSlot(source);
            if (sourceSlot != null) _battleBoard.CardDeath(sourceSlot);
        }

        return true;
    }

    private bool AreTargetValid(CardInstance source, DuelInformation owner, IReadOnlyList<BoardSlot> targets)
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
}
