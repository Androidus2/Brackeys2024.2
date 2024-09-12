using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    enum CloudState
    {
        Roaming,
        Attacking
    }

    CloudState state; 

    [SerializeField]
    private BoxCollider walkArea;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float rotationInterpolation = 0.1f;

    [SerializeField]
    private Transform projectilePrefab;

    [SerializeField]
    private Transform projectileSpawnPosition;

    [SerializeField]
    private float projectileSpeed = 10f;

    [SerializeField]
    private float attackRange = 10f;

    [SerializeField]
    private float attackCooldown = 2f;

    private Animator anim;

    private Vector3 target;
    private Quaternion targetRotation;

    private Transform playerTarget;

    private float timeSinceLastAttack = 2f;

    private void Start()
    {
        anim = GetComponent<Animator>();

        target = GetRandomPosition();

        state = CloudState.Roaming;
    }

    private void Update()
    {
        if(state == CloudState.Roaming)
            DoRoamingState();
        else if(state == CloudState.Attacking)
            DoAttackingState();
    }

    private void DoRoamingState()
    {
        if (Vector3.Distance(transform.position, target) < 1)
            target = GetRandomPosition();

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationInterpolation);
    }

    private void DoAttackingState()
    {
        // Implement attacking state
        // Look at the player, if the player is farther than attackRange, go to the player and attack when off cooldown

        if (playerTarget == null)
        {
            state = CloudState.Roaming;
            target = GetRandomPosition();
            return;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationInterpolation);

        targetRotation = Quaternion.LookRotation(playerTarget.position - transform.position);
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y + 90, 0);

        if (Vector3.Distance(transform.position, playerTarget.position) > attackRange)
        {
            // Move towards the player
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            //Debug.Log("Current position:" + transform.position + " Target position: " + playerTarget.position + " Direction: " + direction + " Speed: " + speed + " DeltaTime: " + Time.deltaTime + " Final position: " + transform.position + direction * speed * Time.deltaTime);
            /*rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);*/
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
        }

        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack >= attackCooldown)
        {
            // Attack the player
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    private void Attack()
    {
        StartCoroutine(SpawnProjectile());

        anim.SetTrigger("Attack");
    }

    IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(0.3f);

        Transform projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
        projectile.LookAt(playerTarget);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        projectileRb.AddForce(projectile.forward * projectileSpeed, ForceMode.Impulse);

        Destroy(projectile.gameObject, 2f);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(walkArea.bounds.min.x, walkArea.bounds.max.x);
        float z = Random.Range(walkArea.bounds.min.z, walkArea.bounds.max.z);

        Vector3 newTarget = new Vector3(x, transform.position.y, z);

        targetRotation = Quaternion.LookRotation(newTarget - transform.position);
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y + 90, 0);

        return newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMain"))
        {
            playerTarget = other.transform;
            state = CloudState.Attacking;

            targetRotation = Quaternion.LookRotation(playerTarget.position - transform.position);
            targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y + 90, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerMain"))
        {
            playerTarget = null;
            state = CloudState.Roaming;

            target = GetRandomPosition();
        }
    }

}
