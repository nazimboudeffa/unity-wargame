using UnityEngine;

// Add this component to any unit you want to move with right-click.
// No NavMesh or baking required — the unit slides directly to the destination.
public class UnitMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    [Tooltip("Unit stops when within this distance of the destination.")]
    public float stoppingDistance = 0.2f;

    [Header("Move marker")]
    [Tooltip("Optional prefab spawned briefly at the destination (like SC2's green circle).")]
    public GameObject moveMarkerPrefab;
    public float markerDuration = 0.6f;

    private Vector3? _destination;

    public void MoveTo(Vector3 destination)
    {
        _destination = destination;

        if (moveMarkerPrefab != null)
        {
            GameObject marker = Instantiate(moveMarkerPrefab, destination, Quaternion.identity);
            Destroy(marker, markerDuration);
        }
    }

    void Update()
    {
        if (_destination == null) return;

        Vector3 dest = _destination.Value;
        Vector3 diff = dest - transform.position;

        // Ignore vertical distance so the unit doesn't try to fly/sink
        diff.y = 0f;

        if (diff.magnitude <= stoppingDistance)
        {
            _destination = null;
            return;
        }

        // Rotate toward destination
        transform.rotation = Quaternion.LookRotation(diff.normalized);

        // Move
        transform.position += diff.normalized * moveSpeed * Time.deltaTime;
    }
}
