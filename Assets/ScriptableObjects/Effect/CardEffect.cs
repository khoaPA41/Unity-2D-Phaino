using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Resolve(CardEffectContext context);
}


