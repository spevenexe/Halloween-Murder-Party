using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    private PlayerData playerData;
    private float xRotation = 0, yRotation = 0;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerData = FindAnyObjectByType<PlayerData>();

        xRotation = transform.rotation.eulerAngles.x;
        yRotation = transform.rotation.eulerAngles.y;

        playerData.CameraTransform = transform; // store reference
    }

    void Update()
    {
        // position
        transform.position = playerData.Head.position;

        // rotation
        // note that the values are flipped when read, since rotations occur *around* an axis.
        xRotation -= playerData.LookDelta.y * playerData.LookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += playerData.LookDelta.x * playerData.LookSensitivity;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // also rotate the y-axis of the player
        playerData.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}