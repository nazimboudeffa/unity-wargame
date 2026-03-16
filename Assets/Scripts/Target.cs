using UnityEngine;

// RequireComponent ensures that adding Target to an object auto-adds Selectable,
// so it is immediately clickable without extra manual steps.
[RequireComponent(typeof(Selectable))]
public class Target : MonoBehaviour
{
    [Header("Visuel")]
    public Color gizmoColor = Color.red;

    // Dessine un repère visible dans l'éditeur pour repérer la cible
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.DrawLine(transform.position + Vector3.up * 2f, transform.position);
    }
}
