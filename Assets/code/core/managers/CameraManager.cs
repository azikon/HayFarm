using Cinemachine;

using DG.Tweening;

using Zenject;

namespace Core.Managers
{
    public class CameraManager : BaseManager
    {
        protected override void OnLoad()
        {
            GameManager.Container.Resolve<SignalBus>().Subscribe<GameStateChangeSignal>( HandleState );
        }

        private void HandleState( GameStateChangeSignal changedState )
        {
            if ( changedState.State == Enums.GameStates.InGame )
            {
                TransitionCameraPathStart();
            }
        }

        private void TransitionCameraPathStart()
        {
            SceneObjectsManager sceneObjectsManager = GameManager.Container.Resolve<SceneObjectsManager>();

            CinemachineVirtualCamera CinemachineVirtualCamera = sceneObjectsManager.CinemachineVirtualCamera;
            CinemachineVirtualCamera CinemachineVirtualCameraPath = sceneObjectsManager.CinemachineVirtualCameraPath;

            CinemachineVirtualCamera.gameObject.SetActive( false );
            CinemachineVirtualCameraPath.gameObject.SetActive( true );

            CinemachineTrackedDolly cinemachineTrackedDolly = CinemachineVirtualCameraPath.GetCinemachineComponent<CinemachineTrackedDolly>();

            DOTween.To( () => cinemachineTrackedDolly.m_PathPosition, x => cinemachineTrackedDolly.m_PathPosition = x, 1, 3f )
                .OnComplete( () =>
                {
                    CinemachineVirtualCameraPath.gameObject.SetActive( false );
                    CinemachineVirtualCamera.gameObject.SetActive( true );
                }
                );
        }
    }
}