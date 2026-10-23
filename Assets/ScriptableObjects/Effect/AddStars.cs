using UnityEngine;

[CreateAssetMenu]
public class AddStars : CardEffect
{
    [SerializeField] private int starsToAdd = 2;
    public override bool Resolve(CardEffectContext context) =>
    context.Targets[0].CardInstance.AddStars(context.SourceCard, starsToAdd, context.Owner.character, ModifierTurn) > 0;
}
