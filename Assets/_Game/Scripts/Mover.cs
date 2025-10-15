using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Вынести в конфиг!!!")] [SerializeField]
    private float _speed = 5f;

    private Transform _transform;
    private IMoveInput _moveInput;
    private IFollowCamera _followCamera;

    private bool _isInitialized = false;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (!_isInitialized)
            return;

        Vector2 moveInput = _moveInput.GetMoveInput();

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        

        _transform.position += moveDirection * (_speed * Time.deltaTime);
    }

    public void Initialize(IMoveInput moveInput, IFollowCamera followCamera)
    {
        _isInitialized = true;

        _moveInput = moveInput;
        _followCamera = followCamera;
    }
}