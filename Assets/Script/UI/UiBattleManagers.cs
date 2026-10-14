using UnityEngine;

public class UiBattleManagers : MonoBehaviour
{
    public static UiBattleManagers Instance;

    // [Header("Player Hand")]
    // [SerializeField]

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }





}
