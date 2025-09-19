using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteract : PlayerSystem
{
    [SerializeField] private float interactRange = 3f;

    protected override void Awake()
    {
        base.Awake();

        // prevents race condition where InteractInput is not yet assigned
        playerData.InteractInput = GetComponent<PlayerInput>().actions.FindAction("Interact");
    }

    void OnEnable()
    {
        playerData.InteractInput.performed += Use;
    }

    void OnDisable()
    {
        playerData.InteractInput.performed -= Use;
    }

    private void Use(InputAction.CallbackContext context)
    {
        // look for something to use
        Ray ray = new(playerData.CameraTransform.position,
            playerData.CameraTransform.forward);
        RaycastHit[] hits = new RaycastHit[1];

        int length = Physics.RaycastNonAlloc(ray, hits,interactRange,LayerMask.GetMask("Interact"));
        // if we hit something, try and use it
        if (length > 0)
        {
            RaycastHit hit = hits[0];

            Interactable target = hit.transform.GetComponent<Interactable>();

            target.Activate(playerData);
        }

    }
}