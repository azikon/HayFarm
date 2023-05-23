using Cinemachine;

using UnityEngine;

namespace Core.Managers
{
    public class SceneObjectsManager : BaseManagerBehaviour
    {
        protected override void OnLoad()
        {
            base.OnLoad();
        }

        [SerializeField]
        private GameObject _rootUI;
        [SerializeField]
        private GameObject _rootEnvironment;
        [SerializeField]
        private GameObject _rootCharacter;
        [SerializeField]
        private GameObject _rootCharacterMain;
        [SerializeField]
        private Camera _environmentCamera;
        [SerializeField]
        private Camera _uiCamera;
        [SerializeField]
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField]
        private CinemachineVirtualCamera _cinemachineVirtualCameraPath;

        public GameObject RootUI
        {
            get
            {
                return _rootUI;
            }
        }

        public GameObject RootEnvironment
        {
            get
            {
                return _rootEnvironment;
            }
        }

        public GameObject RootCharacter
        {
            get
            {
                return _rootCharacter;
            }
        }

        public GameObject RootCharacterMain
        {
            get
            {
                return _rootCharacterMain;
            }
        }

        public Camera EnvironmentCamera
        {
            get
            {
                return _environmentCamera;
            }
        }

        public Camera UICamera
        {
            get
            {
                return _uiCamera;
            }
        }

        public CinemachineVirtualCamera CinemachineVirtualCamera
        {
            get
            {
                return _cinemachineVirtualCamera;
            }
        }

        public CinemachineVirtualCamera CinemachineVirtualCameraPath
        {
            get
            {
                return _cinemachineVirtualCameraPath;
            }
        }
    }
}