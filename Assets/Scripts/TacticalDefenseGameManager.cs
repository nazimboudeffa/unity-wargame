using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestionnaire principal du jeu - Gère les états, la victoire/défaite, les phases
/// </summary>
public class TacticalDefenseGameManager : MonoBehaviour
{
    public static TacticalDefenseGameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        BuyPhase,      // Phase d'achat entre les vagues
        CombatPhase,   // Vague en cours
        Victory,
        Defeat
    }

    [Header("État du jeu")]
    [SerializeField] private GameState _currentState = GameState.MainMenu;

    [Header("Paramètres")]
    public float buyPhaseDuration = 30f;  // Temps pour acheter (désactivé par défaut, contrôle manuel)
    public bool autoStartWaves = false;   // Démarrage automatique des vagues

    private float _buyPhaseTimer;

    // Events
    public System.Action<GameState> OnGameStateChanged;
    public System.Action OnVictory;
    public System.Action OnDefeat;

    public GameState CurrentState => _currentState;

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
        // S'abonner aux events des autres managers
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted += OnWaveStarted;
            WaveManager.Instance.OnWaveCompleted += OnWaveCompleted;
            WaveManager.Instance.OnAllWavesCompleted += OnAllWavesCompleted;
        }

        // Commencer en phase d'achat
        SetGameState(GameState.BuyPhase);
    }

    void Update()
    {
        switch (_currentState)
        {
            case GameState.BuyPhase:
                UpdateBuyPhase();
                break;
        }
    }

    void UpdateBuyPhase()
    {
        if (!autoStartWaves) return;

        _buyPhaseTimer -= Time.deltaTime;
        if (_buyPhaseTimer <= 0f && WaveManager.Instance != null)
        {
            StartCombatPhase();
        }
    }

    /// <summary>Change l'état du jeu</summary>
    void SetGameState(GameState newState)
    {
        if (_currentState == newState) return;

        Debug.Log($"[GameManager] État: {_currentState} -> {newState}");
        _currentState = newState;
        OnGameStateChanged?.Invoke(_currentState);

        // Actions spécifiques selon l'état
        switch (_currentState)
        {
            case GameState.BuyPhase:
                _buyPhaseTimer = buyPhaseDuration;
                break;

            case GameState.Victory:
                OnVictory?.Invoke();
                Debug.Log("=== VICTOIRE! TOUTES LES VAGUES SONT TERMINÉES! ===");
                break;

            case GameState.Defeat:
                OnDefeat?.Invoke();
                Debug.Log("=== DÉFAITE! VOTRE BASE A ÉTÉ DÉTRUITE! ===");
                break;
        }
    }

    /// <summary>Démarre la phase de combat (lance la vague)</summary>
    public void StartCombatPhase()
    {
        if (WaveManager.Instance == null || !WaveManager.Instance.CanStartWave)
        {
            Debug.LogWarning("[GameManager] Impossible de démarrer une vague maintenant.");
            return;
        }

        SetGameState(GameState.CombatPhase);
        WaveManager.Instance.StartNextWave();
    }

    void OnWaveStarted(int currentWave, int totalWaves)
    {
        Debug.Log($"[GameManager] Vague {currentWave}/{totalWaves} démarrée!");
    }

    void OnWaveCompleted(int waveIndex)
    {
        Debug.Log($"[GameManager] Vague {waveIndex + 1} terminée! Retour en phase d'achat.");
        SetGameState(GameState.BuyPhase);
    }

    void OnAllWavesCompleted()
    {
        SetGameState(GameState.Victory);
    }

    /// <summary>Appelé quand la base du joueur est détruite</summary>
    public void OnPlayerBaseDestroyed()
    {
        SetGameState(GameState.Defeat);
    }

    /// <summary>Redémarre le jeu</summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Retour au menu principal</summary>
    public void ReturnToMainMenu()
    {
        // À implémenter selon votre structure de scènes
        Debug.Log("[GameManager] Retour au menu principal");
    }

    /// <summary>Quitter le jeu</summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnWaveStarted -= OnWaveStarted;
            WaveManager.Instance.OnWaveCompleted -= OnWaveCompleted;
            WaveManager.Instance.OnAllWavesCompleted -= OnAllWavesCompleted;
        }
    }
}
