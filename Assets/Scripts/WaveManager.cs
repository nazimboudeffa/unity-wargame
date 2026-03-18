using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère le spawn continu d'ennemis
/// </summary>
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Configuration des ennemis")]
    public GameObject[] enemyPrefabs;  // Types d'ennemis disponibles
    public float spawnInterval = 2f;  // Délai entre chaque spawn
    public float enemyHealthMultiplier = 1f; // Multiplie la santé de base
    public int maxActiveEnemies = 10; // Limite d'ennemis actifs simultanément

    [Header("Spawn")]
    public Transform spawnPoint;  // Point de spawn unique pour les ennemis
    public float spawnSpacing = 3f; // Espacement entre les ennemis spawnés

    [Header("État")]
    [SerializeField] private int _enemiesRemaining = 0;
    [SerializeField] private bool _isSpawning = false;

    private List<GameObject> _activeEnemies = new List<GameObject>();
    private float _spawnTimer = 0f;
    private int _enemiesSpawned = 0; // Compteur pour l'espacement

    // Events
    public System.Action<int> OnEnemyCountChanged; // (remaining)

    public int EnemiesRemaining => _enemiesRemaining;
    public bool IsSpawning => _isSpawning;

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
        if (spawnPoint == null)
        {
            Debug.LogError("[WaveManager] Aucun spawn point défini!");
        }

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("[WaveManager] Aucun prefab d'ennemi défini!");
        }
    }

    void Update()
    {
        if (!_isSpawning) return;

        // Spawn automatique si sous la limite
        if (_activeEnemies.Count < maxActiveEnemies)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                SpawnEnemy();
                _spawnTimer = spawnInterval;
            }
        }
    }

    /// <summary>Démarre le spawn continu d'ennemis</summary>
    public void StartSpawning()
    {
        if (_isSpawning)
        {
            Debug.LogWarning("[WaveManager] Le spawn est déjà actif.");
            return;
        }

        _isSpawning = true;
        _spawnTimer = 0f;
        Debug.Log("[WaveManager] === Spawn d'ennemis démarré! ===");
    }

    /// <summary>Arrête le spawn continu d'ennemis</summary>
    public void StopSpawning()
    {
        _isSpawning = false;
        Debug.Log("[WaveManager] === Spawn d'ennemis arrêté! ===");
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("[WaveManager] Aucun prefab d'ennemi!");
            return;
        }

        // Choisir un prefab aléatoire
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Calculer la position avec espacement
        Vector3 spawnPos = spawnPoint.position + Vector3.right * (_enemiesSpawned * spawnSpacing);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, spawnPoint.rotation);
        _enemiesSpawned++;

        Debug.Log($"[WaveManager] Ennemi spawné: {enemy.name} à position {spawnPos}");

        // Appliquer le multiplicateur de santé
        UnitHealth health = enemy.GetComponent<UnitHealth>();
        if (health != null)
        {
            health.maxHealth *= enemyHealthMultiplier;
            health.currentHealth = health.maxHealth;
            health.isPlayerUnit = false; // C'est un ennemi

            // S'abonner à la mort
            health.OnDeath += () => OnEnemyKilled(enemy);
        }

        // Vérifier que l'ennemi a les composants nécessaires au mouvement
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        UnitMovement movement = enemy.GetComponent<UnitMovement>();
        
        if (ai == null)
        {
            Debug.LogWarning($"[WaveManager] {enemy.name} n'a pas de composant EnemyAI, ajout automatique...");
            ai = enemy.AddComponent<EnemyAI>();
        }
        
        if (movement == null)
        {
            Debug.LogWarning($"[WaveManager] {enemy.name} n'a pas de composant UnitMovement, ajout automatique...");
            movement = enemy.AddComponent<UnitMovement>();
        }

        _activeEnemies.Add(enemy);
        _enemiesRemaining = _activeEnemies.Count;
        OnEnemyCountChanged?.Invoke(_enemiesRemaining);
    }

    void OnEnemyKilled(GameObject enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
        }

        _enemiesRemaining = _activeEnemies.Count;
        OnEnemyCountChanged?.Invoke(_enemiesRemaining);

        Debug.Log($"[WaveManager] Ennemi éliminé! Restants: {_enemiesRemaining}");
    }

    /// <summary>Nettoyage manuel de tous les ennemis (pour reset)</summary>
    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in _activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        _activeEnemies.Clear();
        _enemiesRemaining = 0;
        _enemiesSpawned = 0; // Réinitialiser l'espacement
        OnEnemyCountChanged?.Invoke(_enemiesRemaining);
    }

    /// <summary>Reset le manager pour recommencer</summary>
    public void Reset()
    {
        ClearAllEnemies();
        _isSpawning = false;
        _enemiesSpawned = 0; // Réinitialiser l'espacement
    }
}
