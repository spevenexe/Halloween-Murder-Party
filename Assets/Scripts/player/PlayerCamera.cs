using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : Singleton<PlayerCamera>
{
    private PlayerData playerData;
    private float xRotation = 0, yRotation = 0;

    protected override void Awake()
    {
        base.Awake();

        UnlockCamera();
        playerData = FindAnyObjectByType<PlayerData>();

        xRotation = transform.rotation.eulerAngles.x;
        yRotation = transform.rotation.eulerAngles.y;

        playerData.CameraTransform = transform; // store reference
    }

    void Update()
    {
        // position
        transform.position = playerData.Head.position;

        if (Cursor.lockState != CursorLockMode.Locked) return;

        // rotation
        // note that the values are flipped when read, since rotations occur *around* an axis.
        xRotation -= playerData.LookDelta.y * playerData.LookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += playerData.LookDelta.x * playerData.LookSensitivity;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // also rotate the y-axis of the player
        playerData.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void LockCamera()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnlockCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool IsLocked => Cursor.lockState == CursorLockMode.Locked;
}