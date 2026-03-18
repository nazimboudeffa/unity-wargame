using UnityEngine;

/// <summary>
/// Définit les caractéristiques et le coût d'un type d'unité
/// </summary>
[System.Serializable]
public class UnitData
{
    public enum UnitType
    {
        Tank,
        MissileLauncher
    }

    public UnitType type;
    public string displayName;
    public int purchaseCost;
    public GameObject prefab;
    
    [Header("Stats")]
    public float maxHealth = 100f;
    public float armor = 0f;              // Réduction de dégâts en %
    public int maxAmmoCapacity = 50;      // Capacité maximale de munitions
    public int startingAmmo = 10;         // Munitions de départ
}

/// <summary>
/// Définit les types de munitions et leurs coûts
/// </summary>
[System.Serializable]
public class AmmoData
{
    public enum AmmoType
    {
        StandardShell,
        GuidedMissile,
        AntiTankMissile
    }

    public AmmoType type;
    public string displayName;
    public int cost;
    public float damage;
    public GameObject projectilePrefab;
}
