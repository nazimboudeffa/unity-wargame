using UnityEngine;

/// <summary>
/// Add this component to any GameObject you want to be clickable / box-selectable.
/// The object (or one of its children) must have a Collider for raycasting to work.
/// </summary>
[DisallowMultipleComponent]
public class Selectable : MonoBehaviour
{
    [Header("Highlight")]
    [Tooltip("HDR emission color applied when selected. Increase intensity for a stronger glow.")]
    public Color highlightColor = new Color(0f, 2f, 0f);

    public bool IsSelected { get; private set; }

    private Material _mat;
    private Color    _origBase;

    void Start()
    {
        // Warn early if there's no collider anywhere in the hierarchy
        if (GetComponentInChildren<Collider>() == null)
            Debug.LogWarning($"[Selectable] '{name}' has no Collider — it cannot be raycasted. " +
                             "Add a Collider (Box, Sphere, Mesh…) to this object or a child.", this);
    }

    /// <summary>Called by TargetSelector to select or deselect this object.</summary>
    public void SetSelected(bool value)
    {
        if (IsSelected == value) return;
        IsSelected = value;
        ApplyHighlight(value);
    }

    void ApplyHighlight(bool on)
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null) return;

        if (on)
        {
            _mat = rend.material; // creates a per-instance copy — does not modify the shared asset

            if (_mat.HasProperty("_BaseColor"))
            {
                _origBase = _mat.GetColor("_BaseColor");
                _mat.SetColor("_BaseColor", Color.Lerp(_origBase, highlightColor, 0.5f));
            }
            else
            {
                _origBase  = _mat.color;
                _mat.color = Color.Lerp(_origBase, highlightColor, 0.5f);
            }
        }
        else
        {
            if (_mat == null) return;

            if (_mat.HasProperty("_BaseColor"))
                _mat.SetColor("_BaseColor", _origBase);
            else
                _mat.color = _origBase;
        }
    }

    // Restore material if destroyed while selected
    void OnDestroy()
    {
        if (IsSelected) ApplyHighlight(false);
    }
}
