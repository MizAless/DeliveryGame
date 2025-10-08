using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Вынести в конфиг!!!")] [SerializeField]
    private float _speed = 5f;

    private Transform _transform;
    private IMoveInput _moveInput;

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

        _transform.Translate(moveInput * (_speed * Time.deltaTime));
    }

    public void Initialize(IMoveInput moveInput)
    {
        _isInitialized = true;

        _moveInput = moveInput;
    }
}