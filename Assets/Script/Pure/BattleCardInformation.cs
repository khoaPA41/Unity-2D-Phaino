using System;
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
}
