using UnityEngine;

public class GameplaySceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;

    private Actions _actions;

    private void Awake()
    {
        _actions = new Actions();
        _actions.Enable();
    }

    private void OnDestroy()
    {
        _actions.Dispose();
    }

    private void Start()
    {
        IMoveInput moveInput = new PlayerInput(_actions);
        PlayerFactory factory = new PlayerFactory(_playerPrefab, moveInput);
        var player = factory.Create();
    }
}