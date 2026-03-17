using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchProjectile : MonoBehaviour
{
    [Header("Setup")]
    public Transform    launchPoint;
    public GameObject   projectilePrefab;
    public Transform    target;

    [Header("Settings")]
    public float launchVelocity = 20f;
    public float fireCooldown   = 1.5f;

    private float _nextFireTime;
    private AmmunitionInventory _ammoInventory;

    void Start()
    {
        _ammoInventory = GetComponent<AmmunitionInventory>();
        if (_ammoInventory == null)
            _ammoInventory = GetComponentInParent<AmmunitionInventory>();
    }

    void Update()
    {
        // Manual fire with Space bar (respects cooldown, requires a target)
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame
            && Time.time >= _nextFireTime)
            FireMissile();
    }

    // World-space aim point (may differ from target.position when targeting a renderer center)
    private Vector3 _targetAimPoint;

    /// <summary>Assigns the attack target and fires one missile immediately.</summary>
    public void SetTarget(Transform t, Vector3 aimPoint)
    {
        target          = t;
        _targetAimPoint = aimPoint;
        _nextFireTime   = 0f;
        Debug.Log($"[LaunchProjectile] SetTarget appelé - Cible: {t.name}, Point visée: {aimPoint}");
        FireMissile();
    }

    // Keep old overload for Space-bar manual fire compatibility
    public void SetTarget(Transform t) => SetTarget(t, t.position);

    /// <summary>Clears the attack target and stops auto-firing.</summary>
    public void ClearTarget()
    {
        target = null;
    }

    void FireMissile()
    {
        if (projectilePrefab == null || target == null) return;

        // Vérifier les munitions
        if (_ammoInventory != null && !_ammoInventory.CanFire())
        {
            Debug.LogWarning($"[LaunchProjectile] {gameObject.name} ne peut pas tirer : plus de munitions!");
            return;
        }

        // Consommer une munition
        if (_ammoInventory != null)
        {
            if (!_ammoInventory.ConsumeAmmo())
                return;
        }

        _nextFireTime = Time.time + fireCooldown;

        // Use aim point for accurate direction; missile will track target transform in flight
        Vector3 origin   = launchPoint != null ? launchPoint.position : transform.position;
        Vector3 toTarget = (_targetAimPoint - origin).normalized;

        Vector3    spawnPos = origin + Vector3.up * 0.5f;
        Quaternion spawnRot = Quaternion.LookRotation(toTarget) * Quaternion.Euler(90f, 0f, 180f);

        Debug.Log($"[LaunchProjectile] Tir missile - Origin: {origin}, TargetAimPoint: {_targetAimPoint}, SpawnPos: {spawnPos}, Direction: {toTarget}");

        GameObject missile = Instantiate(projectilePrefab, spawnPos, spawnRot);

        Projectile proj = missile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.target         = target;
            proj.targetAimPoint = _targetAimPoint;
            proj.speed          = launchVelocity;
            proj.SetLaunched(toTarget);
            Debug.Log($"[LaunchProjectile] Projectile configuré - target: {target.name}, targetAimPoint: {_targetAimPoint}");
        }
        else
        {
            Debug.LogError("LaunchProjectile: missile prefab has no Projectile component!");
        }
    }
}