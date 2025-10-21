using UnityEngine;
using Zenject;

public class Mover : MonoBehaviour
{
    [Header("Вынести в конфиг!!!")] [SerializeField]
    private float _speed = 5f;

    private Transform _transform;
    private IMoveInput _moveInput;
    private IFollowCamera _followCamera;

    [Inject]
    public void Construct(IMoveInput moveInput, IFollowCamera followCamera)
    {
        _moveInput = moveInput;
        _followCamera = followCamera;
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        Vector2 moveInput = _moveInput.GetMoveInput();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        _transform.position += moveDirection * (_speed * Time.deltaTime);
    }
}