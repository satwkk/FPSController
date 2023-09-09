using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private bool _pitchInverted = false;
    [SerializeField] private float _gravityMult = 9.8f;
    [SerializeField] private float _yawSmoothStep;
    [SerializeField] private float _pitchSmoothStep;
    [SerializeField] private float _sensitivity;

    private Camera _playerCamera;
    private Vector3 _movementVelocity;

    public float _currentSpeed = 0f;
    public float _currentSpeedVel_Var;
    public float _accelerationSmoothness = 0.5f;

    private CharacterController _characterController;

    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Poll Input here for now
        HandleGravity();
        HandleMovement();
        HandleCameraMovement();
    }

    private void HandleMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _isRunning = Input.GetKey(KeyCode.LeftShift);
        var movementDir = new Vector3(horizontal, 0f, vertical).normalized;
        var targetSpeed = 0f;

        // calculate player speed
        if (movementDir.magnitude == 0f)
            targetSpeed = 0f;
        else
            targetSpeed = _isRunning ? _runningSpeed : _movementSpeed;

        // _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _currentSpeedVel_Var, _accelerationSmoothness * Time.deltaTime);
        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, _accelerationSmoothness);
        movementDir = movementDir * _currentSpeed * Time.deltaTime;
        movementDir = transform.TransformDirection(movementDir);

        // Handle gravity
        if (!_isGrounded)
            movementDir.y -= _gravityMult * Time.deltaTime;

        _characterController.Move(movementDir);
    }

    private void HandleCameraMovement()
    {
        var yaw = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        var pitch = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        // Rotate the player body in yaw
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(0f, yaw, 0f), _yawSmoothStep);

        // Rotate the camera in pitch
        pitch = _pitchInverted ? pitch : -pitch;
        _playerCamera.transform.rotation = Quaternion.Slerp(
            _playerCamera.transform.rotation, 
            _playerCamera.transform.rotation * Quaternion.Euler((Mathf.Clamp(pitch, -45f, 45f)), 0f, 0f), 
            _pitchSmoothStep
        );

        // TODO: Clamp the camera pitch rotation
    }

    private void HandleGravity()
    {
        Vector3 downDir = -transform.up;
        if (!Physics.Raycast(transform.position, downDir, 2f))
        {
            // We do not hit the ground and is in air
            _isGrounded = false;
        }
        else
        {
            _isGrounded = true;
        }
    }
}
