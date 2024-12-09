using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BugEnemy : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator anim;

    public float maxHealth = 100f;
    public float currentHealth;
    public float attackCooldown = 2f;
    public float attackRange = 1.5f;
    public float deAggroRadius = 10f;

    private float distanceToPlayer;
    private bool canAttack = true;
    private bool isAggro = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"{gameObject.name} is not on a NavMesh. Check placement or bake settings.");
            enabled = false;
        }
    }

    void Update()
    {
        if (player == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the spider is aggroed
        if (isAggro && agent != null && agent.isActiveAndEnabled)
        {
            if (distanceToPlayer <= attackRange) // Attack if within range
            {
                StartCoroutine(Attack());
            }
            else if (distanceToPlayer > deAggroRadius && currentHealth == maxHealth)
            {
                isAggro = false; // De-aggro if far enough and full health
            }
            else if (distanceToPlayer > 3f) // Chase player until within 3 units
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                // Stop the agent when within 3 units to prepare for attack
                agent.ResetPath();
            }
        }
    }

    private IEnumerator Attack()
    {
        if (canAttack && distanceToPlayer <= attackRange) // Ensure enemy is close enough to attack
        {
            canAttack = false;

            // Perform the jump attack logic
            Debug.Log("SpiderEnemy performs a jump attack!");
            Vector3 jumpDirection = (player.transform.position - transform.position).normalized;
            Vector3 jumpTarget = transform.position + jumpDirection * 3f; // Jump forward 3 units

            // Simulate a jump using a coroutine (this could be replaced with animation logic)
            // float jumpSpeed = 5f;
            float elapsedTime = 0f;
            float jumpDuration = 0.5f;

            Vector3 initialPosition = transform.position;

            while (elapsedTime < jumpDuration)
            {
                elapsedTime += Time.deltaTime;
                float lerpFactor = elapsedTime / jumpDuration;

                // Apply parabolic arc for the jump
                float height = Mathf.Sin(Mathf.PI * lerpFactor) * 2f; // Jump height
                transform.position = Vector3.Lerp(initialPosition, jumpTarget, lerpFactor) + new Vector3(0, height, 0);

                yield return null;
            }

            // Reset to grounded position after the jump
            transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);

            yield return new WaitForSeconds(attackCooldown); // Wait for cooldown before next attack
            canAttack = true;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            isAggro = true;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        anim.SetTrigger("Die");
        agent.enabled = false;
        Destroy(gameObject, 2f); // Delay to allow death animation
    }
}
