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
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            logic.AddScore(1);
            
            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}

