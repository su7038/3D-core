using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{ 
    [Header("Spawning")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 10;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRangeX = 20f;
    [SerializeField] private float spawnRangeZ = 20f;
    [SerializeField] private float raycastHeight = 50f;
    [SerializeField] private LayerMask groundLayer; // LAYER IS MAINTERRAIN

    private float nextSpawnTime = 0f;
    private int currentEnemies = 0;

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }



void SpawnEnemy()
{
    float randomX = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
    float randomZ = transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ);

    Vector3 rayOrigin = new Vector3(randomX, transform.position.y + raycastHeight, randomZ);

    if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2, groundLayer))
    {
        if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
        {
            Vector3 spawnPos = navHit.position + Vector3.up * 0.5f;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            currentEnemies++;
        }
    }
    else
    {
        Debug.Log("Geen grond gevonden!");
    }
}

    public void OnEnemyDefeated()
    {
        currentEnemies--;
    }
}