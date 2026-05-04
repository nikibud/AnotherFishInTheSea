using UnityEngine;

public class SprayInk : MonoBehaviour
{
    public ParticleSystem inkParticles; // The ink effect

    private bool hasSprayed = false;
    private float sprayCooldown = 5f;
    private float timer;

    void Update()
    {
        if (hasSprayed)
        {
            timer += Time.deltaTime;
            if (timer >= sprayCooldown)
            {
                hasSprayed = false;
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing that entered the circle is the Player
        if (other.CompareTag("Player") && !hasSprayed)
        {
            SprayAttack();
        }
    }

    void SprayAttack()
    {
        hasSprayed = true;
        Debug.Log("SQUID INK ATTACK!");

        if (inkParticles != null)
        {
            inkParticles.Play();
        }

    }
}