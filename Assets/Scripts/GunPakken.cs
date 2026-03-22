using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [Header("Gun Settings")]
    public Vector3 positionOffset = new Vector3(-0.2f, 0.5f, 0.6f);
    public Vector3 rotationOffset = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            AttachGunToPlayer(other.transform);
            Destroy(gameObject);
        }
    }




[SerializeField] private GameObject gunPrefab;

    private void AttachGunToPlayer(Transform player)
    {
        

        if (gunPrefab == null)
        {
            
            return;
        }

        
        Camera cam = player.GetComponentInChildren<Camera>();
        Transform attachPoint = cam != null ? cam.transform : player;

        GameObject gun = Instantiate(gunPrefab);
        gun.transform.SetParent(attachPoint);
        gun.transform.localPosition = positionOffset;
        gun.transform.localRotation = Quaternion.Euler(rotationOffset);
    }
}
