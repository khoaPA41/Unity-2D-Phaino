using System;
using System.Collections.Generic;
using System.Linq;
using Raziel.Battle;
using Raziel.StateMachine;
using UnityEngine;

public enum EnemyInput
{
    Draw,
    Battle,
    Main_II,
    ChooseCard,
    Summon,
    Attack,
    End
}

[RequireComponent(typeof(DuelInformation))]
public class EnemyAIControllerInput : MonoBehaviour
{
    private DuelInformation _duelInformation;
    public EnemyInput currentInput;
    private DuelStateMachine _duelStateMachine;

    public event Action<EnemyInput> ActionByInput = delegate { };

    private int _handIndex;
    private bool isMainI = true;
    private void Start()
    {
        _duelInformation = GetComponent<DuelInformation>();
        _duelStateMachine = DuelStateMachine.Instance;
    }

    private void OnEnable()
    {
        ActionByInput += Action;
    }

    private void OnDisable()
    {
        ActionByInput -= Action;
    }

    public void SetInput(EnemyInput input)
    {
        currentInput = input;
        ActionByInput?.Invoke(currentInput);
    }

    private void Action(EnemyInput input)
    {
        switch (input)
        {
            case EnemyInput.Draw:
                _duelStateMachine.Draw();
                break;
            case EnemyInput.Main_II:
                _duelStateMachine.MainII();
                break;
            case EnemyInput.ChooseCard:
                _duelStateMachine.ChooseCard(RandomChoose(isMainI));
                break;
            case EnemyInput.Battle:
                _duelStateMachine.ChangeBattle();
                break;
            case EnemyInput.Attack:
                Attack();
                break;
            case EnemyInput.Summon:
                Summon();
                break;
            case EnemyInput.End:
                _duelStateMachine.End();
                break;
            default:
                break;
        }
    }

    public void SetPhase(bool isMainI)
    {
        this.isMainI = isMainI;
    }

    private int RandomChoose(bool isMainI)
    {
        if (isMainI) return RandomMainIPhase();

        return RandomMainIICard();
    }

    private int RandomMainIICard()
    {
        BattleBoard battleBoard = BattleBoard.Instance;
        if (battleBoard == null) return -1;

        List<int> cards = new();

        var isFuncNotFull = battleBoard.GetSlotByType(_duelInformation.character, CardType.Mighty).Any(slot => slot?.CardInstance?.CardReference == null);

        var isSoulNotFull = battleBoard.GetSlotByType(_duelInformation.character, CardType.Soul).Any(slot => slot?.CardInstance?.CardReference == null);

        for (int i = 0; i < _duelInformation.Deck.Hand.Count; i++)
        {
            var card = _duelInformation.Deck.Hand[i];
            if (card?.CardReference?.CardType == CardType.Mighty) continue;
            if (!isFuncNotFull && card?.CardReference?.CardType == CardType.Rupture) continue;
            if (!isSoulNotFull && card?.CardReference?.CardType == CardType.Soul) continue;

            cards.Add(i);
        }

        if (cards.Count == 0)
        {
            _handIndex = -1;
            return -1;
        }

        var random = UnityEngine.Random.Range(0, cards.Count);
        _handIndex = cards[random];

        return _handIndex;
    }

    public int RandomMainIPhase()
    {
        List<int> mightyCards = new();

        for (int i = 0; i < _duelInformation.Deck.Hand.Count; i++)
        {
            var card = _duelInformation.Deck.Hand[i];
            if (card?.CardReference?.CardType != CardType.Mighty) continue;
            mightyCards.Add(i);
        }

        if (mightyCards.Count == 0)
        {
            _handIndex = -1;
            return -1;
        }

        var random = UnityEngine.Random.Range(0, mightyCards.Count);
        _handIndex = mightyCards[random];

        return _handIndex;
    }

    private void Summon()
    {
        var battleBoard = BattleBoard.Instance;
        if (battleBoard == null || _duelInformation == null) return;

        var selectedCard = _duelInformation.SelectedCard;
        if (selectedCard == null || selectedCard.CardReference == null) return;

        var character = _duelInformation.character;
        var cardType = selectedCard.CardReference.CardType;
        var row = battleBoard.GetSlotByType(character, cardType);

        if (row == null) return;

        List<int> emptyIndices = new();

        for (int i = 0; i < row.Length; i++)
        {
            if (!battleBoard.CanSummonCard(selectedCard, character, i)) continue;
            emptyIndices.Add(i);
        }

        if (emptyIndices.Count == 0) return;

        var randomBoardIndex = UnityEngine.Random.Range(0, emptyIndices.Count);

        var boardIndex = emptyIndices[randomBoardIndex];

        var randomStance = UnityEngine.Random.Range(0, 2);

        bool isAttack = selectedCard.CardReference.CardType != CardType.Soul || randomStance == 1;

        _duelInformation.Summon(_handIndex, boardIndex, isAttack);
    }

    public int RandomBoardSlotIndex()
    {
        var battleBoard = BattleBoard.Instance;
        if (battleBoard == null || _duelInformation == null) return -1;

        var selectedCard = _duelInformation.SelectedCard;
        if (selectedCard == null || selectedCard.CardReference == null) return -1;

        var row = battleBoard.GetSlotByType(_duelInformation.character, CardType.Mighty);
        List<int> emptyIndices = new();

        for (int i = 0; i < row.Length; i++)
        {
            if (!battleBoard.CanSummonCard(selectedCard, _duelInformation.character, i)) continue;
            emptyIndices.Add(i);
        }

        if (emptyIndices.Count == 0) return -1;

        var random = UnityEngine.Random.Range(0, emptyIndices.Count);

        return emptyIndices[random];
    }

    public BoardSlot TryChooseTarget(CardInstance source)
    {
        if (_duelInformation == null || _duelStateMachine == null) return null;

        var system = _duelStateMachine.CardEffectSystem;
        if (system == null || source?.CardReference?.Effect == null) return null;

        var targets = system.GetAllValidTarget(source, _duelInformation);
        List<BoardSlot> candidate = new();

        foreach (var target in targets)
        {
            var singleTarget = new List<BoardSlot> { target };

            if (!system.TrySummon(source, _duelInformation, singleTarget)) continue;
            candidate.Add(target);
        }

        if (candidate.Count == 0) return null;

        var random = UnityEngine.Random.Range(0, candidate.Count);
        return candidate[random];
    }

    private void Attack()
    {
        Debug.Log("Attack");
        var battleBoard = BattleBoard.Instance;
        if (battleBoard == null) return;

        List<BoardSlot> attacker = new();
        List<BoardSlot> targeter = new();

        if (battleBoard.EnemySouldField.Length == 0)
        {
            Debug.Log("Can Attack");
            SetInput(EnemyInput.Main_II);
            return;
        }

        foreach (var card in battleBoard.EnemySouldField)
        {
            if (card?.CardInstance?.CardReference == null || card?.CardInstance?.IsAttackStance == false) continue;
            attacker.Add(card);
        }

        if (attacker.Count == 0)
        {
            SetInput(EnemyInput.Main_II);
            return;
        }


        if (battleBoard.PlayerSoulField.Length != 0)
            foreach (var card in battleBoard.PlayerSoulField)
            {
                if (card?.CardInstance?.CardReference == null) continue;
                targeter.Add(card);
            }


        var randomAttacker = UnityEngine.Random.Range(0, attacker.Count);
        var randomTargeter = UnityEngine.Random.Range(0, targeter.Count);

        var targeterCard = targeter.Count == 0 ? null : targeter[randomTargeter];

        battleBoard.SetAttacker(attacker[randomAttacker]);
        battleBoard.SetTarget(targeterCard);

        _duelStateMachine.BattleFlowController.TryBeginAttack();
    }

}
