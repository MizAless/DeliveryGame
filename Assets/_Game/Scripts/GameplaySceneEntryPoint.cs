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
        ServiceLocator.Init();
        
        PlayerInput moveInput = new PlayerInput(_actions);
        PlayerFactory factory = new PlayerFactory(_playerPrefab);
        
        var followCameraData = new FollowCameraData()
        {
            Distance = 5,
            VerticalAngle = 45,
            HorizontalAngle = 45,
        };
        
        HorizontalAngleOffset horizontalAngleOffset = new HorizontalAngleOffset(_camera, followCameraData);

        ServiceLocator.Register<IMoveInput>(moveInput);
        ServiceLocator.Register<IHorizontalAngleOffset>(horizontalAngleOffset);
        
        var player = factory.Create();
        
        horizontalAngleOffset.SetTarget(player.transform);
        
        var mover = player.GetComponent<Mover>();
        
        mover.Init(moveInput, horizontalAngleOffset);
        
        _updater.Register(moveInput);
        _updater.Register(horizontalAngleOffset);
        _updater.Register(mover);
    }
}