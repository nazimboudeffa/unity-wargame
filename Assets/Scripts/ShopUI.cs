using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Interface UI pour le shop - Achat d'unités et de munitions
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI budgetText;
    public TextMeshProUGUI waveInfoText;
    public Button startWaveButton;
    public GameObject shopPanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Boutons d'achat d'unités")]
    public Button buyTankButton;
    public Button buyMissileLauncherButton;

    [Header("Textes des prix")]
    public TextMeshProUGUI tankPriceText;
    public TextMeshProUGUI missileLauncherPriceText;

    [Header("Placement des unités")]
    public Transform unitSpawnPoint;
    public float spawnSpacing = 3f;
    private int _unitsSpawned = 0;

    [Header("Sélection d'unité pour munitions")]
    public GameObject ammoShopPanel;
    public TextMeshProUGUI selectedUnitText;
    public TextMeshProUGUI ammoCountText;
    public Button buyAmmoButton;
    public TextMeshProUGUI ammoPriceText;
    private AmmunitionInventory _selectedUnitInventory;

    void Start()
    {
        // S'abonner aux événements
        if (GameEconomyManager.Instance != null)
        {
            GameEconomyManager.Instance.OnBudgetChanged += UpdateBudgetDisplay;
            UpdateBudgetDisplay(GameEconomyManager.Instance.CurrentBudget);
        }

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnEnemyCountChanged += UpdateEnemyCount;
        }

        if (TacticalDefenseGameManager.Instance != null)
        {
            TacticalDefenseGameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            TacticalDefenseGameManager.Instance.OnVictory += ShowVictoryScreen;
            TacticalDefenseGameManager.Instance.OnDefeat += ShowDefeatScreen;
        }

        // Configurer les boutons
        SetupUnitButtons();
        SetupWaveButton();
        SetupAmmoButton();

        // Afficher les prix
        UpdatePriceDisplays();

        // Cacher les panneaux de fin au départ
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (ammoShopPanel != null) ammoShopPanel.SetActive(false);
    }

    void SetupUnitButtons()
    {
        if (GameEconomyManager.Instance == null) return;

        if (buyTankButton != null)
        {
            buyTankButton.onClick.AddListener(() => BuyUnit(UnitData.UnitType.Tank));
            // Désactiver si pas de données configurées
            if (GameEconomyManager.Instance.GetUnitData(UnitData.UnitType.Tank) == null)
            {
                buyTankButton.interactable = false;
                Debug.LogWarning("[ShopUI] Tank n'est pas configuré dans GameEconomyManager - bouton désactivé");
            }
        }

        if (buyMissileLauncherButton != null)
        {
            buyMissileLauncherButton.onClick.AddListener(() => BuyUnit(UnitData.UnitType.MissileLauncher));
            // Désactiver si pas de données configurées
            if (GameEconomyManager.Instance.GetUnitData(UnitData.UnitType.MissileLauncher) == null)
            {
                buyMissileLauncherButton.interactable = false;
                Debug.LogWarning("[ShopUI] MissileLauncher n'est pas configuré dans GameEconomyManager - bouton désactivé");
            }
        }
    }

    void SetupWaveButton()
    {
        if (startWaveButton != null)
        {
            startWaveButton.onClick.AddListener(StartWave);
        }
    }

    void SetupAmmoButton()
    {
        if (buyAmmoButton != null)
        {
            buyAmmoButton.onClick.AddListener(() => BuyAmmo(10)); // Acheter 10 munitions à la fois
        }
    }

    void BuyUnit(UnitData.UnitType type)
    {
        Debug.Log($"[ShopUI DEBUG] === DÉBUT ACHAT {type} ===");
        
        if (GameEconomyManager.Instance == null)
        {
            Debug.LogError("[ShopUI DEBUG] GameEconomyManager.Instance est NULL!");
            return;
        }
        Debug.Log("[ShopUI DEBUG] GameEconomyManager trouvé");

        UnitData unitData = GameEconomyManager.Instance.GetUnitData(type);
        if (unitData == null)
        {
            Debug.LogError($"[ShopUI DEBUG] Impossible de trouver les données pour {type}");
            return;
        }
        Debug.Log($"[ShopUI DEBUG] UnitData trouvé: {unitData.displayName}, Prix: {unitData.purchaseCost}");

        if (unitSpawnPoint == null)
        {
            Debug.LogError("[ShopUI DEBUG] ⚠️ UNITSPAWNPOINT EST NULL! Assignez-le dans l'Inspector!");
            return;
        }
        Debug.Log($"[ShopUI DEBUG] UnitSpawnPoint OK à position: {unitSpawnPoint.position}");

        if (GameEconomyManager.Instance.TryPurchaseUnit(unitData, out GameObject spawnedUnit))
        {
            Debug.Log($"[ShopUI DEBUG] TryPurchaseUnit a retourné TRUE");
            
            // Placer l'unité
            if (spawnedUnit == null)
            {
                Debug.LogError("[ShopUI DEBUG] ⚠️ spawnedUnit est NULL après l'achat!");
                return;
            }
            
            Debug.Log($"[ShopUI DEBUG] spawnedUnit créé: {spawnedUnit.name}");
            Debug.Log($"[ShopUI DEBUG] Position initiale: {spawnedUnit.transform.position}");
            Debug.Log($"[ShopUI DEBUG] Scale: {spawnedUnit.transform.localScale}");
            Debug.Log($"[ShopUI DEBUG] Active: {spawnedUnit.activeSelf}");
            
            Vector3 spawnPos = unitSpawnPoint.position + Vector3.right * (_unitsSpawned * spawnSpacing);
            spawnedUnit.transform.position = spawnPos;
            Debug.Log($"[ShopUI DEBUG] Nouvelle position: {spawnPos}");
            
            // S'assurer que l'unité est active et visible
            spawnedUnit.SetActive(true);
            
            // Vérifier les composants
            var renderer = spawnedUnit.GetComponentInChildren<Renderer>();
            Debug.Log($"[ShopUI DEBUG] Renderer trouvé: {renderer != null}");
            if (renderer != null)
            {
                Debug.Log($"[ShopUI DEBUG] Renderer enabled: {renderer.enabled}");
            }
            
            _unitsSpawned++;

            Debug.Log($"[ShopUI DEBUG] ✅ {unitData.displayName} SPAWNÉ à {spawnPos}!");
        }
        else
        {
            Debug.LogWarning($"[ShopUI DEBUG] TryPurchaseUnit a retourné FALSE (budget insuffisant ou prefab manquant)");
        }
    }

    void StartWave()
    {
        if (TacticalDefenseGameManager.Instance != null)
        {
            TacticalDefenseGameManager.Instance.StartGame();
        }
    }

    public void SelectUnitForAmmo(AmmunitionInventory inventory)
    {
        _selectedUnitInventory = inventory;
        
        if (ammoShopPanel != null)
            ammoShopPanel.SetActive(true);

        if (selectedUnitText != null && inventory != null)
            selectedUnitText.text = $"Unité: {inventory.gameObject.name}";

        UpdateAmmoDisplay();
    }

    void BuyAmmo(int quantity)
    {
        if (_selectedUnitInventory == null)
        {
            Debug.LogWarning("[ShopUI] Aucune unité sélectionnée pour acheter des munitions!");
            return;
        }

        if (GameEconomyManager.Instance == null) return;

        // Utiliser les munitions standard par défaut
        AmmoData ammoData = GameEconomyManager.Instance.GetAmmoData(AmmoData.AmmoType.StandardShell);
        
        if (ammoData != null)
        {
            GameEconomyManager.Instance.TryPurchaseAmmo(ammoData, quantity, _selectedUnitInventory);
            UpdateAmmoDisplay();
        }
    }

    void UpdateAmmoDisplay()
    {
        if (_selectedUnitInventory != null && ammoCountText != null)
        {
            ammoCountText.text = $"Munitions: {_selectedUnitInventory.currentAmmo}/{_selectedUnitInventory.maxAmmo}";
        }
    }

    void UpdateBudgetDisplay(int budget)
    {
        if (budgetText != null)
        {
            budgetText.text = $"Budget: ${budget:N0}";
        }
    }

    void UpdateEnemyCount(int remaining)
    {
        if (waveInfoText != null)
        {
            waveInfoText.text = $"Ennemis actifs: {remaining}";
        }
    }

    void UpdatePriceDisplays()
    {
        if (GameEconomyManager.Instance == null) return;

        UpdateUnitPriceText(tankPriceText, UnitData.UnitType.Tank);
        UpdateUnitPriceText(missileLauncherPriceText, UnitData.UnitType.MissileLauncher);

        // Prix des munitions
        if (ammoPriceText != null)
        {
            AmmoData ammo = GameEconomyManager.Instance.GetAmmoData(AmmoData.AmmoType.StandardShell);
            if (ammo != null)
                ammoPriceText.text = $"10x: ${ammo.cost * 10}";
        }
    }

    void UpdateUnitPriceText(TextMeshProUGUI textField, UnitData.UnitType type)
    {
        if (textField == null) return;

        UnitData data = GameEconomyManager.Instance.GetUnitData(type);
        if (data != null)
        {
            textField.text = $"${data.purchaseCost:N0}";
        }
        else
        {
            textField.text = "N/A";
        }
    }

    void OnGameStateChanged(TacticalDefenseGameManager.GameState state)
    {
        // Activer/désactiver le shop selon l'état
        if (shopPanel != null)
        {
            shopPanel.SetActive(state == TacticalDefenseGameManager.GameState.BuyPhase);
        }

        // Activer/désactiver le bouton de démarrage du jeu
        if (startWaveButton != null)
        {
            startWaveButton.interactable = state == TacticalDefenseGameManager.GameState.BuyPhase;
        }
    }

    void ShowVictoryScreen()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    void ShowDefeatScreen()
    {
        if (defeatPanel != null)
            defeatPanel.SetActive(true);

        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    // Boutons des écrans de fin
    public void OnRestartButtonClicked()
    {
        if (TacticalDefenseGameManager.Instance != null)
            TacticalDefenseGameManager.Instance.RestartGame();
    }

    public void OnQuitButtonClicked()
    {
        if (TacticalDefenseGameManager.Instance != null)
            TacticalDefenseGameManager.Instance.QuitGame();
    }
}
