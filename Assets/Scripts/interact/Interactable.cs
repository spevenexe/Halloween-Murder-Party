using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour, IInteract
{
    public UnityEvent OnHighlight, OnDehighlight;

    void Awake()
    {
        if (OnHighlight == null) OnHighlight = new();
        if (OnDehighlight == null) OnDehighlight = new();
    }

    public abstract void Activate(PlayerData playerData = null);
}

public interface IInteract
{
    /// <summary>
    /// Perform the dedicated interaction of the object. Pass <c>playerData</c> if the action involves manipulation of the player.
    /// </summary>
    /// <param name="playerData"></param>
    void Activate(PlayerData playerData=null);
}