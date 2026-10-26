using Raziel.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Raziel.StateMachine
{
    public class FirstMainPhase : DuelBaseState
    {
        private Button _useMightyCardButton;
        private BoardSlot _targetSlot;
        private CardInstance _cardActive;
        public FirstMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _duelStateMachine.IsCanUseCard = true;

            _useMightyCardButton = _duelStateMachine.UseEffectButton.GetComponent<Button>();
            _useMightyCardButton.onClick.AddListener(UseCardButton);

            Debug.Log("FirstMainPhase: " + _duelStateMachine.CurrentTurn.character);

            _duelStateMachine.BattleAction += ChangeBattle;

            _duelStateMachine.BattleBoard.CardOnBoardClicked += ChooseCardTarget;

            _duelStateMachine.ChooseCardAction += ChooseCard;
            _duelStateMachine.SummonAction += Summon;

            SetupForPlayer();
            SetupForEnemy();
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
            _duelStateMachine.BattleBoard.CardOnBoardClicked -= ChooseCardTarget;
            _duelStateMachine.ChooseCardAction -= ChooseCard;
            _duelStateMachine.SummonAction -= Summon;

            _useMightyCardButton.gameObject.SetActive(false);
            _useMightyCardButton.onClick.RemoveListener(UseCardButton);

            _cardActive = null;
            _targetSlot = null;
        }

        private void ChangeBattle()
        {
            if (_cardActive != null) return;
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

            var ai = _duelStateMachine.EnemyInput;

            var system = _duelStateMachine.CardEffectSystem;

            var handIndex = ai.RandomMainIPhase();

            if (handIndex < 0)
            {
                ChangeBattle();
                return;
            }

            ChooseCard(handIndex);

            var source = _duelStateMachine.CurrentTurn.SelectedCard;

            if (source?.CardReference?.Effect == null)
            {
                ChangeBattle();
                return;
            }

            int boarIndex = ai.RandomBoardSlotIndex();

            if (boarIndex < 0)
            {
                ChangeBattle();
                return;
            }

            bool needTarget = system.IsNeedTarget(source);

            BoardSlot target = null;
            if (needTarget)
            {
                target = ai.TryChooseTarget(source);

                if (target == null)
                {
                    ChangeBattle();
                    return;
                }
            }

            Summon(_duelStateMachine.EnemyInput.RandomBoardSlotIndex());
            if (needTarget)
            {
                if (_cardActive != source)
                {
                    ChangeBattle();
                    return;
                }
                ChooseCardTarget(_duelStateMachine.EnemyInput.TryChooseTarget(_cardActive));
                UseCardButton();
            }
            ChangeBattle();
        }

        private void UseCardButton()
        {
            if (_cardActive == null || _targetSlot == null) return;
            if (!_duelStateMachine.CardEffectSystem.TryActiveEffect(_cardActive, _duelStateMachine.CurrentTurn, new[] { _targetSlot })) return;

            _cardActive = null;
            _targetSlot = null;
            _useMightyCardButton.gameObject.SetActive(false);
        }

        private void ChooseCard(int index) // Save to Selected Card when drag
        {
            _duelStateMachine.CurrentTurn.ChooseCard(index);
        }

        private void Summon(int boardIndex) // summon mighty card, and save card to _cardActive
        {
            if (_cardActive != null || _duelStateMachine.CurrentTurn == null) return;

            var owner = _duelStateMachine.CurrentTurn;
            var system = _duelStateMachine.CardEffectSystem;
            var selectedCard = owner.SelectedCard;

            if (selectedCard == null || selectedCard.CardReference == null) return;
            if (selectedCard.CardReference.CardType != CardType.Mighty) return;

            if (!system.TrySummon(selectedCard, owner, system.GetAllValidTarget(selectedCard, owner))) return;

            if (system.IsNeedTarget(selectedCard) && system.GetAllValidTarget(selectedCard, owner).Count == 0) return;

            if (!owner.Summon(boardIndex, true)) return;

            _targetSlot = null;
            _useMightyCardButton.gameObject.SetActive(false);

            if (selectedCard.CardReference.Effect == null) return;
            if (system.IsNeedTarget(selectedCard)) // if need target not try Active Immediately
            {
                _cardActive = selectedCard;
                return;
            }
            system.TryActiveEffect(selectedCard, owner, null);
        }

        private void ChooseCardTarget(BoardSlot boardSlot)
        {
            if (_cardActive == null) return;
            var valid = _duelStateMachine.CardEffectSystem.GetAllValidTarget(_cardActive, _duelStateMachine.CurrentTurn);


            if (!valid.Contains(boardSlot)) return;

            _targetSlot = boardSlot;
            _useMightyCardButton.gameObject.SetActive(true);
        }
    }
}