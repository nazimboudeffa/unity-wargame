using UnityEngine;

// Add this component to any unit you want to move with right-click.
// No NavMesh or baking required — the unit slides directly to the destination.
public class UnitMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    [Tooltip("Unit stops when within this distance of the destination.")]
    public float stoppingDistance = 0.2f;
    [Tooltip("Axes allowed for movement: (1,0,1)=horizontal only, (1,1,1)=3D movement including vertical, (1,0,0)=X-axis only, etc.")]
    public Vector3 movementAxes = new Vector3(1f, 0f, 1f);
    [Tooltip("Rotation offset (X, Y, Z) in degrees to correct model orientation. Typically Y=90 if model faces right, Y=-90 if facing left.")]
    public Vector3 modelRotationOffset = new Vector3(0f, 90f, 0f);

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

        // Apply movement axes filter (e.g., constrain to horizontal plane)
        diff.x *= movementAxes.x;
        diff.y *= movementAxes.y;
        diff.z *= movementAxes.z;

        if (diff.magnitude <= stoppingDistance)
        {
            _destination = null;
            return;
        }

        // Rotate toward destination with model offset applied (only if there's a direction)
        if (diff.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(diff.normalized);
            transform.rotation = targetRotation * Quaternion.Euler(modelRotationOffset);
        }

        // Move
        transform.position += diff.normalized * moveSpeed * Time.deltaTime;
    }
}
