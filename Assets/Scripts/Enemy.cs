using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;   // De speler
    public float speed = 3f;   // Snelheid van de enemy

    void Start()
    {
        // Zoek automatisch de player als die nog niet is ingesteld
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // Richting naar de speler berekenen
        Vector3 direction = (player.position - transform.position).normalized;

        // Beweeg richting de speler
        transform.position += direction * speed * Time.deltaTime;

        // (optioneel) laat de enemy naar de speler kijken
        transform.LookAt(player);
    }
}