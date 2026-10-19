using System;
using System.Collections.Generic;
using Raziel.Battle;
using UnityEngine;
using UnityEngine.UI;

public class UiBattleManagers : MonoBehaviour
{
    public static UiBattleManagers Instance;

    [Header("Board UI")]
    [SerializeField] private CardViewPool _cardVỉewPool;

    public CardPlaceholder[] _playerSouls;
    public CardPlaceholder[] _playerFunctionals;
    public CardPlaceholder[] _enemySouls;
    public CardPlaceholder[] _enemyFunctionals;

    [Header("Hand")]
    [SerializeField] private CardHandPool _cardHandPool;
    [SerializeField] private RectTransform _playerHand;
    [SerializeField] private RectTransform _enemyHand;


    private List<CardElement> _playerCardHands = new();
    private List<CardElement> _enemyCardHands = new();

    public Dictionary<BoardSlot, CardElement> _playerBoard = new();
    public Dictionary<BoardSlot, CardElement> _enemyBoard = new();

    public event Action<BoardSlot, int> UpdateStarOnBoard = delegate { };


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetupPlaceholderIndex(_playerSouls);
        SetupPlaceholderIndex(_playerFunctionals);
        SetupPlaceholderIndex(_enemySouls);
        SetupPlaceholderIndex(_enemyFunctionals);
    }

    private void Start()
    {
        BattleBoard.Instance.SummonAction += SpawnCardElementOnBoard;
        UpdateStarOnBoard += UpdateCardStarOnBoard;
    }

    private void OnDisable()
    {
        BattleBoard.Instance.SummonAction -= SpawnCardElementOnBoard;
        UpdateStarOnBoard -= UpdateCardStarOnBoard;
    }

    //Placeholder
    private void SetupPlaceholderIndex(CardPlaceholder[] cardPlaceholders)
    {
        for (int i = 0; i < cardPlaceholders.Length; i++)
        {
            cardPlaceholders[i].slotIndex = i;
        }
    }

    // Board
    public void SpawnCardElementOnBoard(Character character, CardInstance cardInstance, int index, BoardSlot boardSlot)
    {
        var card = _cardVỉewPool.GetObject();

        var parent = GetSlotByType(character, cardInstance.CardReference.CardType, index);

        var parentRect = parent.GetComponent<RectTransform>();

        var rect = card.GetComponent<RectTransform>();

        rect.SetParent(parentRect, false);
        rect.rotation = cardInstance.IsAttackStance ? Quaternion.identity : Quaternion.Euler(0f, 0f, 90f);
        card.transform.localScale = Vector3.one;

        var cardElement = card.GetComponent<CardElement>();
        cardElement.Setup(
            index,
            cardInstance.CardReference.CardDetailImage,
            cardInstance.CardReference.CardElementalImage,
            cardInstance.CardReference.CardTypeImage,
            cardInstance.BattleStar.ToString(),
            cardInstance.IsAttackStance
        );

        var cardButton = card.GetComponent<Button>();


        cardButton.onClick.AddListener(() => BattleBoard.Instance.OnCardCliked(boardSlot));

        if (character == Character.Player)
        {
            _playerBoard.Add(boardSlot, cardElement);
        }
        else
        {
            _enemyBoard.Add(boardSlot, cardElement);
        }
    }

    private CardPlaceholder GetSlotByType(Character character, CardType cardType, int index)
    {
        if (character == Character.Player)
        {
            if (cardType == CardType.Soul)
            {
                return _playerSouls[index];
            }
            return _playerFunctionals[index];
        }

        if (cardType == CardType.Soul)
        {
            return _enemySouls[index];
        }
        return _enemyFunctionals[index];
    }

    public void ReleaseCardOnBoardByCharacterAndIndex(Character character, BoardSlot boardSlot)
    {
        if (boardSlot == null) return;

        var cardView = character == Character.Player ? _playerBoard : _enemyBoard;

        if (!cardView.TryGetValue(boardSlot, out CardElement cardElement)) return;

        cardView.Remove(boardSlot);

        if (cardElement != null)
            _cardVỉewPool.ReleaseObject(cardElement);
    }

    private void UpdateCardStarOnBoard(BoardSlot boardSlot, int starAfter)
    {
        var cardView = boardSlot.Character == Character.Player ? _playerBoard : _enemyBoard;

        if (cardView.TryGetValue(boardSlot, out CardElement card))
        {
            card.CardStar.SetText(starAfter.ToString());
        }
    }

    public void OnUpdateStarOnBoard(BoardSlot boardSlot, int starAfter)
    {
        UpdateStarOnBoard?.Invoke(boardSlot, starAfter);
    }

    // Hand
    public void SpawnCardElementOnHand(Character character, CardInstance cardInstance, int index)
    {
        var card = _cardHandPool.GetObject();

        if (character == Character.Player)
        {
            _playerCardHands.Add(card);
        }
        else
        {
            _enemyCardHands.Add(card);
        }

        var parentRect = character == Character.Player ? _playerHand : _enemyHand;

        var rect = card.GetComponent<RectTransform>();

        rect.SetParent(parentRect, false);

        card.transform.localScale = Vector3.one;

        var cardElement = card.GetComponent<CardElement>();
        cardElement.Setup(
            index,
            cardInstance.CardReference.CardDetailImage,
            cardInstance.CardReference.CardElementalImage,
            cardInstance.CardReference.CardTypeImage,
            cardInstance.CardReference.CardStar.ToString(),
            true
        );
    }

    public void ReleaseAllCardHand(Character character)
    {
        List<CardElement> hands;
        if (character == Character.Player)
        {
            hands = _playerCardHands;
        }
        else
        {
            hands = _enemyCardHands;
        }

        foreach (var card in hands)
        {
            _cardHandPool.ReleaseObject(card);
        }
        hands.Clear();
    }

}
