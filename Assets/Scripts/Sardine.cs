using UnityEngine;

public class Sardine : MonoBehaviour {
    public float moveSpeed = 8f;
    public float rotationSpeed = 5f;
    public SpriteRenderer spriteRenderer;
    public float obstacleRange = 3f; // How far the fish looks ahead
    public LayerMask wallLayer;      // Set this to your 'Walls' layer
    public Rigidbody2D rb;

    public float speedMult;
    public float currentSpeed ;


    public void MoveFish(Vector2 leaderDir, Vector2 centerPos) 
    {
        // 1. Calculate the vector pointing exactly at the leader
        Vector2 offset = centerPos - (Vector2)transform.position;
        float distance = offset.magnitude;
        Vector2 pullToCenter = offset.normalized;

        // 2. The Blend: How much do I listen to the Leader vs. my urge to be in the group?
        // If distance is small, follow leaderDir. If distance is large, prioritize pullToCenter.
        float cohesionStrength = Mathf.Clamp01(distance / 4f); 
        
        // We blend the leader's move direction with the "home" direction
        Vector2 combinedDir = Vector2.Lerp(leaderDir, pullToCenter, cohesionStrength).normalized;

        // 3. Wall Avoidance (Must happen AFTER blending)
        
        Vector2 finalDir = GetSteeredDirection(combinedDir);
        // 4. Dynamic Speed
        // If they are behind, they swim faster to catch up to that centerPos
        speedMult = 1f;
        if (distance > 2f) speedMult = Mathf.Clamp(distance / 2f, 1f, 10f);
        currentSpeed = moveSpeed * speedMult;
        rb.MovePosition(rb.position + finalDir * (moveSpeed * speedMult) * Time.deltaTime);
        
        // Update visuals...
    }

    Vector2 GetSteeredDirection(Vector2 desiredDir) {
        // 1. Define the three directions to check
        Vector2[] directionsToCheck = new Vector2[] {
            desiredDir,                                          // Straight
            Quaternion.Euler(0, 0, 30) * desiredDir,            // 30 degrees Left
            Quaternion.Euler(0, 0, -30) * desiredDir            // 30 degrees Right
        };

        foreach (Vector2 dir in directionsToCheck) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, obstacleRange, wallLayer);
            
            // If this specific direction is clear, take it!
            if (hit.collider == null) {
                return dir;
            }
        }

        // 2. If ALL directions are blocked, we need to turn hard (90 degrees)
        // This happens in dead ends or sharp corners
        Vector2 escapeDir = Vector2.Perpendicular(desiredDir);
        return escapeDir;
    }
}