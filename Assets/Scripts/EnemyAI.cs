using UnityEngine;

/// <summary>
/// IA simple pour les ennemis : se déplacer vers la base du joueur et l'attaquer
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Cible")]
    public Transform playerBase;  // Base à attaquer
    public float detectionRange = 50f;

    [Header("Comportement")]
    public bool moveToBase = true;
    public bool canAttack = true;
    public float attackRange = 10f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;

    private UnitMovement _movement;
    private LaunchProjectile _launcher;
    private float _nextAttackTime;
    private bool _isAttacking = false;

    void Start()
    {
        _movement = GetComponent<UnitMovement>();
        if (_movement == null)
            _movement = GetComponentInChildren<UnitMovement>();

        _launcher = GetComponent<LaunchProjectile>();
        if (_launcher == null)
            _launcher = GetComponentInChildren<LaunchProjectile>();

        // Trouver la base du joueur si non assignée
        if (playerBase == null)
        {
            GameObject baseObj = GameObject.FindGameObjectWithTag("PlayerBase");
            if (baseObj != null)
                playerBase = baseObj.transform;
            else
                Debug.LogWarning($"[EnemyAI] {gameObject.name} ne trouve pas la base du joueur! Tag 'PlayerBase' manquant?");
        }

        // Commencer le mouvement vers la base
        if (moveToBase && playerBase != null && _movement != null)
        {
            _movement.MoveTo(playerBase.position);
        }
    }

    void Update()
    {
        if (playerBase == null) return;

        float distanceToBase = Vector3.Distance(transform.position, playerBase.position);

        // Attaquer si à portée
        if (canAttack && distanceToBase <= attackRange)
        {
            if (!_isAttacking)
            {
                _isAttacking = true;
                // Arrêter le mouvement
                if (_movement != null)
                    _movement.MoveTo(transform.position);
            }

            TryAttackBase();
        }
        else if (moveToBase && _movement != null)
        {
            // Continuer à se déplacer vers la base
            if (!_isAttacking)
            {
                _movement.MoveTo(playerBase.position);
            }
        }
    }

    void TryAttackBase()
    {
        if (Time.time < _nextAttackTime) return;

        _nextAttackTime = Time.time + attackCooldown;

        // Si l'ennemi a un lanceur de projectiles
        if (_launcher != null && playerBase != null)
        {
            _launcher.SetTarget(playerBase);
            Debug.Log($"[EnemyAI] {gameObject.name} tire sur la base!");
        }
        else
        {
            // Attaque directe (dégâts instantanés)
            AttackBaseDirect();
        }
    }

    void AttackBaseDirect()
    {
        if (playerBase == null) return;

        UnitHealth baseHealth = playerBase.GetComponent<UnitHealth>();
        if (baseHealth != null)
        {
            baseHealth.TakeDamage(attackDamage);
            Debug.Log($"[EnemyAI] {gameObject.name} inflige {attackDamage} dégâts à la base!");

            // Vérifier si la base est détruite
            if (!baseHealth.IsAlive() && TacticalDefenseGameManager.Instance != null)
            {
                TacticalDefenseGameManager.Instance.OnPlayerBaseDestroyed();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualiser la portée d'attaque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Visualiser la portée de détection
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Ligne vers la base
        if (playerBase != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, playerBase.position);
        }
    }
}
