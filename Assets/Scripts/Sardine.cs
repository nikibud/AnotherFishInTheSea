using UnityEngine;

public class Sardine : MonoBehaviour {
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public SpriteRenderer spriteRenderer;

    public void MoveFish(Vector2 direction, float speed) {
        // Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Flip Y logic to keep back facing up
        spriteRenderer.flipY = (direction.x < 0);

        Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        // Move
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}