using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public float life             = 30f;
    public float speed            = 20f;
    public float collisionDelay   = 0.15f;

    [Header("Guidage")]
    public Transform target;

    [Header("Arc Trajectory")]
    [Tooltip("Use a ballistic arc (up then down). Disable for direct homing.")]
    public bool  useArc      = true;
    [Tooltip("Launch angle in degrees (0 = flat, 90 = straight up). 45 is a classic ballistic arc.")]
    [Range(5f, 85f)]
    public float launchAngle = 45f;

    private bool    _launched;
    private Vector3 _moveDir;

    // Arc state
    private Vector3 _startPos;
    private Vector3 _apex;           // fixed at launch — does NOT move
    private Vector3 _targetSnapshot; // updated every frame to track moving targets
    private float   _arcDuration;
    private float   _arcHeight;      // computed from launchAngle at fire time
    private float   _t;

    /// <summary>Aim point set by the launcher (renderer center or root position).</summary>
    [HideInInspector] public Vector3 targetAimPoint;

    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity  = false;
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
        _moveDir  = direction.normalized;
        _launched = true;
        _startPos = transform.position;

        if (useArc && target != null)
        {
            _targetSnapshot = targetAimPoint != Vector3.zero ? targetAimPoint : target.position;
            // Arc height computed once from horizontal distance at launch
            float hDist    = Vector3.Distance(
                new Vector3(_startPos.x, 0f, _startPos.z),
                new Vector3(_targetSnapshot.x, 0f, _targetSnapshot.z));
            _arcHeight   = (hDist * 0.5f) * Mathf.Tan(launchAngle * Mathf.Deg2Rad);
            float dist   = Vector3.Distance(_startPos, _targetSnapshot);
            _arcDuration = dist / Mathf.Max(speed, 0.1f);
            _t           = 0f;

            // Fix the apex position at launch — it will NOT shift as target moves
            _apex = (_startPos + _targetSnapshot) * 0.5f + Vector3.up * _arcHeight;
        }
    }

    void Update()
    {
        if (!_launched) return;

        if (useArc && _arcDuration > 0f)
            UpdateArc();
        else
            UpdateHoming();
    }

    // ── Arc (ballistic) ───────────────────────────────────────────────────────

    void UpdateArc()
    {
        _t += Time.deltaTime / _arcDuration;
        _t  = Mathf.Clamp01(_t);

        // Always track the target so the arc endpoint stays locked onto it
        if (target != null)
            _targetSnapshot = target.position;

        // Apex is fixed — only the endpoint moves to track the target
        Vector3 apex = _apex;

        // Current and next positions on the quadratic Bezier curve
        Vector3 pos     = QuadBezier(_startPos, apex, _targetSnapshot, _t);
        Vector3 nextPos = QuadBezier(_startPos, apex, _targetSnapshot, Mathf.Clamp01(_t + 0.02f));

        transform.position = pos;

        // Orient missile along its travel direction
        Vector3 dir = (nextPos - pos).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(90f, 0f, 0f);

        if (_t >= 1f)
        {
            // Snap exactly onto target before destroying so no visual offset
            transform.position = _targetSnapshot;
            Destroy(gameObject);
        }
    }

    static Vector3 QuadBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        float u = 1f - t;
        return u * u * a + 2f * u * t * b + t * t * c;
    }

    // ── Direct homing ─────────────────────────────────────────────────────────

    void UpdateHoming()
    {
        if (target != null)
        {
            Vector3 toTarget = target.position - transform.position;
            if (toTarget.sqrMagnitude > 0.01f)
                _moveDir = toTarget.normalized;
        }

        transform.position += _moveDir * speed * Time.deltaTime;
    }

    // ── Collision ─────────────────────────────────────────────────────────────

    void EnableCollision()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = true;
    }

    void OnCollisionEnter(Collision collision) => Destroy(gameObject);
    void OnTriggerEnter(Collider other)        => Destroy(gameObject);
}