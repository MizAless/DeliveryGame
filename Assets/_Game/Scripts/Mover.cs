using UnityEngine;

public class Mover : MonoBehaviour, ITickable
{
    [Header("Вынести в конфиг!!!")] [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float _rotationSpeed = 0.5f;

    private Transform _transform;
    private IMoveInput _moveInput;
    private IHorizontalAngleOffset _horizontalAngleOffset;
    private MapBounds _mapBounds;

    public void Init(IMoveInput moveInput, IHorizontalAngleOffset horizontalAngleOffset, MapBounds mapBounds)
    {
        _moveInput = moveInput;
        _horizontalAngleOffset = horizontalAngleOffset;
        _mapBounds = mapBounds;
    }

    private void Awake()
    {
        _transform = transform;
    }

    public void Tick()
    {
        Vector2 moveInput = _moveInput.GetMoveInput();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = Quaternion.Euler(0, _horizontalAngleOffset.HorizontalAngle, 0) * moveDirection;

        var nextPosition = _transform.position + moveDirection * (_speed * Time.deltaTime);
        
        if (!_mapBounds.IsAllowToMove(nextPosition))
            return;

        _transform.position = nextPosition;
        _transform.forward = Vector3.Lerp(_transform.forward, moveDirection, _rotationSpeed * Time.deltaTime);
    }
}