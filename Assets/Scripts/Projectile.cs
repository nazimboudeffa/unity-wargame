using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public float life = 30f;
    public float speed = 60f;
    public float collisionDelay = 0.15f;

    [Header("Guidage")]
    public Transform target;
    public float turnSpeed = 120f;

    private bool _launched = false;
    private Vector3 _moveDir;

    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Start()
    {
        Destroy(gameObject, life);

        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        Invoke(nameof(EnableCollision), collisionDelay);
    }

    public void SetLaunched(Vector3 direction)
    {
        _moveDir = direction.normalized;
        _launched = true;
    }

    void Update()
    {
        if (!_launched) return;

        if (target != null)
        {
            Vector3 toTarget = target.position - transform.position;
            if (toTarget.sqrMagnitude > 0.01f)
                _moveDir = toTarget.normalized; // pointe directement vers la cible chaque frame
        }

        transform.position += _moveDir * speed * Time.deltaTime;
    }

    void EnableCollision()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}