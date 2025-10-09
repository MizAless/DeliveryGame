using UnityEngine;

public class FollowCamera : ITickable
{
    private readonly Camera _camera;
    private readonly Transform _target;

    private float _distance;
    private float _verticalAngle;
    private float _horizontalAngle;

    public FollowCamera(Camera camera, Transform target, FollowCameraData data)
    {
        _camera = camera;
        _target = target;
        
        _distance = data.Distance;
        _verticalAngle = data.VerticalAngle;
        _horizontalAngle = data.HorizontalAngle;
    }

    public void Tick()
    {
        var offset = Vector3.forward * _distance;  
        // var direction = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0) * offset;
        
        var direction = Quaternion.identity * new Vector3(_horizontalAngle, 0, -_verticalAngle);
        
        // direction *= _distance;

        _camera.transform.position = _target.position;
        _camera.transform.rotation = Quaternion.Euler(direction);
        
        
        _camera.transform.position += Vector3.forward * _distance;
        _camera.transform.LookAt(_target.position);
    }
}

public struct FollowCameraData
{
    public float Distance;
    public float VerticalAngle;
    public float HorizontalAngle;
}