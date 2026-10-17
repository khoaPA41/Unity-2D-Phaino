using Raziel.Battle;
using Raziel.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlaceholder : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    public Character character;
    public bool IsAcceptSoul;

    private DuelStateMachine _duelStateMachine;
    public void OnDrop(PointerEventData eventData)
    {
        _duelStateMachine = DuelStateMachine.Instance;
        if (_duelStateMachine == null) return;

        if (_duelStateMachine.CurrentTurn.character != Character.Player) return;
        if (character != Character.Player) return;

        if (eventData.pointerDrag == null) return;

        if (eventData.pointerDrag.TryGetComponent<CardHandDrag>(out var cardDrag))
        {
            cardDrag.SetDropTarget(this);
        }
    }

}
