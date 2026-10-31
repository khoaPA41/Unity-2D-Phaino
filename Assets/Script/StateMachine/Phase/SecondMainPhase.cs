using System.Collections;
using System.Collections.Generic;
using Raziel.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Raziel.StateMachine
{
    public class SecondMainPhase : DuelBaseState
    {
        private DuelStateMachine _stateMachine;
        private DuelInformation _duelInformation;

        private Button _summon;
        private Button _chooseAttackStance;
        private Button _chooseDefenseStance;
        private int _currentBoardIndex;

        public SecondMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _duelInformation = _duelStateMachine.CurrentTurn;

            _stateMachine.IsCanUseCard = true;

            Debug.Log("SecondMainPhase: " + _stateMachine.CurrentTurn.character);
            ChangePhaseAfterMain_II();

            _stateMachine.ChooseCardAction += ChooseCard;
            _stateMachine.SummonAction += Summon;

            SetupForPlayer();
            SetupForEnemy();
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {
            if (_stateMachine != null)
            {
                _stateMachine.IsCanUseCard = false;
                _stateMachine.EndButton.SetActive(false);
                _stateMachine.ChooseCardButton.SetActive(false);
                _stateMachine.SummonButton.SetActive(false);
                _stateMachine.EndAction -= ChangeEnd;
                _stateMachine.SummonAction -= Summon;
                _stateMachine.ChooseCardAction -= ChooseCard;
            }

            if (_chooseAttackStance != null)
            {
                _chooseAttackStance.onClick.RemoveListener(OnAcceptAtk);
                _chooseAttackStance.gameObject.SetActive(false);

            }

            if (_chooseDefenseStance != null)
            {
                _chooseDefenseStance.onClick.RemoveListener(OnAcceptDef);
                _chooseDefenseStance.gameObject.SetActive(false);
            }

            if (_summon != null)
            {
                _summon.onClick.RemoveListener(SummonAction);
                _summon.gameObject.SetActive(false);
            }

            _chooseAttackStance = null;
            _chooseDefenseStance = null;
            _stateMachine = null;
            _duelInformation = null;
            _currentBoardIndex = -1;
        }

        private void ChangePhaseAfterMain_II()
        {
            _stateMachine.EndAction += ChangeEnd;
        }

        private void ChangeEnd()
        {
            _stateMachine.SwitchState(_stateMachine.EndPhase);
        }

        private void ChooseCard(int index)
        {
            _duelInformation.ChooseCard(index);
        }

        private void Summon(int boardIndex)
        {
            if (_duelInformation == null) return;

            var selectedCard = _duelInformation.SelectedCard;

            if (selectedCard == null || selectedCard.CardReference == null) return;

            if (selectedCard.CardReference.CardType != CardType.Soul)
            {
                if (selectedCard.CardReference.CardType == CardType.Rupture)
                    SummonFuncCard(boardIndex);
                return;
            }

            _currentBoardIndex = boardIndex;

            _chooseAttackStance.gameObject.SetActive(true);
            _chooseDefenseStance.gameObject.SetActive(true);
            _stateMachine.IsCanUseCard = false;
        }

        private void OnAcceptAtk()
        {
            _duelInformation.Summon(_currentBoardIndex, true);

            _stateMachine.IsCanUseCard = true;

            _currentBoardIndex = -1;
            _chooseAttackStance.gameObject.SetActive(false);
            _chooseDefenseStance.gameObject.SetActive(false);
        }

        private void OnAcceptDef()
        {
            _duelInformation.Summon(_currentBoardIndex, false);

            _stateMachine.IsCanUseCard = true;

            _currentBoardIndex = -1;
            _chooseAttackStance.gameObject.SetActive(false);
            _chooseDefenseStance.gameObject.SetActive(false);
        }

        private void SetupForPlayer()
        {
            if (_duelInformation.character != Character.Player) return;

            _chooseAttackStance = _stateMachine.AtkButton.GetComponent<Button>();
            _chooseDefenseStance = _stateMachine.DefButton.GetComponent<Button>();
            _summon = _stateMachine.SummonButton.GetComponent<Button>();

            _chooseAttackStance.onClick.AddListener(OnAcceptAtk);
            _chooseDefenseStance.onClick.AddListener(OnAcceptDef);
            _summon.onClick.AddListener(SummonAction);

            _stateMachine.EndButton.SetActive(true);
            _stateMachine.ChooseCardButton.SetActive(true);
            _stateMachine.SummonButton.SetActive(true);
        }

        private void SetupForEnemy()
        {
            if (_duelInformation.character != Character.Enemy) return;

            _duelStateMachine.EnemyInput.SetPhase(false);

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.ChooseCard);

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Summon);

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.End);
        }

        private void SummonFuncCard(int boardIndex)
        {
            _duelInformation.Summon(boardIndex, true);
        }

        private void SummonAction()
        {
            RandomBoard();
            _duelStateMachine.Summon(_currentBoardIndex);
        }

        private void RandomBoard()
        {
            var cardType = _duelStateMachine.CurrentTurn.SelectedCard.CardReference.CardType == CardType.Soul ? CardType.Soul : CardType.Mighty;

            var soulSlots = _duelStateMachine.BattleBoard.GetSlotByType(_duelStateMachine.CurrentTurn.character, cardType);

            List<int> slots = new();

            for (int i = 0; i < soulSlots.Length; i++)
            {
                var slot = soulSlots[i];
                if (slot?.CardInstance?.CardReference != null) continue;
                slots.Add(i);
            }

            var randomBoarIndex = Random.Range(0, slots.Count);
            _currentBoardIndex = slots[randomBoarIndex];
        }
    }
}