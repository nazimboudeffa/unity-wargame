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
        BuyPhase,      // Phase d'achat initiale
        Playing,       // Jeu en cours (continu)
        Victory,
        Defeat
    }

    [Header("État du jeu")]
    [SerializeField] private GameState _currentState = GameState.MainMenu;

    [Header("Paramètres")]
    public float buyPhaseDuration = 30f;  // Temps pour acheter avant le début
    public bool autoStartGame = false;    // Démarrage automatique après le temps d'achat

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
        // Commencer en phase d'achat
        SetGameState(GameState.BuyPhase);
    }

    void Update()
    {
        if (_currentState == GameState.BuyPhase)
        {
            UpdateBuyPhase();
        }
    }

    void UpdateBuyPhase()
    {
        if (!autoStartGame) return;

        _buyPhaseTimer -= Time.deltaTime;
        if (_buyPhaseTimer <= 0f)
        {
            StartGame();
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
                Debug.Log($"=== PHASE D'ACHAT - {buyPhaseDuration}s pour préparer vos défenses ===");
                break;

            case GameState.Playing:
                Debug.Log("=== LE JEU COMMENCE! ===");
                break;

            case GameState.Victory:
                OnVictory?.Invoke();
                Debug.Log("=== VICTOIRE! ===");
                break;

            case GameState.Defeat:
                OnDefeat?.Invoke();
                Debug.Log("=== DÉFAITE! VOTRE BASE A ÉTÉ DÉTRUITE! ===");
                break;
        }
    }

    /// <summary>Démarre le jeu après la phase d'achat</summary>
    public void StartGame()
    {
        if (_currentState != GameState.BuyPhase)
        {
            Debug.LogWarning("[GameManager] Le jeu ne peut être démarré qu'en phase d'achat.");
            return;
        }

        SetGameState(GameState.Playing);
        
        // Démarrer le spawn continu d'ennemis
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.StartSpawning();
        }
    }

    /// <summary>Retourne le temps restant en phase d'achat</summary>
    public float GetBuyPhaseTimeRemaining()
    {
        return _buyPhaseTimer;
    }

    /// <summary>Appelé quand la base du joueur est détruite</summary>
    public void OnPlayerBaseDestroyed()
    {
        SetGameState(GameState.Defeat);
    }

    /// <summary>Appelé pour déclencher la victoire manuellement</summary>
    public void TriggerVictory()
    {
        SetGameState(GameState.Victory);
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
}
