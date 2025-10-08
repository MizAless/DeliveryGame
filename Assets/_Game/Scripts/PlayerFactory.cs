using Unity.VisualScripting;
using UnityEngine;

public class PlayerFactory : IFactory<Player>
{
    private Player _playerPrefab;
    private IMoveInput _moveInput;

    public PlayerFactory(Player playerPrefab, IMoveInput moveInput)
    {
        _playerPrefab = playerPrefab;
        _moveInput = moveInput;
    }

    public Player Create()
    {
        var player  = Object.Instantiate(_playerPrefab);
        var mover = player.GetComponent<Mover>();
        mover.Initialize(_moveInput);

        return player;
    }
}