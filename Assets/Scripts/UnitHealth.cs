using UnityEngine;

/// <summary>
/// Gère la santé et les dégâts d'une unité
/// </summary>
public class UnitHealth : MonoBehaviour
{
    [Header("Santé")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    
    [Header("Armure")]
    [Tooltip("Réduction de dégâts en % (0-100)")]
    [Range(0f, 100f)]
    public float armor = 0f;

    [Header("Effets visuels")]
    public GameObject destructionEffectPrefab;
    public bool isPlayerUnit = true; // Pour différencier alliés/ennemis

    // Events
    public System.Action<float, float> OnHealthChanged; // (current, max)
    public System.Action OnDeath;

    private bool _isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>Applique des dégâts à l'unité</summary>
    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        // Calcul de réduction d'armure
        float actualDamage = damage * (1f - armor / 100f);
        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(0f, currentHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"[Health] {gameObject.name} prend {actualDamage:F1} dégâts (armure: {armor}%). HP: {currentHealth:F1}/{maxHealth}");

        if (currentHealth <= 0f && !_isDead)
        {
            Die();
        }
    }

    /// <summary>Soigne l'unité</summary>
    public void Heal(float amount)
    {
        if (_isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"[Health] {gameObject.name} soigné de {amount}. HP: {currentHealth:F1}/{maxHealth}");
    }

    void Die()
    {
        _isDead = true;
        Debug.Log($"[Health] {gameObject.name} détruit!");

        OnDeath?.Invoke();

        // Récompense si c'est un ennemi qui meurt
        if (!isPlayerUnit && GameEconomyManager.Instance != null)
        {
            GameEconomyManager.Instance.RewardEnemyKilled();
        }

        // Effet visuel de destruction
        if (destructionEffectPrefab != null)
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    /// <summary>Retourne true si l'unité est encore vivante</summary>
    public bool IsAlive()
    {
        return !_isDead && currentHealth > 0f;
    }

    /// <summary>Retourne le pourcentage de santé restant (0-1)</summary>
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
