using UnityEngine;
using UnityEngine.InputSystem;

// SC2-style controls:
//   Left click on friendly unit (has UnitMovement)       -- select it
//   Right click on enemy (has Selectable, no UnitMovement) -- attack order (auto-fires)
//   Right click on ground                                -- move order
//   Escape                                               -- cancel selection & attack
//
// SETUP:
//   - Friendly units: add Selectable + UnitMovement + LaunchProjectile
//   - Enemies / targets: add Selectable (+ Collider)
public class TargetSelector : MonoBehaviour
{
    [Header("Setup")]
    public Camera cam;

    private Selectable       _selectedUnit;  // friendly unit selected for orders
    private LaunchProjectile _unitLauncher;  // launcher on the selected unit
    private Selectable       _attackTarget;  // current enemy being attacked

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null || Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DeselectUnit();
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)  TrySelect();
        if (Mouse.current.rightButton.wasPressedThisFrame) TryOrder();
    }

    // Left click: select friendly unit
    void TrySelect()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit info))
        {
            Selectable hit = info.collider.GetComponentInParent<Selectable>();
            if (hit != null && hit.transform.root.GetComponentInChildren<UnitMovement>() != null)
            {
                if (hit == _selectedUnit) return;
                DeselectUnit();
                _selectedUnit = hit;
                _unitLauncher = hit.transform.root.GetComponentInChildren<LaunchProjectile>();
                _selectedUnit.SetSelected(true);
                Debug.Log(_unitLauncher != null
                    ? $"[Selector] Unit selected: {_selectedUnit.name} (launcher found)"
                    : $"[Selector] Unit selected: {_selectedUnit.name} (WARNING: no LaunchProjectile found on this unit)");
                return;
            }
        }

        DeselectUnit();
    }

    void DeselectUnit()
    {
        CancelAttack();
        if (_selectedUnit == null) return;
        _selectedUnit.SetSelected(false);
        _selectedUnit = null;
        _unitLauncher = null;
    }

    // Right click: attack order (enemy) or move order (ground)
    void TryOrder()
    {
        if (_selectedUnit == null) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit info)) return;

        Selectable hitSel = info.collider.GetComponentInParent<Selectable>();
        bool isEnemy = hitSel != null
                    && hitSel.transform.root.GetComponentInChildren<UnitMovement>() == null
                    && hitSel != _selectedUnit;

        if (isEnemy)
        {
            CancelAttack();
            _attackTarget = hitSel;
            _attackTarget.SetSelected(true);

            // Re-try lookup in case it was null at selection time
            if (_unitLauncher == null)
                _unitLauncher = _selectedUnit.transform.root.GetComponentInChildren<LaunchProjectile>();

            if (_unitLauncher != null)
            {
                // Aim at renderer bounds center for accuracy; fallback to root transform
                Transform aimTarget = _attackTarget.transform.root;
                Renderer rend = aimTarget.GetComponentInChildren<Renderer>();
                _unitLauncher.SetTarget(aimTarget, rend != null ? rend.bounds.center : aimTarget.position);
                Debug.Log($"[Selector] Attack order: {_selectedUnit.name} -> {_attackTarget.name} at {(rend != null ? rend.bounds.center : aimTarget.position)}");
            }
            else
            {
                Debug.LogWarning($"[Selector] {_selectedUnit.name} has no LaunchProjectile -- check that the component is added and the script compiles with no errors.");
            }
        }
        else
        {
            // Move order
            CancelAttack();
            UnitMovement mover = _selectedUnit.transform.root.GetComponentInChildren<UnitMovement>();
            if (mover != null)
            {
                mover.MoveTo(info.point);
                Debug.Log($"[Selector] Move order: {_selectedUnit.name} -> {info.point}");
            }
        }
    }

    void CancelAttack()
    {
        if (_attackTarget != null)
        {
            _attackTarget.SetSelected(false);
            _attackTarget = null;
        }
        if (_unitLauncher != null)
            _unitLauncher.ClearTarget();
    }
}
