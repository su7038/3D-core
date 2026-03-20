using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    

    [Header("Chase Settings")]
    public float moveSpeed = 4f;
    public float detectionRange = 12f;
    public float updateRate = 0.1f;

    [Header("Wander Settings")]
    public float wanderRadius = 8f;
    public float wanderSpeed = 2f;

    private NavMeshAgent agent;
    private Transform player;
    private float updateTimer;
    private bool isChasing = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogError("EnemyAI: Geen GameObject gevonden met tag 'Player'!");
            enabled = false;
            return;
        }

        player = playerObj.transform;
        SetNewWanderDestination();
    }
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
        }

        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer < updateRate) return;
        updateTimer = 0f;

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("EnemyAI: Agent staat niet op een NavMesh!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Chase
            isChasing = true;
            agent.speed = moveSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            // Wander
            isChasing = false;
            agent.speed = wanderSpeed;

            // Nieuw doel als het huidige bereikt is
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                SetNewWanderDestination();
        }
    }

    void SetNewWanderDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        randomPoint.y = transform.position.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}