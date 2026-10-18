using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
public class CardViewPool : MonoBehaviour
{
    [SerializeField] private GameObject _cardVỉewPrefab;

    private ObjectPool<CardElement> _pool;

    void Awake()
    {
        _pool = new ObjectPool<CardElement>(
            createFunc: CreateItem,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyObject,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 50
        );
    }

    private CardElement CreateItem()
    {
        var pooledObject = Instantiate(_cardVỉewPrefab).GetComponent<CardElement>();
        pooledObject.name = pooledObject.name;
        pooledObject.gameObject.SetActive(false);

        return pooledObject;
    }

    private void OnGet(CardElement pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnRelease(CardElement pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyObject(CardElement pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    public CardElement GetObject()
    {
        return _pool.Get();
    }

    public void ReleaseObject(CardElement cardElement)
    {
        var button = cardElement.GetComponent<Button>();

        if (button != null)
            button.onClick.RemoveAllListeners();

        _pool.Release(cardElement);
    }
}
