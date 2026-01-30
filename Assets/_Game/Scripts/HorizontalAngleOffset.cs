using UnityEngine;

public class HorizontalAngleOffset : ILateTickable, IHorizontalAngleOffset, IService, ITickable
{
    private readonly Camera _camera;
    
    private Transform _target;

    private float _distance;
    private float _verticalAngle;
    private float _horizontalAngle;

    public HorizontalAngleOffset(Camera camera, FollowCameraData data)
    {
        _camera = camera;

        _distance       = data.Distance;
        _verticalAngle  = data.VerticalAngle;  
        _horizontalAngle= data.HorizontalAngle;
    }

    public float HorizontalAngle => _horizontalAngle;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void LateTick()
    {
        Quaternion orbitRotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0f);
        
        Vector3 offsetOnSphere = orbitRotation * (Vector3.back * _distance);

        Vector3 desiredPosition = _target.position + offsetOnSphere;
        Quaternion lookAtTarget = Quaternion.LookRotation(_target.position - desiredPosition, Vector3.up);

        _camera.transform.SetPositionAndRotation(desiredPosition, lookAtTarget);
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _verticalAngle += 1;
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
            _verticalAngle += -1;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _distance += 0.5f;
            Debug.Log(_distance);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _distance += -0.5f;
            Debug.Log(_distance);
        }
    }
}