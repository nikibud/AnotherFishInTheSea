using UnityEngine;
using System.Collections.Generic;

public class SchoolManager : MonoBehaviour {
    public GameObject sardinePrefab;
    public int schoolSize = 10;
    public float spawnRadius = 3f;
    
    [Header("Detection")]
    public Transform player;
    public float detectionRange = 6f;
    public float fleeSpeed = 6f;
    public float idleSpeed = 1f;

    private List<Sardine> sardines = new List<Sardine>();

   
    void Start() {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;

        for (int i = 0; i < schoolSize; i++) {
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            
            // --- THE CHANGE IS HERE ---
            // 'this.transform' makes the school the parent of the new sardine
            GameObject newFishObj = Instantiate(sardinePrefab, randomPos, Quaternion.identity, this.transform);
            
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
        float currentSpeed;

        if (distToPlayer < detectionRange) {
            // FLEE: Move away from player
            moveDir = (transform.position - player.position).normalized;
            currentSpeed = fleeSpeed;
            // Move the center of the school away too
            transform.position += (Vector3)moveDir * fleeSpeed * Time.deltaTime;
        } else {
            // IDLE: Just drift or stay put
            moveDir = Vector2.right; // Or some wandering logic
            currentSpeed = idleSpeed;
        }

        // Tell every fish in the school to move in that direction
        foreach (Sardine fish in sardines) {
            if (fish != null) { // Safety check
                fish.MoveFish(moveDir, currentSpeed);
            }
            else
            {
                //Debug.Log(fish);
            }
        }
    }
}