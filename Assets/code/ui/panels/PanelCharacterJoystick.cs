using Core.Managers;
using Core.Signals;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelCharacterJoystick : UIBasePanel, ITickable
    {
        [SerializeField]
        private Image _joystickRing;
        [SerializeField]
        private Image _joystickCircle;
        [SerializeField]
        private Image _joystickMoveFocusesTL;
        [SerializeField]
        private Image _joystickMoveFocusesTR;
        [SerializeField]
        private Image _joystickMoveFocusesBL;
        [SerializeField]
        private Image _joystickMoveFocusesBR;

        [SerializeField]
        private float _radius;

        [SerializeField] private InputSystem _inputSystem;

        public override void Initialize()
        {
            base.Initialize();

            _inputSystem = new InputSystem();
        }

        public override void OnShow()
        {
            GameManager.Container.Resolve<TickableManager>().Add( this, -10 );
        }

        public override void OnHide()
        {
            ShowJoystick( false, Vector3.zero );
            GameManager.Container.Resolve<TickableManager>().Remove( this, true );
        }

        public void Tick()
        {
            _inputSystem.ReadInput();
            if ( _inputSystem.TouchInfo.Phase == TouchPhase.Began )
            {
                ShowJoystick( true, _inputSystem.TouchInfo.StartPos );
            }
            else if ( ( _inputSystem.TouchInfo.Phase == TouchPhase.Moved || _inputSystem.TouchInfo.Phase == TouchPhase.Stationary ) )
            {
                GameManager.Container.Resolve<SignalBus>().TryFire( new CharacterMovementDirectionSignal( MoveJoystick( _inputSystem.TouchInfo.Direction ) ) );
            }
            else if ( _inputSystem.TouchInfo.Phase == TouchPhase.Ended )
            {
                ShowJoystick( false, _inputSystem.TouchInfo.StartPos );
            }
        }

        private void ShowJoystick( bool state, Vector3 pos )
        {
            _joystickMoveFocusesTL.enabled = state;
            _joystickMoveFocusesTR.enabled = state;
            _joystickMoveFocusesBL.enabled = state;
            _joystickMoveFocusesBR.enabled = state;

            _joystickRing.rectTransform.position = pos;
            _joystickCircle.rectTransform.position = pos;

            _joystickRing.enabled = state;
            _joystickCircle.enabled = state;

            _radius = _joystickRing.rectTransform.rect.height / 2f - ( ( _joystickCircle.transform as RectTransform ).sizeDelta.x / 2f );
        }

        private Vector3 MoveJoystick( Vector3 pos )
        {
            Vector3 direction = ( new Vector3( pos.x / _radius, pos.y / _radius ) ).normalized;

            SetColor( _joystickMoveFocusesTL, -1f, 1f, direction );
            SetColor( _joystickMoveFocusesTR, 1f, 1f, direction );
            SetColor( _joystickMoveFocusesBL, -1f, -1f, direction );
            SetColor( _joystickMoveFocusesBR, 1f, -1f, direction );

            if ( ( new Vector3( pos.x / _radius, pos.y / _radius ) ).sqrMagnitude > 1 )
            {
                pos = pos.normalized * _radius;
            }
            _joystickCircle.rectTransform.position = _joystickRing.rectTransform.position + pos;
            return new Vector3( pos.x / _radius, pos.y / _radius, pos.z / _radius );
        }

        private void SetColor( Image image, float xMultiple, float yMultiple, Vector3 direction )
        {
            Color sourceColor = image.color;
            float resX = xMultiple * direction.x;
            float resY = yMultiple * direction.y;
            float res = 0f;
            if ( resX > 0 && resY > 0 )
            {
                res = resX * resY * 2f;
            }
            sourceColor.a = res;
            image.color = sourceColor;
        }
    }
}