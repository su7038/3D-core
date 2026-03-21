using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public LogicScript logic;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

void Start()
{
    logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

    if (playerObj == null)
    {
        Debug.LogError("EnemyAI: Geen GameObject gevonden met tag 'Player'!");
        enabled = false;
        return;
    }

    player = playerObj.transform;
    StartCoroutine(WaitForNavMesh());
}

IEnumerator WaitForNavMesh()
{
    while (!agent.isOnNavMesh)
        yield return null;
    
    SetNewWanderDestination();
}
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Game Over");
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    void Update()
    {
{
    Debug.Log($"isOnNavMesh: {agent.isOnNavMesh}, positie: {transform.position}");
    // rest van je code
}

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
        if (!agent.isOnNavMesh) return; // extra check

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