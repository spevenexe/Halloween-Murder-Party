using UnityEngine.Events;

public class PlayerAura : PlayerSystem
{
    public int Aura { get; private set; }

    // event trigger when aura changes. Invokes with new aura level, and signed change in aura
    public UnityEvent<int, int> AuraChanged;

    void Start()
    {
        Aura = playerData.BaseAura;
    }

    public void SetAura(int newAura)
    {
        AuraChanged.Invoke(newAura, newAura - Aura);
        Aura = newAura;
    }

    public void AddAura(int amt)
    {
        Aura += amt;
        AuraChanged.Invoke(Aura, amt);
    }

    public void SubtractAura(int amt) => AddAura(-amt);
}