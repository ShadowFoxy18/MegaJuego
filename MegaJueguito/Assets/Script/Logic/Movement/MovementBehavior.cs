using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    protected void MoveInDirection(Rigidbody2D body, Vector2 direction, float speed)
    {
        if (body == null)
            return;

        Vector2 movement = Vector2.ClampMagnitude(direction, 1f) * speed * Time.fixedDeltaTime;
        body.MovePosition(body.position + movement);
    }
}
