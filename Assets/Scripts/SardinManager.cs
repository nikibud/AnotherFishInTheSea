using UnityEngine;
using System.Collections.Generic;

public class SchoolManager : MonoBehaviour {
    public GameObject sardinePrefab;
    public int schoolSize = 10;
    public float spawnRadius = 3f;
    public Transform sardineHolder;
    
    [Header("Detection")]
    public Transform player;
    public float detectionRange = 6f;
    public float fleeSpeed = 6f;
    public float idleSpeed = 1f;
    private float wanderTimer;
    
    private Vector2 wanderDirection;
    public LayerMask wallLayer;      // Set this to your 'Walls' layer
    public float obstacleRange = 4f; // How far the fish looks ahead
    private List<Sardine> sardines = new List<Sardine>();

    public float currentSpeed;
   
    void Start() {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;

        for (int i = 0; i < schoolSize; i++) {
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            
            // --- THE CHANGE IS HERE ---
            // 'this.transform' makes the school the parent of the new sardine
            GameObject newFishObj = Instantiate(sardinePrefab, randomPos, Quaternion.identity, sardineHolder);
            
            Sardine sardineScript = newFishObj.GetComponentInChildren<Sardine>();

            if (sardineScript != null) {
                sardines.Add(sardineScript);
            } else {
                Debug.LogError("Could not find Sardine script on spawned prefab!");
            }
        }
    }
    void Update() {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 moveDir;

        if (distToPlayer < detectionRange) {
            // FLEE: Move away from player
            moveDir = (transform.position - player.position).normalized;
            currentSpeed = fleeSpeed;
            
        } else {
            // IDLE: Just drift or stay put
            UpdateWanderDirection();
            moveDir = Vector2.right; // Or some wandering logic
            currentSpeed = idleSpeed;
        }
        
        Vector2 safeMoveDir = GetSteeredDirection(moveDir);
        // Move the center of the school away too
        transform.position += (Vector3)safeMoveDir * currentSpeed * Time.deltaTime;

        // Tell every fish in the school to move in that direction
        foreach (Sardine fish in sardines) {
            if (fish != null) { // Safety check
                fish.MoveFish(moveDir, transform.position);
            }
            else
            {
                //Debug.Log(fish);
            }
        }
    }
    
    /*
    Vector2 GetSteeredDirection(Vector2 desiredDir) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, desiredDir, obstacleRange, wallLayer);

        if (hit.collider != null) {
            Vector2 hitNormal = hit.normal;
            Vector2 escapeDir = Vector2.Perpendicular(hitNormal);

            // Make sure escapeDir points generally in the direction we want to go
            if (Vector2.Dot(desiredDir, -escapeDir) > Vector2.Dot(desiredDir, escapeDir)) {
                escapeDir = -escapeDir;
            }

            return Vector2.Lerp(desiredDir, escapeDir, 0.7f).normalized;
        }

        return desiredDir;
    }*/
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
    void UpdateWanderDirection() {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0) {
            // Pick a new direction with a slight jitter for smooth turning
            wanderDirection = (wanderDirection + Random.insideUnitCircle * 0.5f).normalized;
            wanderTimer = Random.Range(2f, 5f);
        }
    }

    
}