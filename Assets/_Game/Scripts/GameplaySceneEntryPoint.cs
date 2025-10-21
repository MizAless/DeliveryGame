using UnityEngine;

public class GameplaySceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    
    [SerializeField] private Updater _updater;

    [SerializeField] private Camera _camera;
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
        // ServiceLocator.Init();
        
        IMoveInput moveInput = new PlayerInput(_actions);
        PlayerFactory factory = new PlayerFactory(_playerPrefab);
        
        var followCameraData = new FollowCameraData()
        {
            Distance = 5,
            VerticalAngle = 45,
            HorizontalAngle = 45,
        };
        
        IFollowCamera followCamera = new FollowCamera(_camera, player.transform, followCameraData);

        // ServiceLocator.Register<IMoveInput>((IService)moveInput);
        // ServiceLocator.Register<IFollowCamera>((IService)followCamera);
        
        var player = factory.Create();
        
        _updater.Register((ITickable)moveInput);
        _updater.Register((ILateTickable)followCamera);
    }
}