using UnityEngine;

public class GameplaySceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Updater _updater;
    [SerializeField] private Camera _camera;
    
    [SerializeField] private DeliveryObject _deliveryObjectPrefab;
    [SerializeField] private DeliveryRecipient _deliveryRecipientPrefab;
    
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDistance;

    [SerializeField] private int _startChunkCount;
    [SerializeField] private ChunkView _chunkPrefab;
    
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

        MapGenerator mapGenerator = new MapGenerator(_startChunkCount, _chunkPrefab);
        mapGenerator.Generate();
        
        PlayerInput moveInput = new PlayerInput(_actions);
        PlayerFactory playerFactory = new PlayerFactory(_playerPrefab);
        
        DeliveryObjectFactory deliveryObjectFactory = new DeliveryObjectFactory(_deliveryObjectPrefab);
        DeliveryRecipientFactory deliveryRecipientFactory = new DeliveryRecipientFactory(_deliveryRecipientPrefab);
        
        var followCameraData = new FollowCameraData()
        {
            Distance = 8,
            VerticalAngle = 57,
            HorizontalAngle = 45,
        };
        
        HorizontalAngleOffset horizontalAngleOffset = new HorizontalAngleOffset(_camera, followCameraData);

        ServiceLocator.Register<IMoveInput>(moveInput);
        ServiceLocator.Register<IHorizontalAngleOffset>(horizontalAngleOffset);
        
        var player = playerFactory.Create();
        var navigationArrow = player.GetComponentInChildren<NavigationArrow>();
        
        NavigationSystem navigationSystem = new NavigationSystem(navigationArrow);
        
        horizontalAngleOffset.SetTarget(player.transform);
        
        var mover = player.GetComponent<Mover>();
        var deliveryMan = player.GetComponent<DeliveryMan>();

        deliveryMan.Init(deliveryObjectFactory);
        
        RandomPlacer randomPlacer = new RandomPlacer(_spawnPoint.position, _spawnDistance); 
        
        DeliverySystem deliverySystem = new DeliverySystem(deliveryMan, deliveryObjectFactory, deliveryRecipientFactory, randomPlacer);
        
        mover.Init(moveInput, horizontalAngleOffset);
        
        EventsSystem eventsSystem = new EventsSystem();
        
        _updater.Register(moveInput);
        _updater.Register(horizontalAngleOffset as ILateTickable);
        _updater.Register(horizontalAngleOffset as ITickable);
        _updater.Register(mover);
        _updater.Register(deliveryMan);
        _updater.Register(navigationArrow);
        _updater.Register(deliverySystem);
        _updater.Register(navigationSystem);
        _updater.Register(eventsSystem);
    }
}