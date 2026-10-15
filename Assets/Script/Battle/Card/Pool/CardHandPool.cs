using UnityEngine;
using UnityEngine.Pool;

public class CardHandPool : MonoBehaviour
{
    [SerializeField] private GameObject _cardHandPrefab;

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
        var pooledObject = Instantiate(_cardHandPrefab).GetComponent<CardElement>();
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
        _pool.Release(cardElement);
    }
}
