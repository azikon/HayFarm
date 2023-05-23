using Core.Managers;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PanelPlayPause : UIBasePanel
    {
        [SerializeField]
        private Button buttonOpen;

        public override void Initialize()
        {
            buttonOpen.onClick.AddListener( () =>
            {
                GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_pause_menu" ).Show();
            } );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }
    }
}