using System;
using Raziel.Battle;


public enum State
{
    Disable,
    Idle,
    AwaitingReaction,
    Resolving
}

public class BattleFlowController
{
    private BattleBoard _battleBoard;
    private CardEffectSystem _cardEffectSystem;
    private DuelInformation _player;
    private DuelInformation _enemy;


    private AttackContext _attackContext;
    private Character _turnOwner;

    private State _state;

    public event Action<Character, AttackContext> ReactionRequested;
    public event Action<AttackContext> AttackFinished;

    public BattleFlowController(BattleBoard battleBoard, CardEffectSystem cardEffectSystem, DuelInformation player, DuelInformation enemy)
    {
        _battleBoard = battleBoard;
        _cardEffectSystem = cardEffectSystem;
        _player = player;
        _enemy = enemy;
    }

    public void BeginBattle(Character turnOwner)
    {
        _turnOwner = turnOwner;
        _state = State.Idle;
    }

    public void TryBeginAttack()
    {
        if (_state != State.Idle) return;

        _attackContext = new AttackContext
        {
            AttackerSlot = _battleBoard._attackerSlot,
            TargeterSlot = _battleBoard._targetSlot
        };

        if (!_battleBoard.HasRuptureCard())
        {
            ResolveCurrentAttack();
        }
        else
        {
            _state = State.AwaitingReaction;
            ReactionRequested?.Invoke(_battleBoard._targetSlot.Character, _attackContext);
        }
    }

    public void TryUseRuptureCard(Character responder, BoardSlot ruptureSlot)
    {
        if (_state != State.AwaitingReaction) return;

        var ownerSource = responder == Character.Player ? _player : _enemy;

        var valid = _cardEffectSystem.GetAllValidTarget(ruptureSlot.CardInstance, ownerSource);
        if (!valid.Contains(_battleBoard._attackerSlot)) return;

        if (!_cardEffectSystem.TryActiveEffect(ruptureSlot.CardInstance, ownerSource, new[] { _battleBoard._attackerSlot }, _attackContext)) return;
        ResolveCurrentAttack();
    }

    public void PassReaction(Character responder)
    {
        if (_state != State.AwaitingReaction) return;
        if (_attackContext == null) return;

        if (_battleBoard._targetSlot.Character != responder) return;

        ResolveCurrentAttack();
    }

    public void ResolveCurrentAttack()
    {
        if (_attackContext == null) return;
        if (_state == State.Disable || _state == State.Resolving)
            return;

        var attackFinished = _attackContext;
        _state = State.Resolving;

        if (!_attackContext.IsCancelled)
        {
            _battleBoard.Attack();
        }


        if (_state != State.Resolving || !ReferenceEquals(_attackContext, attackFinished)) return;

        _attackContext = null;
        _state = State.Idle;

        AttackFinished?.Invoke(attackFinished);
    }

    public void EndBattle()
    {
        _state = State.Disable;
        _attackContext = null;
    }

}
