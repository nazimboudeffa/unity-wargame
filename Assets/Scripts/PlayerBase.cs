using UnityEngine;

/// <summary>
/// Représente la base du joueur à défendre
/// </summary>
public class PlayerBase : MonoBehaviour
{
    [Header("Santé de la base")]
    public float maxHealth = 1000f;
    private float _currentHealth;

    [Header("Effets")]
    public GameObject destructionEffect;

    private UnitHealth _healthComponent;

    void Start()
    {
        // Utiliser le composant UnitHealth s'il existe
        _healthComponent = GetComponent<UnitHealth>();
        
        if (_healthComponent != null)
        {
            _healthComponent.maxHealth = maxHealth;
            _healthComponent.currentHealth = maxHealth;
            _healthComponent.isPlayerUnit = true;
            _healthComponent.OnDeath += OnBaseDestroyed;
        }
        else
        {
            // Santé manuelle si pas de UnitHealth
            _currentHealth = maxHealth;
        }

        Debug.Log($"[PlayerBase] Base initialisée avec {maxHealth} HP");
    }

    /// <summary>La base prend des dégâts</summary>
    public void TakeDamage(float damage)
    {
        if (_healthComponent != null)
        {
            _healthComponent.TakeDamage(damage);
        }
        else
        {
            _currentHealth -= damage;
            Debug.Log($"[PlayerBase] Base endommagée! HP: {_currentHealth}/{maxHealth}");

            if (_currentHealth <= 0f)
            {
                OnBaseDestroyed();
            }
        }
    }

    void OnBaseDestroyed()
    {
        Debug.Log("[PlayerBase] === BASE DÉTRUITE! ===");

        // Notification au GameManager
        if (TacticalDefenseGameManager.Instance != null)
        {
            TacticalDefenseGameManager.Instance.OnPlayerBaseDestroyed();
        }

        // Effet visuel
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        // Ne pas détruire l'objet immédiatement pour permettre l'affichage de l'écran de défaite
        // gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnDeath -= OnBaseDestroyed;
        }
    }
}
