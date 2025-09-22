using UnityEngine;
using UnityEngine.Events;

public class InteractionLimitManager : DontDestroySingleton<InteractionLimitManager>
{
    [SerializeField] private int numInteracts = 5;
    public int NumInteracts { get => numInteracts; }
    // trigger when the number of interacts remaining changes, passes in the new number of interacts and the signed amount of change.
    public UnityEvent<int, int> interactionsChanged;

    // note that the setter does NOT invoke the change event
    public void SetInteracts(int amt)
    {
        numInteracts = amt;
    }

    public void IncreaseInteracts(int amt = 1)
    {
        if (numInteracts + amt < 0)
            amt = -numInteracts; // avoiding negatives
        numInteracts += amt;
        if (amt != 0) interactionsChanged.Invoke(numInteracts, amt);
    }

    public void DecreaseInteracts(int amt = 1) => IncreaseInteracts(-amt);

    void OnDisable()
    {
        interactionsChanged.RemoveAllListeners();
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        interactionsChanged.AddListener(DebugDie);
    }


    void DebugDie(int numInteracts, int delta)
    {
        if(numInteracts == 0) Debug.Log("Out of Interactions!");
    }
#endif
}