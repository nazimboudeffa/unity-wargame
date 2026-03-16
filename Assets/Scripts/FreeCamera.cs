using UnityEngine;
using UnityEngine.InputSystem;

// RTS-style camera (StarCraft 2 controls):
//   WASD / Arrow keys  — pan
//   Edge scrolling     — pan when cursor is near screen border
//   Scroll wheel       — zoom (move along look direction)
//   Right mouse drag   — rotate
public class FreeCamera : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed       = 20f;
    public float fastMultiplier = 3f;
    public bool  edgeScrolling  = true;
    public float edgeSize       = 30f;   // pixels from screen edge

    [Header("Zoom")]
    public float zoomSpeed = 0.3f;       // world units per scroll unit
    public float minHeight = 3f;
    public float maxHeight = 100f;

    [Header("Rotation")]
    public float rotateSensitivity = 0.25f;

    private float _yaw;
    private float _pitch;

    void Start()
    {
        _yaw   = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;

        // Cursor is always free — no locking
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
        HandleRotate();
    }

    void HandlePan()
    {
        if (Keyboard.current == null) return;

        float speed = panSpeed * (Keyboard.current.leftShiftKey.isPressed ? fastMultiplier : 1f);

        // Panning stays horizontal regardless of camera tilt
        Quaternion flatRot = Quaternion.Euler(0f, _yaw, 0f);
        Vector3 forward    = flatRot * Vector3.forward;
        Vector3 right      = flatRot * Vector3.right;

        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)    move += forward;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)  move -= forward;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)  move -= right;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) move += right;

        if (edgeScrolling && Mouse.current != null)
        {
            Vector2 mp = Mouse.current.position.ReadValue();
            if (mp.x < edgeSize)                 move -= right;
            if (mp.x > Screen.width  - edgeSize) move += right;
            if (mp.y < edgeSize)                 move -= forward;
            if (mp.y > Screen.height - edgeSize) move += forward;
        }

        if (move.sqrMagnitude > 0f)
            transform.position += move.normalized * speed * Time.deltaTime;
    }

    void HandleZoom()
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) < 0.01f) return;

        Vector3 next = transform.position + transform.forward * (scroll * zoomSpeed);
        next.y = Mathf.Clamp(next.y, minHeight, maxHeight);
        transform.position = next;
    }

    // Minimum mouse movement in pixels before right-click becomes a drag-rotate
    private const float RotateDragThreshold = 8f;
    private Vector2 _rightPressStart;
    private bool    _isRotating;

    void HandleRotate()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _rightPressStart = Mouse.current.position.ReadValue();
            _isRotating      = false;
        }

        if (Mouse.current.rightButton.isPressed)
        {
            if (!_isRotating &&
                Vector2.Distance(Mouse.current.position.ReadValue(), _rightPressStart) > RotateDragThreshold)
                _isRotating = true;

            if (_isRotating)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                _yaw   += delta.x * rotateSensitivity;
                _pitch -= delta.y * rotateSensitivity;
                _pitch  = Mathf.Clamp(_pitch, -89f, 89f);
                transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            }
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
            _isRotating = false;
    }
}
