using UnityEngine;

public class GunSpawnScript : MonoBehaviour

{
    [Header("Spawning")]
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxGuns = 10;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRangeX = 20f;
    [SerializeField] private float spawnRangeZ = 20f;
    [SerializeField] private float raycastHeight = 50f;
    [SerializeField] private LayerMask groundLayer; // stel in op MainTerrain in Inspector

    private float nextSpawnTime = 0f;
    private int currentGuns = 0;

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentGuns < maxGuns)
        {
            SpawnGun();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }



void SpawnGun()
{
    float randomX = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
    float randomZ = transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ);

    Vector3 rayOrigin = new Vector3(randomX, transform.position.y + raycastHeight, randomZ);

    if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2, groundLayer))
    {
        Vector3 spawnPos = hit.point + Vector3.up * 0.5f;
        Instantiate(gunPrefab, spawnPos, Quaternion.identity);
        currentGuns++;
    }
}

    public void OnGunCollected()
    {
        currentGuns--;
    }
}
