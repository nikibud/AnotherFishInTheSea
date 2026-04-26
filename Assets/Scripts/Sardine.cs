using UnityEngine;

public class Sardine : MonoBehaviour
{
    public Transform player;        // Drag your Player here
    public float detectionRange = 5f;
    public float fleeSpeed = 4f;
    public float rotationSpeed = 5f;

    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // If you don't want to drag the player manually, find them by Tag:
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange) {
            Flee();
        }
    }

    void Flee() {
        // 1. Calculate direction AWAY from player
        Vector2 fleeDirection = (transform.position - player.position).normalized;

        // 2. Rotate to face the direction of flight
        float angle = Mathf.Atan2(fleeDirection.y, fleeDirection.x) * Mathf.Rad2Deg;
        
        // Flip logic similar to your player fish
        if (fleeDirection.x < 0) {
            spriteRenderer.flipY = true;
        } else {
            spriteRenderer.flipY = false;
        }

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 3. Move forward
        transform.position += (Vector3)fleeDirection * fleeSpeed * Time.deltaTime;
    }
}
