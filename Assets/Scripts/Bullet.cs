using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LogicScript logic;
    public float lifetime = 3f;

    private void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        Destroy(gameObject, lifetime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Kogel raak: " + collision.gameObject.name);
        Destroy(gameObject);
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            logic.AddScore(1);
            Debug.Log("Kogel raak: " + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}

