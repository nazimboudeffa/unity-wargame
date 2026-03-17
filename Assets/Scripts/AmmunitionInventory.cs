using UnityEngine;

/// <summary>
/// Gère l'inventaire de munitions d'une unité
/// </summary>
public class AmmunitionInventory : MonoBehaviour
{
    [Header("Munitions")]
    public int currentAmmo = 10;
    public int maxAmmo = 50;

    [Header("Debug")]
    public bool showDebugInfo = true;

    // Event pour notifier l'UI
    public System.Action<int, int> OnAmmoChanged; // (current, max)

    void Start()
    {
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    /// <summary>Vérifie si on peut tirer (a des munitions)</summary>
    public bool CanFire()
    {
        return currentAmmo > 0;
    }

    /// <summary>Consomme une munition lors d'un tir</summary>
    public bool ConsumeAmmo()
    {
        if (currentAmmo <= 0)
        {
            if (showDebugInfo)
                Debug.LogWarning($"[Ammo] {gameObject.name} n'a plus de munitions!");
            return false;
        }

        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        
        if (showDebugInfo)
            Debug.Log($"[Ammo] {gameObject.name} tire. Munitions restantes: {currentAmmo}/{maxAmmo}");
        
        return true;
    }

    /// <summary>Ajoute des munitions</summary>
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        
        if (showDebugInfo)
            Debug.Log($"[Ammo] +{amount} munitions. Total: {currentAmmo}/{maxAmmo}");
    }

    /// <summary>Vérifie si on peut ajouter des munitions</summary>
    public bool CanAddAmmo(int amount)
    {
        return (currentAmmo + amount) <= maxAmmo;
    }

    /// <summary>Retourne le nombre de munitions qui peuvent encore être ajoutées</summary>
    public int GetRemainingCapacity()
    {
        return maxAmmo - currentAmmo;
    }

    /// <summary>Rechargement complet</summary>
    public void Refill()
    {
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }
}
