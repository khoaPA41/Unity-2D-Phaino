using System;
using Raziel.Battle;
using Raziel.Pure.TurnController;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class DuelStateMachine : StateMachine
    {
        public static DuelStateMachine Instance;

        [field: SerializeField] public int TwoFirstTurnBattle { get; private set; } = 2;

        [field: SerializeField] public DuelInformation DuelInformation_I { get; private set; }
        [field: SerializeField] public DuelInformation DuelInformation_II { get; private set; }


        [field: Header("Button")]
        [field: SerializeField] public GameObject DrawButton { get; private set; }
        [field: SerializeField] public GameObject BattleButton { get; private set; }
        [field: SerializeField] public GameObject MainIIButton { get; private set; }
        [field: SerializeField] public GameObject ChooseCardButton { get; private set; }
        [field: SerializeField] public GameObject SummonButton { get; private set; }
        [field: SerializeField] public GameObject EndButton { get; private set; }

        public TurnController TurnController { get; private set; }
        public DuelInformation CurrentTurn { get; set; }

        // State
        public State DrawPhase;
        public State StandbyPhase;
        public State FirstMainPhase;
        public State SecondMainPhase;
        public State BattlePhase;
        public State EndPhase;
        public State Setup;

        // Event
        public event Action DrawCardAction = delegate { };
        public event Action BattleAction = delegate { };
        public event Action MainIIAction = delegate { };
        public event Action<int> ChooseCardAction = delegate { };
        public event Action<int, int> SummonAction = delegate { };
        public event Action EndAction = delegate { };

        // Situation
        public int Turn { get; set; }

        private void Awake()
        {

            if(Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
             Instance = this;
            
            DrawPhase = new DrawPhase(this);
            StandbyPhase = new StandbyPhase(this);
            FirstMainPhase = new FirstMainPhase(this);
            SecondMainPhase = new SecondMainPhase(this);
            BattlePhase = new BattlePhase(this);
            EndPhase = new EndPhase(this);
            Setup = new SetupPhase(this);
            TurnController = new();
        }

        private void Start()
        {
            SwitchState(Setup);
        }

        public void Draw()
        {
            DrawCardAction?.Invoke();
        }

        public void Battle()
        {
            BattleAction?.Invoke();
        }

        public void MainII()
        {
            MainIIAction?.Invoke();
        }

        public void ChooseCard(int index)
        {
            ChooseCardAction?.Invoke(index);
        }

        public void Summon(int handIndex, int boardIndex)
        {
            SummonAction?.Invoke(handIndex, boardIndex);
        }

        public void End()
        {
            EndAction?.Invoke();
        }
    }
}