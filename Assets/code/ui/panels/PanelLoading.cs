using Core.Managers;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PanelLoading : UIBasePanel
    {
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private TMP_Text _sliderLoadingText;

        private int _loadingValue = 0;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnShow()
        {
            DOTween.To( () => _loadingValue, x => _loadingValue = x, 100, 1f )
                .OnUpdate( () =>
                {
                    _slider.value = _loadingValue / 100f;
                    _sliderLoadingText.text = "Loading... " + _loadingValue + " %";
                } )
                .OnComplete( () =>
                {
                    StaticCoroutine.Delay( () => GameManager.Container.Resolve<StateManager>().ChangeState( Enums.GameStates.MainMenu ), 0.5f );
                } );
        }

        public override void OnHide()
        {
        }
    }
}