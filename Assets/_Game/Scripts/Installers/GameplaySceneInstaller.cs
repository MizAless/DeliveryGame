// using UnityEngine;
// using Zenject;
//
// public class GameplaySceneInstaller : MonoInstaller
// {
//     [SerializeField] private Player _playerPrefab;
//     [SerializeField] private Updater _updater;
//     [SerializeField] private Camera _camera;
//     
//     [SerializeField] private FollowCameraData _followCameraData;
//
//     public override void InstallBindings()
//     {
//         Container.Bind<Actions>().AsTransient().NonLazy();
//         
//         Container.Bind<IMoveInput>().To<PlayerInput>().AsSingle().NonLazy();
//         
//         Container.Bind<IFollowCamera>().To<FollowCamera>().AsSingle().NonLazy();
//     }
// }
