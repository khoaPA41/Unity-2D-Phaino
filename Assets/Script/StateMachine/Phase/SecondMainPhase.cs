using System.Collections;
using Raziel.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Raziel.StateMachine
{
    public class SecondMainPhase : DuelBaseState
    {
        private DuelStateMachine _stateMachine;
        private DuelInformation _duelInformation;

        private Button _chooseAttackStance;
        private Button _chooseDefenseStance;


        private int _currentHandIndex;
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

            _chooseAttackStance = null;
            _chooseDefenseStance = null;
            _stateMachine = null;
            _duelInformation = null;
            _currentHandIndex = -1;
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

        private void Summon(int handIndex, int boardIndex)
        {
            if (_duelInformation == null) return;

            var selectedCard = _duelInformation.SelectedCard;

            if (selectedCard == null || selectedCard.CardReference == null) return;

            if (selectedCard.CardReference.CardType != CardType.Soul)
            {
                if (selectedCard.CardReference.CardType == CardType.Rupture)
                    SummonFuncCard(handIndex, boardIndex);
                return;
            }

            _currentHandIndex = handIndex;
            _currentBoardIndex = boardIndex;

            _chooseAttackStance.gameObject.SetActive(true);
            _chooseDefenseStance.gameObject.SetActive(true);
            _stateMachine.IsCanUseCard = false;
        }

        private void OnAcceptAtk()
        {
            _duelInformation.Summon(_currentHandIndex, _currentBoardIndex, true);

            _stateMachine.IsCanUseCard = true;

            _currentHandIndex = -1;
            _currentBoardIndex = -1;

            _chooseAttackStance.gameObject.SetActive(false);
            _chooseDefenseStance.gameObject.SetActive(false);
        }

        private void OnAcceptDef()
        {
            _duelInformation.Summon(_currentHandIndex, _currentBoardIndex, false);

            _stateMachine.IsCanUseCard = true;

            _currentHandIndex = -1;
            _currentBoardIndex = -1;

            _chooseAttackStance.gameObject.SetActive(false);
            _chooseDefenseStance.gameObject.SetActive(false);
        }

        private void SetupForPlayer()
        {
            if (_duelInformation.character != Character.Player) return;

            _chooseAttackStance = _stateMachine.AtkButton.GetComponent<Button>();
            _chooseDefenseStance = _stateMachine.DefButton.GetComponent<Button>();

            _chooseAttackStance.onClick.AddListener(OnAcceptAtk);
            _chooseDefenseStance.onClick.AddListener(OnAcceptDef);

            _stateMachine.EndButton.SetActive(true);
            _stateMachine.ChooseCardButton.SetActive(true);
            _stateMachine.SummonButton.SetActive(true);
        }

        private void SetupForEnemy()
        {
            if (_duelInformation.character != Character.Enemy) return;

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.ChooseCard);

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Summon);

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.End);
        }

        private void SummonFuncCard(int handIndex, int boardIndex)
        {
            _duelInformation.Summon(handIndex, boardIndex, true);
        }
    }
}