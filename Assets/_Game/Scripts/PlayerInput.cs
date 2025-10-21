using UnityEngine;

public class PlayerInput : IMoveInput, ITickable, IService
{
    private readonly Actions _controls;

    private Vector2 _moveInput;
    
    public PlayerInput(Actions controls)
    {
        _controls = controls;
    }

    public void Tick()
    {
        _moveInput = _controls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMoveInput() => _moveInput;
}