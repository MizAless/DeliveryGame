using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour, ITickable
{
    [Header("Вынести в конфиг!!!")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 0.5f;
    
    [Header("Gravity")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundCheckDistance = 0.1f;

    private Transform _transform;
    private CharacterController _characterController;
    private IMoveInput _moveInput;
    private IHorizontalAngleOffset _horizontalAngleOffset;
    private MapBounds _mapBounds;
    
    private float _verticalVelocity;

    public void Init(IMoveInput moveInput, IHorizontalAngleOffset horizontalAngleOffset, MapBounds mapBounds)
    {
        _moveInput = moveInput;
        _horizontalAngleOffset = horizontalAngleOffset;
        _mapBounds = mapBounds;
    }

    private void Awake()
    {
        _transform = transform;
        
        _characterController = GetComponent<CharacterController>();
    }

    public void Tick()
    {
        Vector2 moveInput = _moveInput.GetMoveInput();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = Quaternion.Euler(0, _horizontalAngleOffset.HorizontalAngle, 0) * moveDirection;

        Vector3 desiredMovement = moveDirection * (_speed * Time.deltaTime);
    
        Vector3 actualMovement = _mapBounds.GetValidatedMovement(_transform.position, desiredMovement);
    
        // Гравитация
        if (_characterController.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f; // Небольшое прижатие к земле
        }
        
        _verticalVelocity += _gravity * Time.deltaTime;
        
        // Добавляем вертикальное движение к горизонтальному
        Vector3 finalMovement = actualMovement;
        finalMovement.y = _verticalVelocity * Time.deltaTime;
        
        _characterController.Move(finalMovement);
    
        if (actualMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(actualMovement.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}