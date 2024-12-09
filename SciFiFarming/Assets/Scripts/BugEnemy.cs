using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BugEnemy : MonoBehaviour
{
    public Transform player; // Assign this in the Inspector
    public NavMeshAgent agent;
    public Animator anim;

    public float maxHealth = 100f;
    public float currentHealth;
    public float attackCooldown = 2f;
    public float attackRange = 1.5f;
    public float detectionRadius = 10f; // Added for detection logic
    public float deAggroRadius = 15f; // Adjusted to match detection logic
    public float pauseBeforeAttack = 1f; // Pause duration before attacking

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

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("Player not found in the scene. Assign it in the Inspector.");
                enabled = false;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isAggro && agent.isActiveAndEnabled)
        {
            if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(Attack());
            }
            else if (distanceToPlayer > deAggroRadius && currentHealth == maxHealth)
            {
                isAggro = false; // De-aggro
            }
            else if (distanceToPlayer > 2f)
            {
                agent.SetDestination(player.position);
                if (anim != null) anim.SetBool("IsMoving", true); // Trigger movement animation
            }
            else
            {
                // Stop moving and face the player
                agent.ResetPath();
                RotateTowardsPlayer();
                if (anim != null) anim.SetBool("IsMoving", false); // Stop movement animation
            }
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            isAggro = true; // Aggro when within detection radius
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation
    }

    private IEnumerator Attack()
    {
        if (canAttack && distanceToPlayer <= attackRange)
        {
            canAttack = false;

            // Pause before attacking
            agent.isStopped = true; // Ensure agent is stopped
            RotateTowardsPlayer(); // Ensure facing the player
            if (anim != null) anim.SetTrigger("PrepareAttack"); // Optional preparation animation

            Debug.Log($"{gameObject.name} is preparing to attack...");
            yield return new WaitForSeconds(pauseBeforeAttack);

            // Perform jump attack
            if (anim != null) anim.SetTrigger("Attack"); // Trigger attack animation
            Debug.Log($"{gameObject.name} performs a jump attack!");

            Vector3 jumpDirection = (player.position - transform.position).normalized;
            Vector3 jumpTarget = transform.position + jumpDirection * 3f; // Jump forward 3 units

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

            // Reset to NavMesh after jump
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }

            agent.isStopped = false; // Resume movement after attack
            yield return new WaitForSeconds(attackCooldown); // Cooldown
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
        if (anim != null) anim.SetTrigger("Die");
        agent.enabled = false;
        Destroy(gameObject, 2f); // Delay to allow death animation
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, deAggroRadius);
    }
}
