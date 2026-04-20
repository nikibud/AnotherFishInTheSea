using UnityEngine;

public class Bite : MonoBehaviour
{
    public float biteLungeDistance = 2f;
    public float biteCooldown = 0.5f;
    private float nextBiteTime = 0f;

    public GameObject attackHitbox ;
    public float attackRange = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ActiveBite(Vector2 lungeDirection) {
        if(Time.time >= nextBiteTime)
        {
            nextBiteTime = Time.time + biteCooldown;
        
            // Simple Lunge: Move the fish forward instantly in the direction it's facing
            //transform.position += transform.right * biteLungeDistance;
            transform.position += (Vector3)lungeDirection * biteLungeDistance;

            Debug.Log("Chomp!"); 
            // Later, you can trigger an animation here: GetComponent<Animator>().SetTrigger("Bite");
        }  
    }
}
