using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInformation", menuName = "ScriptableObjects/CardInformation")]
public class CardInformation : ScriptableObject
{
    public List<CardElementGroup> Groups;

    public bool TryGetInformation(CardNameId cardNameId, out CardType cardType, out CardElemental cardElemental)
    {
        cardType = default;
        cardElemental = default;

        if (Groups == null) return false;

        bool found = false;

        foreach (var group in Groups)
        {
            if (group?.CardInfomationEntries == null) continue;

            foreach (var entry in group.CardInfomationEntries)
            {
                if (entry == null || entry.CardNameId != cardNameId) continue;

                if (found)
                {
                    cardType = default;
                    cardElemental = default;
                    return false;
                }
                cardType = entry.CardType;
                cardElemental = group.CardElemental;
                found = true;
            }
        }

        return found;
    }
}
