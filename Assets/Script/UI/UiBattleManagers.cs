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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        BattleBoard.Instance.SummonAction += SpawnCardElementOnBoard;
    }

    private void OnDisable()
    {
        BattleBoard.Instance.SummonAction -= SpawnCardElementOnBoard;
    }

    // Board
    public void SpawnCardElementOnBoard(Character character, CardInstance cardInstance, int index)
    {
        var card = _cardVỉewPool.GetObject();

        var parent = GetSlotByType(character, cardInstance.CardReference.CardType, index);

        var parentRect = parent.GetComponent<RectTransform>();

        var rect = card.GetComponent<RectTransform>();

        rect.SetParent(parentRect, false);

        card.transform.localScale = Vector3.one;

        var cardElement = card.GetComponent<CardElement>();
        cardElement.Setup(
            cardInstance.CardReference.CardDetailImage,
            cardInstance.CardReference.CardElementalImage,
            cardInstance.CardReference.CardTypeImage,
            cardInstance.CardReference.CardStar.ToString()
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


    public void SpawnCardElementOnHand(Character character, CardInstance cardInstance)
    {
        var card = _cardHandPool.GetObject();


        var parentRect = character == Character.Player ? _playerHand : _enemyHand;

        var rect = card.GetComponent<RectTransform>();

        rect.SetParent(parentRect, false);

        card.transform.localScale = Vector3.one;

        var cardElement = card.GetComponent<CardElement>();
        cardElement.Setup(
            cardInstance.CardReference.CardDetailImage,
            cardInstance.CardReference.CardElementalImage,
            cardInstance.CardReference.CardTypeImage,
            cardInstance.CardReference.CardStar.ToString()
        );
    }
}
