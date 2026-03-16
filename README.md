# Unity Wargame

RTS-style game prototype with missile launcher units, camera controls, and target selection.

---

## Controls

### Camera
| Input | Action |
|-------|--------|
| `W` / `A` / `S` / `D` or Arrow Keys | Pan camera |
| Scroll Wheel | Zoom in / out |
| Right Mouse Drag | Rotate camera |
| `Left Shift` (hold) | Fast pan (3x speed) |

### Unit Selection & Orders
| Input | Action |
|-------|--------|
| Left Click on friendly unit | Select unit |
| Right Click on enemy (Target) | Issue attack order — fires one missile |
| Right Click on ground | Issue move order — unit walks to that point |
| `Escape` | Deselect unit / cancel attack |

### Missile (manual, when unit is selected)
| Input | Action |
|-------|--------|
| `Space` | Manually fire missile at current target |

---

## Scene Setup

### Friendly Unit (MissileLauncher / Tank)
Add these components to the root GameObject:
- **Selectable** — enables click detection and highlight
- **UnitMovement** — handles right-click move orders
  - **Movement Axes** — constrain movement (default `1,0,1` = horizontal only; `1,1,1` = full 3D for flying units)
  - **Model Rotation Offset** — correct model orientation in degrees (e.g., `0,90,0` if model faces right)
- **LaunchProjectile** — fires missiles; assign *Launch Point* and *Projectile Prefab* in the Inspector
- **Collider** — required for raycasting

Add one child GameObject:
- **LaunchPoint** — empty GameObject positioned at the barrel tip; drag it into the *Launch Point* field of LaunchProjectile

### Enemy / Target
Add these components to the root GameObject:
- **Selectable** — enables click detection and highlight
- **Collider** — required for raycasting
- **Target** *(optional)* — auto-adds Selectable; used for gizmo drawing

### Projectile Prefab (Missile)
The missile prefab should have:
- **Projectile** component with:
  - **Use Arc** — enable ballistic trajectory (launch angle 45° by default)
  - **Launch Angle** — arc steepness (5-85°)
  - **Speed** — movement speed
  - **Life** — auto-destroy after X seconds
- **Collider** — sphere or capsule collider for impact detection
- **Rigidbody** — set to kinematic (script handles movement)

---

## Features

- **RTS Camera** — WASD movement, scroll zoom, right-drag rotation
- **Unit Selection** — left-click to select friendly units with visual highlight
- **Move Orders** — right-click ground to move selected unit
- **Attack Orders** — right-click enemy to fire missile
- **Ballistic Missiles** — arc trajectory with target tracking
- **Model Orientation Correction** — per-unit rotation offset for FBX models with non-standard forward axes
- **3-Axis Movement Control** — constrain units to horizontal, vertical, or full 3D movement

---

## Scripts

### Camera
- **FreeCamera.cs** — RTS-style camera with pan, zoom, and rotation

### Unit Control
- **UnitMovement.cs** — movement system with axis constraints and model rotation correction
- **TargetSelector.cs** — handles left-click selection and right-click orders
- **Selectable.cs** — visual highlighting for selected units

### Combat
- **LaunchProjectile.cs** — missile firing system with aim point tracking
- **Projectile.cs** — ballistic arc trajectory with target following
- **Target.cs** — basic target marker component
