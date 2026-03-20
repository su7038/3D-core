using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Kogel raak: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}