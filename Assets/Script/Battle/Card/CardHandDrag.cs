using Raziel.Battle;
using Raziel.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHandDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private CardHandParent _rootParent;
    private RectTransform _thisRect;
    private Canvas _canvas;

    private bool _isDragging;

    private DuelStateMachine _duelStateMachine;
    private CardElement _cardElement;

    private CardPlaceholder _pendingSlot;

    // origin 

    private int _originSiblingIndex;
    private Vector3 _originLocalPosition;
    private Quaternion _originLocalRotation;
    private Vector3 _originLocalScale;

    private void Start()
    {
        _cardElement = GetComponent<CardElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        _duelStateMachine = DuelStateMachine.Instance;
        if (_duelStateMachine == null || _duelStateMachine.CurrentTurn.character != Character.Player || !_duelStateMachine.IsCanUseCard) return;

        _isDragging = false;
        _pendingSlot = null;


        _thisRect = gameObject.GetComponent<RectTransform>();
        _rootParent = FindInParents<CardHandParent>(gameObject);
        if (_rootParent == null) return;

        _canvas = FindInParents<Canvas>(gameObject);
        if (_canvas == null) return;

        SetOrigin();

        _thisRect.SetParent(_canvas.transform, false);
        _thisRect.SetAsLastSibling();

        _canvasGroup.blocksRaycasts = false;

        _duelStateMachine.ChooseCard(_cardElement.CardIndex);
        _isDragging = true;
        UpdateDragPos(eventData);
    }

    private void SetOrigin()
    {
        _originSiblingIndex = _thisRect.GetSiblingIndex();
        _originLocalPosition = _thisRect.localPosition;
        _originLocalRotation = _thisRect.localRotation;
        _originLocalScale = _thisRect.localScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDragPos(eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        int handIndex = _cardElement.CardIndex;
        var targetSlot = _pendingSlot;
        var stateMachine = DuelStateMachine.Instance;



        CancelDrag();
        _isDragging = false;
        _pendingSlot = null;

        if (targetSlot == null || stateMachine == null) return;

        DuelStateMachine.Instance.Summon(handIndex, targetSlot.slotIndex);
    }

    private void UpdateDragPos(PointerEventData eventData)
    {
        if (_thisRect == null || !_isDragging) return;

        var rectTransform = (RectTransform)_canvas.transform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            null,
            out Vector2 localPos
            ))
        {
            _thisRect.localPosition = (Vector3)localPos;
        }

    }

    private void CancelDrag()
    {
        _canvasGroup.blocksRaycasts = true;

        if (_rootParent == null) return;

        _thisRect.SetParent(_rootParent.transform, false);
        _thisRect.SetSiblingIndex(_originSiblingIndex);
        _thisRect.GetSiblingIndex();
        _thisRect.localPosition = _originLocalPosition;
        _thisRect.localRotation = _originLocalRotation;
        _thisRect.localScale = _originLocalScale;
    }

    public void SetDropTarget(CardPlaceholder slot)
    {
        if (!_isDragging) return;
        _pendingSlot = slot;
    }

    static public T FindInParents<T>(GameObject thisGameObject) where T : Component
    {
        if (thisGameObject == null) return null;

        var comp = thisGameObject.GetComponent<T>();

        if (comp != null) return comp;

        Transform t = thisGameObject.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
