using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Affiche des informations au-dessus d'une unité
/// Créez un Canvas enfant avec un TextMeshProUGUI et assignez-le dans l'Inspector
/// </summary>
public class UnitInfoDisplay : MonoBehaviour
{
    [Header("Références UI (à assigner manuellement)")]
    [Tooltip("Le texte où afficher les infos (TextMeshProUGUI)")]
    public TextMeshProUGUI infoText;

    [Header("Composants")]
    public UnitHealth unitHealth;
    public AmmunitionInventory ammoInventory;

    [Header("Options d'affichage")]
    public bool showHealth = true;
    public bool showAmmo = true;

    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;

        // Trouver les composants si non assignés
        if (unitHealth == null)
            unitHealth = GetComponent<UnitHealth>();

        if (ammoInventory == null)
            ammoInventory = GetComponent<AmmunitionInventory>();

        // Vérifier que le texte est assigné
        if (infoText == null)
        {
            Debug.LogError($"[UnitInfoDisplay] ⚠️ 'Info Text' n'est pas assigné sur {gameObject.name}! Créez un Canvas enfant avec un TextMeshProUGUI et assignez-le dans l'Inspector.");
            enabled = false;
            return;
        }

        // S'abonner aux changements
        if (unitHealth != null)
            unitHealth.OnHealthChanged += OnStatsChanged;

        if (ammoInventory != null)
            ammoInventory.OnAmmoChanged += OnStatsChanged;

        // Affichage initial
        UpdateDisplay();
        
        Debug.Log($"[UnitInfoDisplay] ✅ Initialisé pour {gameObject.name}");
    }

    void Update()
    {
        // Faire que le Canvas parent face à la caméra
        if (infoText != null && _mainCamera != null)
        {
            Canvas canvas = infoText.GetComponentInParent<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
            {
                canvas.transform.LookAt(_mainCamera.transform);
                canvas.transform.Rotate(0, 180, 0);
            }
        }
    }

    void OnStatsChanged(float current, float max)
    {
        UpdateDisplay();
    }

    void OnStatsChanged(int current, int max)
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (infoText == null) return;

        string info = "";
        Color textColor = Color.white;

        // Afficher la santé
        if (showHealth && unitHealth != null)
        {
            float healthPercent = (unitHealth.currentHealth / unitHealth.maxHealth) * 100f;
            info += $"{Mathf.RoundToInt(unitHealth.currentHealth)}/{unitHealth.maxHealth} HP";
            
            // Couleur selon la santé
            if (healthPercent > 60f)
                textColor = Color.green;
            else if (healthPercent > 30f)
                textColor = Color.yellow;
            else
                textColor = Color.red;
        }

        // Afficher les munitions
        if (showAmmo && ammoInventory != null)
        {
            if (!string.IsNullOrEmpty(info)) info += "\n";
            info += $"{ammoInventory.currentAmmo}/{ammoInventory.maxAmmo} Ammo";
        }

        // Si rien à afficher
        if (string.IsNullOrEmpty(info))
        {
            info = "Ready";
        }

        infoText.text = info;
        infoText.color = textColor;
    }

    void OnDestroy()
    {
        if (unitHealth != null)
            unitHealth.OnHealthChanged -= OnStatsChanged;

        if (ammoInventory != null)
            ammoInventory.OnAmmoChanged -= OnStatsChanged;
    }
}
