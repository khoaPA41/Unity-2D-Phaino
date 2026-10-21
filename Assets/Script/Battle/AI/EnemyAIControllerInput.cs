using System;
using System.Collections.Generic;
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
                _duelStateMachine.ChooseCard(RandomChoose());
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

    private int RandomChoose()
    {
        _handIndex = UnityEngine.Random.Range(0, _duelInformation.Deck.Hand.Count);
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

    private void Attack()
    {
        Debug.Log("Attack");
        var battleBoard = BattleBoard.Instance;
        if (battleBoard == null) return;

        List<BoardSlot> attacker = new();
        List<BoardSlot> targeter = new();

        if (battleBoard.EnemySouldField.Length == 0)
        {
            return;
        }

        foreach (var card in battleBoard.EnemySouldField)
        {
            if (card?.CardInstance?.CardReference == null || card?.CardInstance?.IsAttackStance == false) continue;
            attacker.Add(card);
        }

        if (attacker.Count == 0) return;


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
        battleBoard.Attack();
    }
}
