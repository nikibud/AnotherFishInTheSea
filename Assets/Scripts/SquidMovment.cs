using UnityEngine;

public class SquidMovment : MonoBehaviour
{
    [Header("Movement")]
    public float burstForce = 15f;
    public float restDurationIdle = 2.5f;
    public float restDurationRun = 1.5f;
    
    private float burstTimer;

    [Header("Detection")]
    public Transform player;
    public float detectionRange = 5f;
    public float checkDistance = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // High linear drag makes the squid "glide" to a stop naturally
        rb.linearDamping = 2f; 
    }

    void Update()
    {
        burstTimer -= Time.deltaTime;

        if (burstTimer <= 0)
        {
            DecideDirection();
            PerformBurst();
            
        }

        // Always look where we are moving
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            UpdateRotation();
        }
    }

    void DecideDirection()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 targetDir;

        // 1. Determine the "Desired" direction
        if (distToPlayer < detectionRange)
        {
            targetDir = (transform.position - player.position).normalized;
            burstTimer = restDurationRun;
        }
        else
        {
            targetDir = Random.insideUnitCircle.normalized;
            burstTimer = restDurationIdle;

        }

        // 2. THE RAYCAST CHECK: Look ahead to see if the path is clear
        checkDistance = 3f; // How far the squid looks before bursting
        LayerMask wallLayer = LayerMask.GetMask("Walls"); // Make sure your layer name matches!

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, checkDistance, wallLayer);

        if (hit.collider != null)
        {
            // 3. AVOIDANCE: If a wall is in the way, don't burst into it!
            // We calculate a new direction by "reflecting" off the wall normal
            // This makes the squid bounce off the wall angle naturally
            moveDirection = Vector2.Reflect(targetDir, hit.normal).normalized;
            
            // Debug line so you can see the "bounce" in the Scene view
            Debug.DrawRay(transform.position, moveDirection * checkDistance, Color.yellow, 1f);
        }
        else
        {
            // Path is clear, use the original target direction
            moveDirection = targetDir;
        }
    }

    void PerformBurst()
    {
        // ForceMode2D.Impulse gives an instant kick of speed
        rb.AddForce(moveDirection * burstForce, ForceMode2D.Impulse);
    }

    void UpdateRotation()
    {
        // Squid usually swim "butt-first" or "tentacle-first"
        // Adjust the +90 or -90 depending on your art orientation
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}