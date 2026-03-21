using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public  class Gun : MonoBehaviour

{
    [Header("Shooting")]
    public Transform muzzle;
    public float bulletSpeed = 30f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    [SerializeField] private GameObject bulletPrefab;
    private Camera cam;
    [SerializeField] private ParticleSystem muzzleFlash;

private void Start()
{
    cam = Camera.main;
    Destroy(gameObject, 30f);
}

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null) return;
    
        if (muzzleFlash != null)
        muzzleFlash.Play();

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100f);

        Vector3 direction = (targetPoint - muzzle.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * bulletSpeed;
    }
}