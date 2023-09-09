using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private Transform _weaponHolder;

    [Header("Mouse Sway Settings")]
    [SerializeField] private float _swayAmount;
    [SerializeField] private float _tiltAmount;
    [SerializeField] float _swayResetSmoothing;
    [SerializeField] float _swaySmoothing;
    [SerializeField] private float _swayClampX;
    [SerializeField] private float _swayClampY;
    Vector3 targetRotation;
    Vector3 newRotation;
    Vector3 targetRotationVelocity;
    Vector3 newRotationVelocity;

    [Header("Walking Sway Settings")]
    [SerializeField] private float _movementSwayAmount;
    [SerializeField] private float _movementSwayResetSmoothing;
    private Vector3 _targetMovementSwayVector;
    private Vector3 _newMovementSwayVector;
    private Vector3 _targetMovementSwayVelocity;
    private Vector3 _finalRotationVelocity;

    private void Update()
    {
        float f = Input.GetAxis("Mouse X");
        float p = Input.GetAxis("Mouse Y");
        HandleWeaponSway(f, p);
    }

    private void HandleWeaponSway(float f, float p)
    {
        float yaw = f * _swayAmount * Time.deltaTime;
        float pitch = p * _swayAmount * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * _movementSwayAmount * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") *  _movementSwayAmount * Time.deltaTime;

        /** Movement Sway Handling **/
        _targetMovementSwayVector = new Vector3(vertical, 0f, -horizontal * _tiltAmount);
        _targetMovementSwayVector.x = Mathf.Clamp(_targetMovementSwayVector.x, -_swayClampX, _swayClampX);
        _targetMovementSwayVector.y = Mathf.Clamp(_targetMovementSwayVector.y, -_swayClampY, _swayClampY);
        _targetMovementSwayVector = Vector3.SmoothDamp(_targetMovementSwayVector, Vector3.zero, ref _targetMovementSwayVelocity, _movementSwayResetSmoothing);
        _newMovementSwayVector = Vector3.SmoothDamp(_newMovementSwayVector, _targetMovementSwayVector, ref _finalRotationVelocity, _swaySmoothing);

        /** Mouse Sway Handling **/
        targetRotation += new Vector3(pitch, yaw, -yaw * _tiltAmount);
        targetRotation.x = Mathf.Clamp(targetRotation.x, -_swayClampX, _swayClampX);
        targetRotation.y = Mathf.Clamp(targetRotation.y, -_swayClampY, _swayClampY);
        targetRotation += _newMovementSwayVector;
        targetRotation = Vector3.SmoothDamp(targetRotation, Vector3.zero, ref targetRotationVelocity, _swayResetSmoothing);
        newRotation = Vector3.SmoothDamp(newRotation, targetRotation, ref newRotationVelocity, _swaySmoothing);
        _weaponHolder.localRotation = Quaternion.Euler(newRotation);
    }
}
