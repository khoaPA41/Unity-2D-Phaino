using UnityEngine;

public enum TargetRule
{
    None,
    OwnSoul,
    EnemySoul,
    AnySoul
}

public enum EffectDuration
{
    Forever,
    ByTurn,
    StillExists
}

public abstract class CardEffect : ScriptableObject
{
    public const int NoExpiry = -1;

    [field: SerializeField] public EffectDuration EffectDuration { get; private set; }
    [field: SerializeField] public int Turns { get; private set; } = 1;

    protected int ModifierTurn => EffectDuration == EffectDuration.ByTurn ? Turns : NoExpiry;

    [field: SerializeField] public TargetRule TargetRule { get; private set; }
    public virtual bool CanActivate(CardEffectContext ctx) => true;
    public virtual void RemoveEffect(Modifier modifier, CardInstance target)
    {

    }
    public abstract bool Resolve(CardEffectContext context);
}


