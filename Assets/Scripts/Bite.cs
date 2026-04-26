using UnityEngine;
using System.Collections;

public class Bite : MonoBehaviour
{
    
    public float biteLungeDistance = 2f;
    public float biteCooldown = 0.5f;
    public float lungeDuration = 0.1f; // How fast the dash is (smaller = faster)
    private float nextBiteTime = 0f;
    private bool isBiting = false;  
    public BiteDamage biteDamage;

    public GameObject attackHitbox ;
    public float attackRange = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ActiveBite(Vector2 lungeDirection) {
        if(Time.time >= nextBiteTime && !isBiting)
        {
            nextBiteTime = Time.time + biteCooldown;
            
            StartCoroutine(LungeSequence(lungeDirection));
        }  
    }

    IEnumerator LungeSequence(Vector2 direction) {
        isBiting = true;
        if (biteDamage != null) biteDamage.SetBiteActive(true);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + (Vector3)direction * biteLungeDistance;
        
        float elapsedTime = 0;

        // Smoothly move from start to target over the duration
        while (elapsedTime < lungeDuration) {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / lungeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        if (biteDamage != null) biteDamage.SetBiteActive(false);

        transform.position = targetPosition; // Ensure we finish at the exact spot
        isBiting = false;
    }
}
