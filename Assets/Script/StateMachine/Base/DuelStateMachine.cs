using Raziel.Battle;
using Raziel.Pure.TurnController;
using UnityEngine;
using UnityEngine.Rendering;

namespace Raziel.StateMachine
{
    public class DuelStateMachine : StateMachine
    {
        [field: SerializeField] public DuelInformation DuelInformation_I { get; private set; }
        [field: SerializeField] public DuelInformation DuelInformation_II { get; private set; }

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

        private void Awake()
        {
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
            SwitchState(StandbyPhase);
        }


        public void Battle()
        {
            SwitchState(BattlePhase);
        }

        public void MainII()
        {
            SwitchState(SecondMainPhase);
        }
        public void End()
        {
            SwitchState(EndPhase);
        }
    }
}