using UnityEngine;

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
