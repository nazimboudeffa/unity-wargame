# Wargame

This is some tests and explorations on Unity

---

## Controls

### Camera
| Input | Action |
|-------|--------|
| `W` / `A` / `S` / `D` or Arrow Keys | Pan camera |
| Mouse edge scroll | Pan camera |
| Scroll Wheel | Zoom in / out |
| Right Mouse Drag | Rotate camera |

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

### Friendly Unit (MissileLauncher)
Add these components to the root GameObject:
- **Selectable** — enables click detection and highlight
- **UnitMovement** — handles right-click move orders
- **LaunchProjectile** — fires missiles; assign *Launch Point* and *Projectile Prefab* in the Inspector
- **Collider** — required for raycasting

Add one child GameObject:
- **LaunchPoint** — empty GameObject positioned at the barrel tip; drag it into the *Launch Point* field of LaunchProjectile

### Enemy / Target
Add these components to the root GameObject:
- **Selectable** — enables click detection and highlight
- **Collider** — required for raycasting
- **Target** *(optional)* — auto-adds Selectable; used for gizmo drawing
