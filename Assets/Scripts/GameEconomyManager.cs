using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère l'économie du jeu : budget, prix des unités et munitions, achats
/// </summary>
public class GameEconomyManager : MonoBehaviour
{
    public static GameEconomyManager Instance { get; private set; }

    [Header("Budget")]
    [SerializeField] private int startingBudget = 1000000;
    private int _currentBudget;

    [Header("Unit Catalog")]
    public List<UnitData> availableUnits = new List<UnitData>();

    [Header("Ammo Catalog")]
    public List<AmmoData> availableAmmo = new List<AmmoData>();

    [Header("Rewards")]
    public int rewardPerEnemyKilled = 10000;
    public int[] waveCompletionBonus = { 25000, 30000, 40000, 50000, 60000, 75000, 85000, 95000, 100000, 150000 };

    public int CurrentBudget => _currentBudget;

    // Events pour notifier l'UI
    public System.Action<int> OnBudgetChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _currentBudget = startingBudget;
        OnBudgetChanged?.Invoke(_currentBudget);
    }

    /// <summary>Tente d'acheter une unité</summary>
    public bool TryPurchaseUnit(UnitData unitData, out GameObject spawnedUnit)
    {
        Debug.Log($"[Economy DEBUG] === DÉBUT TryPurchaseUnit ===");
        spawnedUnit = null;

        if (unitData == null)
        {
            Debug.LogError("[Economy DEBUG] UnitData est null!");
            return false;
        }
        Debug.Log($"[Economy DEBUG] UnitData OK: {unitData.displayName}");

        Debug.Log($"[Economy DEBUG] Budget actuel: ${_currentBudget}, Coût: ${unitData.purchaseCost}");
        if (_currentBudget < unitData.purchaseCost)
        {
            Debug.LogWarning($"[Economy DEBUG] ⚠️ Fonds insuffisants pour {unitData.displayName}. Besoin: ${unitData.purchaseCost}, Disponible: ${_currentBudget}");
            return false;
        }

        if (unitData.prefab == null)
        {
            Debug.LogError($"[Economy DEBUG] ⚠️ PREFAB MANQUANT pour {unitData.displayName}! Assignez-le dans GameEconomyManager → Available Units!");
            return false;
        }
        Debug.Log($"[Economy DEBUG] Prefab OK: {unitData.prefab.name}");

        // Déduire le coût
        _currentBudget -= unitData.purchaseCost;
        OnBudgetChanged?.Invoke(_currentBudget);
        Debug.Log($"[Economy DEBUG] Budget déduit. Nouveau budget: ${_currentBudget}");

        // Spawn l'unité (la position sera gérée par le shop/placement system)
        Debug.Log($"[Economy DEBUG] Instantiation du prefab...");
        spawnedUnit = Instantiate(unitData.prefab);
        Debug.Log($"[Economy DEBUG] Prefab instantié: {spawnedUnit.name}");
        
        // Initialiser les munitions de départ
        AmmunitionInventory ammoInv = spawnedUnit.GetComponent<AmmunitionInventory>();
        if (ammoInv != null)
        {
            ammoInv.currentAmmo = unitData.startingAmmo;
            ammoInv.maxAmmo = unitData.maxAmmoCapacity;
            Debug.Log($"[Economy DEBUG] Munitions initialisées: {unitData.startingAmmo}/{unitData.maxAmmoCapacity}");
        }
        else
        {
            Debug.LogWarning($"[Economy DEBUG] Pas de AmmunitionInventory sur {spawnedUnit.name}");
        }

        // Initialiser la santé
        UnitHealth health = spawnedUnit.GetComponent<UnitHealth>();
        if (health != null)
        {
            health.maxHealth = unitData.maxHealth;
            health.currentHealth = unitData.maxHealth;
            health.armor = unitData.armor;
            Debug.Log($"[Economy DEBUG] Santé initialisée: {unitData.maxHealth} HP, {unitData.armor} armor");
        }
        else
        {
            Debug.LogWarning($"[Economy DEBUG] Pas de UnitHealth sur {spawnedUnit.name}");
        }

        Debug.Log($"[Economy DEBUG] ✅ {unitData.displayName} acheté pour ${unitData.purchaseCost}. Budget restant: ${_currentBudget}");
        return true;
    }

    /// <summary>Tente d'acheter des munitions pour une unité</summary>
    public bool TryPurchaseAmmo(AmmoData ammoData, int quantity, AmmunitionInventory targetInventory)
    {
        if (ammoData == null || targetInventory == null)
        {
            Debug.LogError("[Economy] AmmoData ou inventaire null!");
            return false;
        }

        int totalCost = ammoData.cost * quantity;

        if (_currentBudget < totalCost)
        {
            Debug.Log($"[Economy] Fonds insuffisants pour {quantity}x {ammoData.displayName}. Besoin: ${totalCost}, Disponible: ${_currentBudget}");
            return false;
        }

        if (!targetInventory.CanAddAmmo(quantity))
        {
            Debug.Log($"[Economy] Capacité de munitions insuffisante!");
            return false;
        }

        // Déduire le coût et ajouter les munitions
        _currentBudget -= totalCost;
        OnBudgetChanged?.Invoke(_currentBudget);
        targetInventory.AddAmmo(quantity);

        Debug.Log($"[Economy] {quantity}x {ammoData.displayName} acheté pour ${totalCost}. Budget restant: ${_currentBudget}");
        return true;
    }

    /// <summary>Ajoute de l'argent au budget (récompenses)</summary>
    public void AddFunds(int amount)
    {
        _currentBudget += amount;
        OnBudgetChanged?.Invoke(_currentBudget);
        Debug.Log($"[Economy] +${amount} ajouté. Budget: ${_currentBudget}");
    }

    /// <summary>Récompense pour avoir tué un ennemi</summary>
    public void RewardEnemyKilled()
    {
        AddFunds(rewardPerEnemyKilled);
    }

    /// <summary>Récompense de fin de vague</summary>
    public void RewardWaveCompleted(int waveIndex)
    {
        if (waveIndex >= 0 && waveIndex < waveCompletionBonus.Length)
        {
            int bonus = waveCompletionBonus[waveIndex];
            AddFunds(bonus);
            Debug.Log($"[Economy] Bonus de vague {waveIndex + 1}: ${bonus}");
        }
    }

    /// <summary>Trouve une UnitData par type</summary>
    public UnitData GetUnitData(UnitData.UnitType type)
    {
        return availableUnits.Find(u => u.type == type);
    }

    /// <summary>Trouve une AmmoData par type</summary>
    public AmmoData GetAmmoData(AmmoData.AmmoType type)
    {
        return availableAmmo.Find(a => a.type == type);
    }
}
