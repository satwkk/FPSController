using UnityEngine;

public class WeaponCollisionDetector : MonoBehaviour
{
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Transform _weaponTransform;
    [SerializeField] private float _collisionCheckDist = 10f;
    [SerializeField] private float _maxThreshold = 0.2f;
    [SerializeField] private float _snappiness = 1f;
    
    void Update()
    {
        Debug.DrawLine(_weaponTransform.position, _weaponTransform.position + _weaponTransform.right * _collisionCheckDist);
        if (Physics.Raycast(_weaponTransform.position, _weaponTransform.right, out RaycastHit hitInfo, _collisionCheckDist))
        {
            // Gizmos.DrawWireSphere(hitInfo.point, 0.05f);
            Gizmos.color = Color.red;
            Debug.DrawRay(hitInfo.point, hitInfo.normal);
            var distance = Vector3.Distance(_weaponTransform.position, hitInfo.point);
            if (distance <= _maxThreshold)
            {
                _weaponHolder.localRotation = Quaternion.Slerp(_weaponHolder.localRotation, Quaternion.Euler(0f, -180f, 0f), _snappiness * Time.deltaTime);
                print("Gun should rotate now");
            }
        }
        else
        {
            _weaponHolder.localRotation = Quaternion.Slerp(_weaponHolder.localRotation, Quaternion.Euler(0f, -90f, 0f), _snappiness * Time.deltaTime);
        }
    }
}
