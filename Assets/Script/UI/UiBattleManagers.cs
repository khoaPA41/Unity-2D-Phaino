using System.Collections.Generic;
using Raziel.Battle;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        BattleBoard.Instance.SummonAction -= SpawnCardElementOnBoard;
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
    public void SpawnCardElementOnBoard(Character character, CardInstance cardInstance, int index)
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
            cardInstance.CardReference.CardStar.ToString(),
            cardInstance.IsAttackStance
        );
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
