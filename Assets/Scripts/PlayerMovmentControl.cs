using UnityEngine;
using UnityEngine.InputSystem; // 1. Add this!

public class PlayerMovmentControl : MonoBehaviour {
    public Bite bite;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    
    private SpriteRenderer fishBodySprite;

    public Rigidbody2D rb; // Make sure to drag your Rigidbody2D here

    public Vector2 direction;
    void Start()
    {
        // This looks for a child named "FishBody" and gets its SpriteRenderer
        Transform bodyTransform = transform.Find("FishBody");
        
        if (bodyTransform != null) {
            fishBodySprite = bodyTransform.GetComponent<SpriteRenderer>();
        }
    }
     void Update()
    {
        // Check for Space Bar using the New Input System
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            bite.ActiveBite(direction);
        }
    }
    void FixedUpdate() {
        HandleMovement();

        
    }
    void HandleMovement()
    {
        // 1. Get Mouse Position (Keep this in Update if you prefer, but the calculation is fine here)
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        // 2. Rotation logic
        direction = (mouseWorldPos - transform.position).normalized;
        
        // Using -90 because your sprite likely faces 'Up' by default
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        // Use fixedDeltaTime if calling from FixedUpdate for smoother physics sync
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // 3. Flip Logic
        if (mouseWorldPos.x < transform.position.x) {
            fishBodySprite.flipX = true;
        } else {
            fishBodySprite.flipX = false;
        }

        // 4. Calculate the new position
        // We use rb.position instead of transform.position to keep physics accurate
        Vector2 newPos = Vector2.MoveTowards(rb.position, mouseWorldPos, moveSpeed * Time.fixedDeltaTime);

        // 5. Rigidbody Movement (This prevents walking through walls)
        rb.MovePosition(newPos);
    }
}