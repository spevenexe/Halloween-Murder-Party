using TMPro;
using UnityEngine;

public class InteractionLimitUI : Singleton<InteractionLimitUI>
{
    [SerializeField] private TMP_Text counter;

    void OnEnable()
    {
        InteractionLimitManager.instance.interactionsChanged.AddListener(SetCounter);
    }

    void OnDisable()
    {
        InteractionLimitManager.instance.interactionsChanged.RemoveListener(SetCounter);
    }

    void SetCounter(int newValue, int change)
    {
        counter.text = $"{newValue} Interactions Remaining";
    }
}