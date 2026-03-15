using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSelector : MonoBehaviour
{
    [Header("Setup")]
    public LaunchProjectile launcher;
    public Camera cam;

    [Header("Visuel")]
    public Color selectedColor   = Color.green;
    public Color unselectedColor = Color.red;

    private Target _selected;

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        // Ctrl + Clic gauche pour sélectionner une cible
        bool ctrl = Keyboard.current != null && Keyboard.current.ctrlKey.isPressed;
        if (ctrl && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            TrySelectTarget();
    }

    void TrySelectTarget()
    {
        // Libère le curseur temporairement le temps du raycast
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Target t = hit.collider.GetComponentInParent<Target>();
            if (t != null)
                SelectTarget(t);
        }
    }

    void SelectTarget(Target t)
    {
        // Désélectionne l'ancienne cible
        if (_selected != null)
            SetTargetColor(_selected, unselectedColor);

        _selected = t;
        SetTargetColor(_selected, selectedColor);

        // Transmet au lanceur
        if (launcher != null)
            launcher.target = _selected.transform;

        Debug.Log($"Cible sélectionnée : {_selected.name}");
    }

    void SetTargetColor(Target t, Color color)
    {
        Renderer rend = t.GetComponentInChildren<Renderer>();
        if (rend != null)
            rend.material.color = color;
    }
}
