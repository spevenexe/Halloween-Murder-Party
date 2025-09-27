using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerSystem
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // putting this in `Update` instead of `FixedUpdate` allows movement to feel smoother on throttled CPU execution. Could have performance impact due to physics calculations, however.
    void Update()
    {
        if (PlayerCamera.instance.IsLocked)
        {
            rb.linearVelocity = new(0, rb.linearVelocity.y, 0);
            return;
        }

        // matrix rotation around camera angle
        float cameraAngle = -transform.eulerAngles.y * Mathf.PI / 180f;
        float rotatedXDirection = playerData.MoveDirection.x * Mathf.Cos(cameraAngle) - playerData.MoveDirection.y * Mathf.Sin(cameraAngle);
        float rotatedYDirection = playerData.MoveDirection.x * Mathf.Sin(cameraAngle) + playerData.MoveDirection.y * Mathf.Cos(cameraAngle);
        playerData.MoveDirection = new(rotatedXDirection, rotatedYDirection);


        Vector3 moveDir = new(
            playerData.MoveDirection.x * playerData.Speed,
            rb.linearVelocity.y,
            playerData.MoveDirection.y * playerData.Speed);

        rb.linearVelocity = moveDir;
    }
}