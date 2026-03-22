using UnityEngine;

public class Coin : MonoBehaviour
{
    public LogicScript logic;
    private bool collected = false;
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();   
    }
   void OnTriggerEnter(Collider other)  // geen 2D
    {
        if (other.CompareTag("Player"))
        {
            collected = true;
            logic.AddScore(5);
            Destroy(gameObject);
        }
    }
}