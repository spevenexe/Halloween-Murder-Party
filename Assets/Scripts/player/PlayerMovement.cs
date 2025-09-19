using UnityEngine;

public class PlayerMovement : PlayerSystem
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // matrix rotation around camera angle
        float cameraAngle = -transform.eulerAngles.y * Mathf.PI / 180f;
        float rotatedXDirection = playerData.MoveDirection.x * Mathf.Cos(cameraAngle) - playerData.MoveDirection.y * Mathf.Sin(cameraAngle);
        float rotatedYDirection = playerData.MoveDirection.x * Mathf.Sin(cameraAngle) + playerData.MoveDirection.y * Mathf.Cos(cameraAngle);
        playerData.MoveDirection = new Vector2(rotatedXDirection, rotatedYDirection);


        Vector3 moveDir = new Vector3(
            playerData.MoveDirection.x * playerData.Speed,
            rb.linearVelocity.y,
            playerData.MoveDirection.y * playerData.Speed);

        rb.linearVelocity = moveDir;
    }
}