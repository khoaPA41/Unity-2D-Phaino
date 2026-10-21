using System.Collections;
using Raziel.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Raziel.StateMachine
{
    public class FirstMainPhase : DuelBaseState
    {
        private Coroutine _chooseStanceCoroutine;
        private Button _useMightyCardButton;
        private bool _isUsed;

        private CardInstance _cardActive;
        public FirstMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _duelStateMachine.IsCanUseCard = true;

            _useMightyCardButton = _duelStateMachine.UseMightyButton.GetComponent<Button>();
            _useMightyCardButton.onClick.AddListener(UseCardButton);

            Debug.Log("FirstMainPhase: " + _duelStateMachine.CurrentTurn.character);

            _duelStateMachine.BattleAction += ChangeBattle;
            _duelStateMachine.BattleBoard.CardOnBoardClicked += ActiveFunc;
            SetupForPlayer();
            SetupForEnemy();

            _duelStateMachine.ChooseCardAction += ChooseCard;
            _duelStateMachine.SummonAction += Summon;
        }

        public override void Tick(float deltaTime)
        {
            // Hien nut battle

        }

        public override void Exit()
        {
            _duelStateMachine.BattleButton.SetActive(false);
            _duelStateMachine.IsCanUseCard = false;
            _duelStateMachine.BattleAction -= ChangeBattle;

            _duelStateMachine.BattleBoard.CardOnBoardClicked -= ActiveFunc;

            _duelStateMachine.ChooseCardAction -= ChooseCard;
            _duelStateMachine.SummonAction -= Summon;
            _useMightyCardButton.onClick.RemoveListener(UseCardButton);
            _isUsed = false;
        }

        private void ChangeBattle()
        {
            _duelStateMachine.SwitchState(_duelStateMachine.BattlePhase);
        }

        private void SetupForPlayer()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Player) return;
            _duelStateMachine.BattleButton.SetActive(true);
        }

        private void SetupForEnemy()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Enemy) return;
            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Battle);
        }

        private void UseCardButton()
        {
            _isUsed = true;
        }

        private void ChooseCard(int index)
        {
            if (_chooseStanceCoroutine != null) return;

            _duelStateMachine.CurrentTurn.ChooseCard(index);
        }
        private void ActiveFunc(BoardSlot boardSlot)
        {
            _cardActive.CardReference.Effect.Resolve(
                new CardEffectContext
                {
                    SourceCard = _cardActive,
                    TargetCard = boardSlot.CardInstance
                }
            );

            UiBattleManagers.Instance.OnUpdateStarOnBoard(boardSlot, boardSlot.CardInstance.BattleStar);
        }

        private void Summon(int handIndex, int boardIndex)
        {
            if (_chooseStanceCoroutine != null) return;
            if (_duelStateMachine.CurrentTurn == null) return;

            var selectedCard = _duelStateMachine.CurrentTurn.SelectedCard;

            if (selectedCard == null || selectedCard.CardReference == null) return;

            if (selectedCard.CardReference.CardType == CardType.Soul) return;

            _cardActive = selectedCard;
            _chooseStanceCoroutine = _duelStateMachine.CurrentTurn.StartCoroutine(ChooseStance(handIndex, boardIndex));
        }

        private IEnumerator ChooseStance(int handIndex, int boardIndex)
        {
            _useMightyCardButton.gameObject.SetActive(true);
            _duelStateMachine.IsCanUseCard = false;

            yield return new WaitUntil(() => _isUsed);

            _duelStateMachine.CurrentTurn.Summon(handIndex, boardIndex, true);

            _useMightyCardButton.gameObject.SetActive(true);

            _duelStateMachine.IsCanUseCard = true;
            _chooseStanceCoroutine = null;
        }
    }
}