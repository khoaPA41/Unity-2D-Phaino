using System.Collections;
using Raziel.Battle;
using UnityEngine;
using UnityEngine.UI;
namespace Raziel.StateMachine
{
    public class BattlePhase : DuelBaseState
    {
        BattleBoard _battleBoard;
        private Button _useRuptureCardButton;
        private Button _attackButton;

        private BoardSlot _selectedRupture;

        private BattleFlowController _flow;

        public BattlePhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _battleBoard = _duelStateMachine.BattleBoard;
            _flow = _duelStateMachine.BattleFlowController;

            Debug.Log("BattlePhase: " + _duelStateMachine.CurrentTurn.character);

            // Lam nut chuyen
            //Tam thoi cho tu chuyen
            _flow.ReactionRequested += HandleRequested;
            _flow.AttackFinished += HandleAttackFinished;


            _flow.BeginBattle(_duelStateMachine.CurrentTurn.character);
            ChangePhaseAfterBattle();

            SetupForPlayer();
            SetupForEnemy();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            _duelStateMachine.MainIIButton.SetActive(false);
            _duelStateMachine.AttackButton.SetActive(false);
            _duelStateMachine.MainIIAction -= ChangeMainII;
            if (_flow != null)
            {
                _flow.ReactionRequested -= HandleRequested;
                _flow.AttackFinished -= HandleAttackFinished;

                _flow.EndBattle();
                _flow = null;
            }
            if (_battleBoard != null)
            {
                _battleBoard.CardOnBoardClicked -= GetCard;
                _battleBoard.CardOnBoardClicked -= SetSelectedRupture;
                _battleBoard.SetTarget(null);
                _battleBoard.SetAttacker(null);
            }

            if (_useRuptureCardButton != null)
            {
                _useRuptureCardButton.onClick.RemoveAllListeners();
            }

            if (_attackButton != null)
            {
                _attackButton.onClick.RemoveAllListeners();
            }

            _useRuptureCardButton = null;
            _battleBoard = null;
        }

        private void ChangePhaseAfterBattle()
        {
            _duelStateMachine.MainIIAction += ChangeMainII;
        }

        private void ChangeMainII()
        {
            if (_flow == null || _flow.IsIdle)
                _duelStateMachine.SwitchState(_duelStateMachine.SecondMainPhase);
        }

        private void GetCard(BoardSlot boardSlot)
        {
            if (_flow == null || !_flow.IsIdle) return;

            if (!boardSlot.AcceptSoul) return;
            if (_duelStateMachine.CurrentTurn.character == boardSlot.Character)
            {
                _battleBoard.SetAttacker(boardSlot);
            }
            else
            {
                _battleBoard.SetTarget(boardSlot);
            }
        }

        private void SetupForPlayer()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Player) return;

            _attackButton = _duelStateMachine.AttackButton.GetComponent<Button>();
            _attackButton.onClick.AddListener(PlayerAttack);

            _battleBoard.CardOnBoardClicked += GetCard; // Choose attack and target card

            _duelStateMachine.MainIIButton.SetActive(true);
            _duelStateMachine.AttackButton.SetActive(true);
        }

        private void PlayerAttack()
        {
            _flow.TryBeginAttack();
        }

        private void SetupForEnemy()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Enemy) return;

            _useRuptureCardButton = _duelStateMachine.UseEffectButton.GetComponent<Button>();
            _useRuptureCardButton.onClick.AddListener(HandleUseRuptureClicked);

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Attack);
        }

        private void EnemyFinishBattlePhase()
        {
            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Main_II);
        }

        private void SetSelectedRupture(BoardSlot boardSlot)
        {
            if (_flow == null || !_flow.IsWaitingReaction) return;

            if (boardSlot.Character == _duelStateMachine.CurrentTurn.character) return;

            if (boardSlot.CardInstance.CardReference.CardType != CardType.Rupture) return;
            _selectedRupture = boardSlot;
            Debug.Log(_selectedRupture.CardInstance.CardReference.CardNameId);
        }

        private void HandleUseRuptureClicked()
        {
            if (_flow == null || !_flow.IsWaitingReaction) return;

            var responer = _duelStateMachine.CurrentTurn.character == Character.Player ? Character.Enemy : Character.Player;
            _flow.TryUseRuptureCard(responer, _selectedRupture);

        }

        private void HandleRequested(Character responder, AttackContext attackContext)
        {
            if (responder == Character.Player)
            {
                _selectedRupture = null;
                _battleBoard.CardOnBoardClicked += SetSelectedRupture;
                _useRuptureCardButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Doing");
            }
        }

        private void HandleAttackFinished(AttackContext attack)
        {
            _selectedRupture = null;
            _battleBoard.SetAttacker(null);
            _battleBoard.SetTarget(null);
            _selectedRupture = null;
            _useRuptureCardButton?.gameObject.SetActive(false);

            if (_duelStateMachine.CurrentTurn.character == Character.Enemy)
                EnemyFinishBattlePhase();
        }
    }
}
