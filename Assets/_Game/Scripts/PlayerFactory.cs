using Unity.VisualScripting;
using UnityEngine;

public class PlayerFactory : IFactory<Player>
{
    private Player _playerPrefab;

    public PlayerFactory(Player playerPrefab)
    {
        _playerPrefab = playerPrefab;
    }

    public Player Create()
    {
        var player  = Object.Instantiate(_playerPrefab);

        return player;
    }
}