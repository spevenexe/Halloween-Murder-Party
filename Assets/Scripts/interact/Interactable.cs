using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour, IInteract
{
    public UnityEvent OnHighlight, OnDehighlight;

    protected virtual void Awake()
    {
        if (OnHighlight == null) OnHighlight = new();
        if (OnDehighlight == null) OnDehighlight = new();
    }

    protected virtual void OnDisable()
    {
        OnHighlight.RemoveAllListeners();
        OnDehighlight.RemoveAllListeners();
    }

    public abstract void Activate(PlayerData playerData = null);
    protected abstract void RevealInteractPrompt();
    protected abstract void HideInteractPrompt();
}

public interface IInteract
{
    /// <summary>
    /// Perform the dedicated interaction of the object. Pass <c>playerData</c> if the action involves manipulation of the player.
    /// </summary>
    /// <param name="playerData"></param>
    void Activate(PlayerData playerData = null);
}