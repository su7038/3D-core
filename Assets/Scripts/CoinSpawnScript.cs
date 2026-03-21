using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxCoins = 10;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRangeX = 20f;
    [SerializeField] private float spawnRangeZ = 20f;
    [SerializeField] private float raycastHeight = 50f;
    [SerializeField] private LayerMask groundLayer; // stel in op MainTerrain in Inspector

    private float nextSpawnTime = 0f;
    private int currentCoins = 0;

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentCoins < maxCoins)
        {
            SpawnCoin();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }



void SpawnCoin()
{
    float randomX = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
    float randomZ = transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ);

    Vector3 rayOrigin = new Vector3(randomX, transform.position.y + raycastHeight, randomZ);

    if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2, groundLayer))
    {
        Vector3 spawnPos = hit.point + Vector3.up * 0.5f;
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        currentCoins++;
    }
}

    public void OnCoinCollected()
    {
        currentCoins--;
    }
}