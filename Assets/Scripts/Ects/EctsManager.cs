using UnityEngine;

public class EctsManager : MonoBehaviour
{
    public static EctsManager Instance { get; private set; }

    [Header("Configuration")]
    [SerializeField] private int startingEcts = 0;

    public int CurrentEcts { get; private set; }

    public delegate void EctsChangedDelegate(int newEcts);
    public event EctsChangedDelegate OnEctsChanged;

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
        CurrentEcts = startingEcts;
        NotifyEctsChanged();
    }

    public void AddEcts(int amount)
    {
        CurrentEcts += amount;
        NotifyEctsChanged();
    }

    private void NotifyEctsChanged()
    {
        OnEctsChanged?.Invoke(CurrentEcts);
    }
}
