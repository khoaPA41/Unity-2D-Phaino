using System.Collections;
using Raziel.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Raziel.StateMachine
{
    public class SecondMainPhase : DuelBaseState
    {
        private Coroutine _chooseStanceCoroutine;
        private DuelStateMachine _stateMachine;
        private DuelInformation _duelInformation;
        private BattleBoard _battleBoard;

        private Button _chooseAttackStance;
        private Button _chooseDefenseStance;

        private bool isAtkStance = false;
        private bool isDefStance = false;

        // private bool isChooseStance = false;

        public SecondMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _duelInformation = _duelStateMachine.CurrentTurn;
            _battleBoard = BattleBoard.Instance;

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
            isAtkStance = false;
            isDefStance = false;

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

            if (_chooseStanceCoroutine != null)
            {
                if (_stateMachine != null)
                    _stateMachine.StopCoroutine(_chooseStanceCoroutine);

                _chooseStanceCoroutine = null;
            }

            _chooseAttackStance = null;
            _chooseDefenseStance = null;
            _stateMachine = null;
            _duelInformation = null;
            _battleBoard = null;
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
            if (_chooseStanceCoroutine != null) return;

            _duelInformation.ChooseCard(index);
        }

        private void Summon(int handIndex, int boardIndex)
        {
            if (_chooseStanceCoroutine != null) return;

            _chooseStanceCoroutine = _stateMachine.StartCoroutine(ChooseStance(handIndex, boardIndex));
        }

        private void OnAcceptAtk()
        {
            isAtkStance = true;
        }

        private void OnAcceptDef()
        {
            isDefStance = true;
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

        private IEnumerator ChooseStance(int handIndex, int boardIndex)
        {
            _chooseAttackStance.gameObject.SetActive(true);
            _chooseDefenseStance.gameObject.SetActive(true);
            _stateMachine.IsCanUseCard = false;

            yield return new WaitUntil(() => isAtkStance || isDefStance);

            var isAttackStance = isAtkStance != isDefStance && isAtkStance ? isAtkStance : false;

            Debug.Log(isAttackStance);
            _duelInformation.Summon(handIndex, boardIndex, isAttackStance);

            isAtkStance = false;
            isDefStance = false;

            _chooseAttackStance.gameObject.SetActive(false);
            _chooseDefenseStance.gameObject.SetActive(false);

            _stateMachine.IsCanUseCard = true;
            _chooseStanceCoroutine = null;
        }
    }
}