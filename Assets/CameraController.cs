using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _lookAt;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _yawRotationSpeed;
    public float _pitch;

    private void Start()
    {
        _pitch *= Mathf.Deg2Rad;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        Vector3 direction = _lookAt.position - transform.position;
        float distance = direction.magnitude;
        distance = Mathf.Clamp(distance, _minDistance, _maxDistance);
        direction.y = 0.0f;
        direction.Normalize();

        float yaw = 0;

        yaw = Mathf.Atan2(direction.x, direction.z);

        yaw -= Input.GetAxis("Horizontal") * _yawRotationSpeed * Mathf.Deg2Rad * Time.deltaTime;

        direction = new Vector3(Mathf.Sin(yaw) * Mathf.Cos(_pitch), Mathf.Sin(_pitch),
            Mathf.Cos(yaw) * Mathf.Cos(_pitch));
        Vector3 desiredPostion = _lookAt.position - direction * distance;
        transform.position = desiredPostion;
        transform.LookAt(_lookAt.position);
    }
}
