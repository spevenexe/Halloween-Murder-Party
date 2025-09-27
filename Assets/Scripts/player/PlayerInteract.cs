using DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteract : PlayerSystem
{

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        playerData.InteractInput.performed += Interact;
    }

    void OnDisable()
    {
        playerData.InteractInput.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (playerData.Target != null)
        {
            playerData.Target.Activate(playerData);
        }
    }
}