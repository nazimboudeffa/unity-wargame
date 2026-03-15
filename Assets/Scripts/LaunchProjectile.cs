using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchProjectile : MonoBehaviour
{
    [Header("Setup")]
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public Transform target;

    [Header("Settings")]
    public float launchVelocity = 60f;
    public float fireCooldown = 0.5f;

    private float _nextFireTime;

    void Update()
    {
        bool firePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        if (firePressed && Time.time >= _nextFireTime)
            FireMissile();
    }

    void FireMissile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("LaunchProjectile: projectilePrefab non assigné dans l'Inspector !");
            return;
        }

        _nextFireTime = Time.time + fireCooldown;

        Vector3 origin    = launchPoint != null ? launchPoint.position : transform.position;
        Vector3 direction = launchPoint != null ? launchPoint.forward  : transform.forward;
        Vector3 spawnPos  = origin + direction * 2f + Vector3.up * 1f;
        Quaternion spawnRot = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 180f);

        GameObject missile = Instantiate(projectilePrefab, spawnPos, spawnRot);

        // Transmet la cible et démarre le missile
        Projectile proj = missile.GetComponent<Projectile>();
        if (proj != null)
        {
            if (target != null) proj.target = target;
            proj.speed = launchVelocity;
            // Passe la vraie direction de tir (pas la rotation visuelle corrigée)
            Vector3 dir = new Vector3(direction.x, 0f, direction.z).normalized;
            if (dir == Vector3.zero) dir = direction.normalized;
            proj.SetLaunched(dir);
        }
        else
        {
            Debug.LogError("LaunchProjectile: le prefab missile n'a pas de composant Projectile !");
        }
    }
}