using UnityEngine;

public class Mover : MonoBehaviour, ITickable
{
    [Header("Вынести в конфиг!!!")] [SerializeField]
    private float _speed = 5f;

    private Transform _transform;
    private IMoveInput _moveInput;
    private IHorizontalAngleOffset _horizontalAngleOffset;

    public void Init(IMoveInput moveInput, IHorizontalAngleOffset horizontalAngleOffset)
    {
        _moveInput = moveInput;
        _horizontalAngleOffset = horizontalAngleOffset;
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

        _transform.position += moveDirection * (_speed * Time.deltaTime);
    }
}