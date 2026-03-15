using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCamera : MonoBehaviour
{
    [Header("Vitesse")]
    public float moveSpeed = 10f;
    public float fastMultiplier = 3f;   // maintenir Shift pour accélérer
    public float mouseSensitivity = 0.15f;

    private float _pitch; // rotation verticale
    private float _yaw;   // rotation horizontale

    void Start()
    {
        _yaw   = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();

        // Échap pour libérer le curseur
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        // Clic gauche pour recapturer le curseur
        if (Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }
    }

    void HandleRotation()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        _yaw   += mouseDelta.x * mouseSensitivity;
        _pitch -= mouseDelta.y * mouseSensitivity;
        _pitch  = Mathf.Clamp(_pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    void HandleMovement()
    {
        float speed = moveSpeed;
        if (Keyboard.current.leftShiftKey.isPressed)
            speed *= fastMultiplier;

        Vector3 dir = Vector3.zero;

        if (Keyboard.current.zKey.isPressed || Keyboard.current.wKey.isPressed) dir += transform.forward;
        if (Keyboard.current.sKey.isPressed)                                     dir -= transform.forward;
        if (Keyboard.current.qKey.isPressed || Keyboard.current.aKey.isPressed) dir -= transform.right;
        if (Keyboard.current.dKey.isPressed)                                     dir += transform.right;
        if (Keyboard.current.eKey.isPressed)                                     dir += Vector3.up;
        if (Keyboard.current.cKey.isPressed)                                     dir -= Vector3.up;

        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
