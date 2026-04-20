using UnityEngine;
using UnityEngine.InputSystem; // 1. Add this!

public class PlayerMovmentControl : MonoBehaviour {
    public Bite bite;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    
    private SpriteRenderer fishBodySprite;

    public Vector2 direction;
    void Start()
    {
        // This looks for a child named "FishBody" and gets its SpriteRenderer
        Transform bodyTransform = transform.Find("FishBody");
        
        if (bodyTransform != null) {
            fishBodySprite = bodyTransform.GetComponent<SpriteRenderer>();
        }
    }
    void Update() {
        HandleMovement();

        // Check for Space Bar using the New Input System
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            bite.ActiveBite(direction);
        }
    }
    void HandleMovement()
    {
        // 2. Use Mouse.current instead of Input.mousePosition
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        // Rotation logic
        direction = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // 2. Flip Logic: Keep the fish right-side up
        if (mouseWorldPos.x < transform.position.x) {
            fishBodySprite.flipX = true;
        } else {
            fishBodySprite.flipX = false;
        }
        // Movement logic
        transform.position = Vector2.MoveTowards(transform.position, mouseWorldPos, moveSpeed * Time.deltaTime);
    }
}