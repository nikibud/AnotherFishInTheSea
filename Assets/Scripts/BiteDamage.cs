using UnityEngine;

public class BiteDamage : MonoBehaviour
{
    public int damageAmount = 10;
    private bool canDamage = false;

    
    public void SetBiteActive(bool active) {
        canDamage = active;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // Only deal damage if the fish is actually "biting"
        if (canDamage && other.CompareTag("Enemy")) {
            // Check if the enemy has a script to receive damage
            // Replace 'EnemyHealth' with your actual enemy script name
            if (other.TryGetComponent(out EnemyHealth enemy)) {
                enemy.TakeDamage(damageAmount);
                Debug.Log("Hit " + other.name + "!");
            }
        }
    }
}
