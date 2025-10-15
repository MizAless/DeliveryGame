using UnityEngine;

public class FollowCameraService : ITickable, IService, IFollowCamera
{
    private readonly Camera _camera;
    private readonly Transform _target;

    private float _distance;
    private float _verticalAngle;
    private float _horizontalAngle;

    public FollowCameraService(Camera camera, Transform target, FollowCameraData data)
    {
        _camera = camera;
        _target = target;

        _distance       = data.Distance;
        _verticalAngle  = data.VerticalAngle;  
        _horizontalAngle= data.HorizontalAngle;
    }

    public float HorizontalAngle => _horizontalAngle;

    public void Tick()
    {
        Quaternion orbitRotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0f);
        
        Vector3 offsetOnSphere = orbitRotation * (Vector3.back * _distance);

        Vector3 desiredPosition = _target.position + offsetOnSphere;
        Quaternion lookAtTarget = Quaternion.LookRotation(_target.position - desiredPosition, Vector3.up);

        _camera.transform.SetPositionAndRotation(desiredPosition, lookAtTarget);
    }
}

public interface IFollowCamera
{
    float HorizontalAngle { get; } 
}

public struct FollowCameraData
{
    public float Distance;
    public float VerticalAngle;
    public float HorizontalAngle;
}