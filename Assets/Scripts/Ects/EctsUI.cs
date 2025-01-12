using TMPro;
using UnityEngine;

public class EctsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI ectsText;

    // Direct reference to the manager
    [SerializeField] private EctsManager manager;

    private void OnEnable()
    {
        // Subscribe to the manager we have wired in via the Inspector
        manager.OnEctsChanged += UpdateUI;
    }

    private void OnDisable()
    {
        manager.OnEctsChanged -= UpdateUI;
    }

    private void Start()
    {
        // Initialize UI to whatever the manager currently has
        UpdateUI(manager.CurrentEcts);
    }

    private void UpdateUI(int newEcts)
    {
        ectsText.text = newEcts.ToString();
    }
}