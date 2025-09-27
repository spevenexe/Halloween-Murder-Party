using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerData : Singleton<PlayerData>
{
    [Header("Input")]
    public InputAction MovementInput;
    public InputAction LookInput;
    public InputAction InteractInput;
    public InputAction AccuseInput;
    public InputAction PauseInput;
    [SerializeField] private float keyboardLookSensitivity;
    public float KeyBoardSensitivity { get => keyboardLookSensitivity; }
    [SerializeField] private float controllerSensitivity;
    public float ControllerSensitivity { get => controllerSensitivity; }

    [Header("Camera")]
    public Vector2 LookDelta { get; set; }
    public Transform CameraTransform { get; set; }
    /// <summary>
    /// the player's head offset, to raise/lower the camera
    /// </summary>
    [SerializeField] private Transform head;
    public Transform Head { get => head; }

    [Header("Interact")]
    public float interactRange = 3f;
    public Interactable Target { get; set; }

    [Header("Movement")]
    public Vector2 MoveDirection { get; set; }
    public float Speed;

    [Header("Accusation")]
    [SerializeField] private int baseAura = 1000;
    public int BaseAura { get => baseAura; }

    [Header("Inventory")]
    public Inventory Inventory { get; private set; } = new(10);

    [Header("Pausing")]
    public bool isPaused = false;

    protected override void Awake()
    {
        base.Awake();

        PlayerInput playerInput = GetComponent<PlayerInput>();
        MovementInput = playerInput.actions.FindAction("Move");
        LookInput = playerInput.actions.FindAction("Look");
        InteractInput = playerInput.actions.FindAction("Interact");
        AccuseInput = playerInput.actions.FindAction("Accuse");
        PauseInput = playerInput.actions.FindAction("Pause");
        isPaused = false;
    }
}
