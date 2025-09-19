using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public abstract class PlayerSystem : MonoBehaviour
{
    protected PlayerData playerData;

    protected virtual void Awake()
    {
        playerData = GetComponent<PlayerData>();
    }
}