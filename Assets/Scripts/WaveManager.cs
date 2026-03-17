using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère les vagues d'ennemis : spawn, progression, difficulté
/// </summary>
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [System.Serializable]
    public class Wave
    {
        public string waveName = "Vague 1";
        public int enemyCount = 5;
        public float spawnInterval = 2f;  // Délai entre chaque spawn
        public GameObject[] enemyPrefabs;  // Types d'ennemis pour cette vague
        public float enemyHealthMultiplier = 1f; // Multiplie la santé de base
    }

    [Header("Configuration des vagues")]
    public List<Wave> waves = new List<Wave>();
    public int totalWavesToWin = 10;

    [Header("Spawn")]
    public Transform[] spawnPoints;
    public float delayBetweenWaves = 15f; // Temps pour acheter entre les vagues

    [Header("État")]
    [SerializeField] private int _currentWaveIndex = 0;
    [SerializeField] private int _enemiesRemaining = 0;
    [SerializeField] private bool _waveInProgress = false;
    [SerializeField] private bool _isSpawning = false;

    private List<GameObject> _activeEnemies = new List<GameObject>();

    // Events
    public System.Action<int, int> OnWaveStarted; // (currentWave, totalWaves)
    public System.Action<int> OnWaveCompleted;    // (waveIndex)
    public System.Action OnAllWavesCompleted;
    public System.Action<int> OnEnemyCountChanged; // (remaining)

    public int CurrentWaveIndex => _currentWaveIndex;
    public int EnemiesRemaining => _enemiesRemaining;
    public bool IsWaveInProgress => _waveInProgress;
    public bool CanStartWave => !_waveInProgress && !_isSpawning && _currentWaveIndex < waves.Count;

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
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveManager] Aucun spawn point défini!");
        }

        // Générer des vagues par défaut si la liste est vide
        if (waves.Count == 0)
        {
            GenerateDefaultWaves();
        }
    }

    /// <summary>Démarre la prochaine vague (appelé par le GameManager ou UI)</summary>
    public void StartNextWave()
    {
        if (!CanStartWave)
        {
            Debug.LogWarning("[WaveManager] Impossible de démarrer une nouvelle vague maintenant.");
            return;
        }

        StartCoroutine(SpawnWave(_currentWaveIndex));
    }

    private System.Collections.IEnumerator SpawnWave(int waveIndex)
    {
        if (waveIndex >= waves.Count)
        {
            Debug.LogError($"[WaveManager] Index de vague invalide: {waveIndex}");
            yield break;
        }

        _isSpawning = true;
        _waveInProgress = true;
        
        Wave wave = waves[waveIndex];
        _enemiesRemaining = wave.enemyCount;

        Debug.Log($"[WaveManager] === {wave.waveName} commence! {wave.enemyCount} ennemis ===");
        OnWaveStarted?.Invoke(_currentWaveIndex + 1, totalWavesToWin);
        OnEnemyCountChanged?.Invoke(_enemiesRemaining);

        // Spawn tous les ennemis de la vague
        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(wave);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        _isSpawning = false;
        Debug.Log($"[WaveManager] Tous les ennemis de {wave.waveName} sont spawned. En attente de leur élimination...");
    }

    void SpawnEnemy(Wave wave)
    {
        if (wave.enemyPrefabs == null || wave.enemyPrefabs.Length == 0)
        {
            Debug.LogError("[WaveManager] Aucun prefab d'ennemi dans cette vague!");
            return;
        }

        // Choisir un prefab aléatoire et un spawn point aléatoire
        GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Appliquer le multiplicateur de santé
        UnitHealth health = enemy.GetComponent<UnitHealth>();
        if (health != null)
        {
            health.maxHealth *= wave.enemyHealthMultiplier;
            health.currentHealth = health.maxHealth;
            health.isPlayerUnit = false; // C'est un ennemi

            // S'abonner à la mort
            health.OnDeath += () => OnEnemyKilled(enemy);
        }

        _activeEnemies.Add(enemy);
    }

    void OnEnemyKilled(GameObject enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
        }

        _enemiesRemaining--;
        OnEnemyCountChanged?.Invoke(_enemiesRemaining);

        Debug.Log($"[WaveManager] Ennemi éliminé! Restants: {_enemiesRemaining}");

        // Vérifier si la vague est terminée
        if (_enemiesRemaining <= 0 && _waveInProgress)
        {
            WaveCompleted();
        }
    }

    void WaveCompleted()
    {
        _waveInProgress = false;
        
        Debug.Log($"[WaveManager] === Vague {_currentWaveIndex + 1} terminée! ===");
        
        // Donner la récompense
        if (GameEconomyManager.Instance != null)
        {
            GameEconomyManager.Instance.RewardWaveCompleted(_currentWaveIndex);
        }

        OnWaveCompleted?.Invoke(_currentWaveIndex);

        _currentWaveIndex++;

        // Vérifier la victoire
        if (_currentWaveIndex >= totalWavesToWin)
        {
            Debug.Log("[WaveManager] === TOUTES LES VAGUES TERMINÉES! VICTOIRE! ===");
            OnAllWavesCompleted?.Invoke();
        }
        else
        {
            Debug.Log($"[WaveManager] Prochaine vague dans {delayBetweenWaves}s. Utilisez ce temps pour acheter des unités!");
        }
    }

    /// <summary>Génère des vagues par défaut pour tester</summary>
    void GenerateDefaultWaves()
    {
        Debug.LogWarning("[WaveManager] Génération de vagues par défaut (mode démo).");
        // Les vagues seront configurées dans l'Inspector avec les vrais prefabs
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
    }

    /// <summary>Reset le manager pour recommencer</summary>
    public void ResetWaves()
    {
        ClearAllEnemies();
        _currentWaveIndex = 0;
        _waveInProgress = false;
        _isSpawning = false;
    }
}
